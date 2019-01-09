using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal abstract class SegRunValue : SegRunBase, ISegArrayable
    {
        //单个字段的比特长度
        internal abstract int BitLen { get;}

        //取字段的内存流
        internal abstract ulong GetRaw(IFrameWriteBuffer buff, JValue v);
        internal abstract object FromRaw(ulong v);
        protected abstract void DoValid(IFrameReadBuffer buff, SegRunValue seg, JToken value);

        //取自动计算值
        internal abstract JValue GetAutoValue(IFrameWriteBuffer buff, JObject parent);

        #region --Array--

        private SegRunArrayWrapper _arr;
        private bool _isarr = false;

        protected void InitialArray(JObject o)
        {
            _isarr = true;
            _arr = new SegRunArrayWrapper(this, o);
        }

        #endregion

        #region --Pack--

        //打包
        public override ISegRun Pack(IFrameWriteBuffer buff, JObject parent)
        {
            return _isarr ? _arr.Pack(buff, parent) : PackItem(buff, parent, null, parent?[Name]);
        }

        public ISegRun PackItem(IFrameWriteBuffer buff, JObject parent, JArray arr, JToken theValue)
        {
            if (theValue == null)
            {
                theValue = GetAutoValue(buff, parent);
                if (arr != null)
                    arr.Add(theValue);
                else
                    parent.Add(theValue);
            }
            buff.Write(GetRaw(buff, (JValue)theValue), BitLen, theValue);
            return Next;
        }

        public override int GetBitLen(JObject parent)
        {
            return _isarr ? _arr.GetBitLen(parent) : BitLen;
        }

        public int GetItemBitLen(JObject parent, JToken theValue)
        {
            return BitLen;
        }

        #endregion

        #region --Unpack--

        public override ISegRun Unpack(IFrameReadBuffer buff, JObject parent)
        {
            return _isarr ? _arr.Unpack(buff, parent) : UnpackItem(buff, parent, null, null);
        }


        public ISegRun UnpackItem(IFrameReadBuffer buff, JObject parent, JArray arr, JToken theValue)
        {
            if (parent == null) parent = ((StopPosition)buff.StopPosition).Parent;
            Debug.Assert(theValue == null);
            var vt = new JValue(0);
            var raw = buff.Read(BitLen, vt);
            vt.Value = FromRaw(raw);
            if (arr != null)
                arr.Add(vt);
            else
                parent.Add(Name, vt);
            DoValid(buff, this, vt);
            return Next;
        }


        //尝试取比特位长
        public override bool GetNeedBitLen(ref int len, out ISegRun next, JObject parent)
        {
            return _isarr ? _arr.GetNeedBitLen(ref len, out next, parent) : GetItemNeedBitLen(ref len, out next, parent, parent?[Name]);
        }

        public bool GetItemNeedBitLen(ref int len, out ISegRun next, JObject parent, JToken theValue)
        {
            len += BitLen;
            next = Next;
            return true;
        }

        #endregion

        #region --Helper--


        //取负数的反码
        protected ulong GetInversion(ulong value)
        {
            return (value & (~(ulong)0 << (BitLen - 1))) | ((~value) & (~(ulong)0 >> (64 - BitLen)));
        }

        //取负数的补码
        protected ulong GetComplement(ulong value)
        {
            return GetInversion(value) + 1;
        }

        //转大端序
        protected ulong GetBigOrder(ulong value)
        {
            var oldv = BitConverter.GetBytes(value);
            var newv = new byte[8];

            int bcount = BitLen / 8;
            if (BitLen % 8 != 0) bcount += 1;
            var oldi = bcount;
            for (int i = 0; i < bcount; i++)
            {
                newv[i] = oldv[oldi - 1];
                oldi -= 1;
            }
            return BitConverter.ToUInt64(newv, 0);
        }


        #endregion

    }
}
