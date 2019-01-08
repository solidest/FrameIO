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
        internal abstract ulong GetRaw(IFrameBuffer buff, JValue v);

        //取自动计算值
        internal abstract JValue GetAutoValue(IFrameBuffer buff, JObject parent);

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
        public override ISegRun Pack(IFrameBuffer buff, JObject parent)
        {
            return IsArray ? _arr.Pack(buff, parent) : PackItem(buff, parent);
        }

        public ISegRun PackItem(IFrameBuffer buff, JObject parent)
        {
            var v = parent?[Name]?.Value<JValue>();
            if(v==null)
            {
                v = GetAutoValue(buff, parent);
                parent?.Add(Name, v);
            }
            buff.Write(Slice.GetSlice(GetRaw(buff, v), BitLen), parent?[Name]);
            return Next;
        }

        //取位长
        public override int GetBitLen(IFrameBuffer buff, JObject parent)
        {
            return IsArray ? _arr.GetBitLen(buff, parent) : GetItemBitLen(buff, parent);
        }

        public int GetItemBitLen(IFrameBuffer buff, JObject parent)
        {
            return BitLen;
        }

        #endregion

        #region --UnPack--

        //尝试取比特位长
        public override bool TryGetBitLen(IFrameBuffer buff, ref int len, JObject parent)
        {
            return IsArray ? _arr.TryGetBitLen(buff, ref len, parent) : TryGetItemBitLen(buff, ref len, parent);
        }

        public bool TryGetItemBitLen(IFrameBuffer buff, ref int len, JObject parent)
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
