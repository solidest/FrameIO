using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal abstract class SegRunNumber : SegRunValue
    {

        //取字段的内存流
        internal abstract ulong GetRaw(IFrameWriteBuffer buff, JValue v);
        internal abstract object FromRaw(ulong v);
        protected abstract void DoValid(IFrameReadBuffer buff, SegRunNumber seg, JToken value);

        #region --Pack--

        //打包
        public override void Pack(IFrameWriteBuffer buff, JObject parent, JToken theValue)
        {
            if (IsArray)
            {
                SegRunArray.Pack(ArrayLen, PackItem, this, buff, parent, (JArray)theValue);
            }
            else
                PackItem(buff, parent, theValue);
        }

        private void PackItem(IFrameWriteBuffer buff, JObject parent, JToken theValue)
        {
            buff.Write(GetRaw(buff, (JValue)theValue), BitLen, theValue);
        }

        public override int GetBitLen(JObject parent)
        {
            return (ArrayLen != null) ? SegRunArray.GetBitLen(ArrayLen, GetItemBitLen, this, parent) : GetItemBitLen(parent, parent[Name]);
        }

        public int GetItemBitLen(JObject parent, JToken theValue)
        {
            return BitLen;
        }

        #endregion

        #region --Unpack--

        public override JValue UnpackValue(IFrameReadBuffer buff, JContainer pc)
        {
            var v = (JValue)GetDefaultValue();
            v.Value = buff.ReadBits(BitLen, v);

            if (IsArray)
                pc.Add(v);
            else
                ((JObject)pc).Add(Name, v);
            return v;
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
