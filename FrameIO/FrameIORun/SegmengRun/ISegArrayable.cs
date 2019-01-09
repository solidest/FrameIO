using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal interface ISegArrayable : ISegRun
    {
        bool TryGetItemBitLen(ref int len, JObject parent, JToken theValue);
        int GetItemBitLen(JObject parent, JToken theValue);
        ISegRun PackItem(IFrameWriteBuffer buff, JObject parent, JToken theValue);
        ISegRun UnPackItem(IFrameReadBuffer buff, JObject parent, JToken theValue, JArray mycontainer);
        bool IsArray { get; }
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
            var vs = GetMyValues(parent);

            for (int i = 0; i < vs.Count; i++)
            {
                _item.PackItem(buff, parent, vs[i]);
            }

            return _item.Next;
        }

        public ISegRun UnPack(IFrameReadBuffer buff, JObject parent, JToken theValue)
        {
            JArray my = theValue?.Value<JArray>();
            var len = _arrLen.GetLong(parent, _item.Parent);
            int repeated = 0;

            if(my == null)
            {
                my = new JArray();
                parent.Add(_item.Name, my);
            }
            else
            {
                repeated = buff.LoadRepeated(my);
            }

            for (int i=repeated; i<len; i++)
            {
                if (!buff.CanRead) break;
                _item.UnPackItem(buff, parent,i<my.Count?my[i]:null, my);
                repeated = i;
            }
            if (repeated == len - 1)
                return _item.Next;
            else
                return _item;

        }

        public int GetBitLen(JObject parent)
        {
            var vs = GetMyValues(parent);
            int ret = 0;
            for(int i=0; i<vs.Count; i++)
            {
                ret += _item.GetItemBitLen(parent, vs[i]);
            }
            return ret;
        }

        public bool TryGetBitLen(ref int len, JObject parent)
        {
            long repeated = 0;
            var vs = parent?[_item.Name]?.Value<JArray>();
            if (!_arrLen.TryGetLong(parent, _item.Parent, ref repeated))
                return false;

            int mylen = 0;

            for (int i = 0; i < repeated; i++)
            {
                if (!_item.TryGetItemBitLen(ref mylen, parent, vs?[i]))
                {
                    len += mylen;
                    return false;
                }
            }
            len += mylen;
            return true;
        }

        private JArray GetMyValues(JObject parent)
        {
            var vs = parent?[_item.Name]?.Value<JArray>();
            int len = (int)_arrLen.GetLong(parent, _item.Parent);

            if (vs == null)
            {
                _item.LogError(Interface.FrameIOErrorType.SendErr, "数组未初始化");
            }
            else if (vs.Count != len)
            {
                _item.LogError(Interface.FrameIOErrorType.SendErr, "数组初始化长度错误");
            }
            return vs;
        }
    }
}
