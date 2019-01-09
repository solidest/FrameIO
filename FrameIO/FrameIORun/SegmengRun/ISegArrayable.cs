﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal interface ISegArrayable : ISegRun
    {
        bool GetItemNeedBitLen(ref int len, out ISegRun next, JObject parent, JToken theValue);
        int GetItemBitLen(JObject parent, JToken theValue);
        ISegRun PackItem(IFrameWriteBuffer buff, JObject parent, JArray arr, JToken theValue);
        ISegRun UnpackItem(IFrameReadBuffer buff, JObject parent, JArray arr, JToken theValue);
    }

    internal class SegRunArrayWrapper
    {
        private ISegArrayable _item;
        private IExpRun _arrLen;

        public SegRunArrayWrapper(ISegArrayable item, JObject o)
        {
            _arrLen = Helper.GetExp(o[SegRunBase.ARRAYLEN_TOKEN]);
            _item = item;
        }

        public ISegRun Pack(IFrameWriteBuffer buff, JObject parent)
        {
            int len = 0;
            var vs = GetMyValues(parent, out len);

            for (int i = 0; i < len; i++)
            {
                _item.PackItem(buff, parent, vs, i<vs.Count?vs[i]:null);
            }

            return _item.Next;
        }

        public ISegRun Unpack(IFrameReadBuffer buff, JObject parent)
        {
            int repeated = 0;
            if (parent == null)
            {
                var pos = (StopPosition)buff.StopPosition;
                parent = pos.Parent;
                repeated = pos.Index;
            }

            var my = parent[_item.Name]?.Value<JArray>();
            if(my == null)
            {
                my = new JArray();
                parent.Add(_item.Name, my);
            }

            var len = _arrLen.GetLong(parent, _item.Parent);

            for (int i=repeated; i<len; i++)
            {
                if (!buff.CanRead)
                {
                    buff.StopPosition = new StopPosition() { Parent = parent, Index = repeated };
                    return _item;
                }
                var ret = _item.UnpackItem(buff, parent, my, i<my.Count?my[i]:null);
                Debug.Assert(ret == _item.Next);
            }

            return _item.Next;

        }

        public int GetBitLen(JObject parent)
        {
            int len = 0;
            var vs = GetMyValues(parent, out len);
            int ret = 0;
            for(int i=0; i<len; i++)
            {
                ret += _item.GetItemBitLen(parent, i<vs.Count?vs[i]:null);
            }
            return ret;
        }

        public bool GetNeedBitLen(ref int len, out ISegRun next, JObject parent)
        {
            next = _item;
            long repeated = 0;
            var vs = parent?[_item.Name]?.Value<JArray>();
            if (!_arrLen.HaveValue(parent, _item))
            {
                return false;
            }

            for (int i = 0; i < repeated; i++)
            {
                int mylen = 0;
                ISegRun nullnext = null;
                if (!_item.GetItemNeedBitLen(ref mylen, out nullnext, parent, vs?[i]))
                {
                    return false;
                }
                else
                {
                    len += mylen;
                }
            }

            next = _item.Next;
            return true;
        }

        private JArray GetMyValues(JObject parent, out int len)
        {
            var vs = parent?[_item.Name]?.Value<JArray>();
            len = (int)_arrLen.GetLong(parent, _item.Parent);

            if (vs == null)
            {
                vs = new JArray();
                parent.Add(_item.Name, vs);
            }
            else if (vs.Count != len)
            {
                _item.LogError(Interface.FrameIOErrorType.SendErr, "数组长度错误");
            }
            return vs;
        }
    }
}
