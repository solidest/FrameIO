using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

        //取自动计算值
        internal abstract JValue GetAutoValue(IFrameWriteBuffer buff, JObject parent);

        #region --Array--

        private SegRunArrayWrapper _arr;
        public bool IsArray { get; private set; } = false;

        protected void InitialArray(JObject o)
        {
            IsArray = true;
            _arr = new SegRunArrayWrapper(this, o);
        }

        #endregion

        #region --Pack--

        //打包
        public override ISegRun Pack(IFrameWriteBuffer buff, JObject parent)
        {
            return IsArray ? _arr.Pack(buff, parent) : PackItem(buff, parent, parent?[Name]);
        }

        public ISegRun PackItem(IFrameWriteBuffer buff, JObject parent, JToken theValue)
        {
            if (theValue == null)
            {
                theValue = GetAutoValue(buff, parent);
            }
            buff.Write(GetRaw(buff, (JValue)theValue), BitLen, theValue);
            return Next;
        }


        //取位长
        public override int GetBitLen(JObject parent)
        {
            return IsArray ? _arr.GetBitLen(parent) : GetItemBitLen(parent, parent?[Name]);
        }

        public int GetItemBitLen(JObject parent, JToken theValue)
        {
            return BitLen;
        }

        #endregion

        #region --UnPack--

        public override ISegRun UnPack(IFrameReadBuffer buff, JObject parent, JToken theValue)
        {
            return IsArray ? _arr.UnPack(buff, parent, parent[Name]) : UnPackItem(buff, parent, parent[Name], null);
        }


        public ISegRun UnPackItem(IFrameReadBuffer buff, JObject parent, JToken theValue, JArray mycontainer)
        {
            var vt = (JValue)theValue ?? (new JValue(0));
            var raw = buff.Read(BitLen, vt);
            vt.Value = FromRaw(raw);
            if (mycontainer != null)
                mycontainer.Add(vt);
            else
                parent.Add(Name, vt);
            return Next;
        }


        //尝试取比特位长
        public override bool TryGetBitLen(ref int len, JObject parent)
        {
            return IsArray ? _arr.TryGetBitLen(ref len, parent) : TryGetItemBitLen(ref len, parent, parent?[Name]);
        }

        public bool TryGetItemBitLen(ref int len, JObject parent, JToken theValue)
        {
            len += BitLen;
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
