using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    internal class SegmentFrameRun : SegmentBaseRun
    {
        private ushort _beginidx;
        private ushort _endidx;
        internal SegmentFrameRun(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte pos_refBegin = 16;
            const byte pos_refEnd = 32;
            _beginidx = GetTokenUShort(token, pos_refBegin);
            _endidx = GetTokenUShort(token, pos_refEnd);
        }

        #region --Pack--

        internal override ushort GetBitLen(ref int bitlen, SetValueInfo info, IPackRunExp ir)
        {
            if (!info.IsSetValue) SetAutoValue(info);
            var fp = (FramePacker)info.Tag;
            //取全部字段的位长
            var pos = (ushort)(_beginidx+1);
            while (pos != _endidx)
            {
                var res = FrameRuntime.Info[pos].GetBitLen(ref bitlen, fp.Info[pos], fp);
                if (res == 0)
                    pos += 1;
                else
                    pos = res;
            }
            return 0;
        }

        internal override ushort Pack(MemoryStream value_buff, MemoryStream pack, ref byte odd, ref byte oddlen, SetValueInfo info, IPackRunExp ir)
        {
            if (!info.IsSetValue) SetAutoValue(info);

            var fp = (FramePacker)info.Tag;
            ushort idx = (ushort)(_beginidx + 1);
            while (idx != _endidx)
            {
                var result = FrameRuntime.Info[idx].Pack(value_buff, pack, ref odd, ref oddlen, fp.Info[idx], fp);
                if (result == 0)
                    idx += 1;
                else
                    idx = result;
            }
            return 0;

        }

        internal FramePacker GetSegmentSettor(MemoryStream value_buff, SetValueInfo info)
        {
            if (info.IsSetValue)
                return (FramePacker)info.Tag;
            else
            {
                var ret = new FramePacker(_beginidx, _endidx);
                info.IsSetValue = true;
                info.Tag = ret;
                return ret;
            }
        }

        #endregion

        #region --Unpack--
        internal override bool TryGetBitLen(ref int bitlen, ref ushort nextseg, UnpackInfo info, IUnpackRunExp ir)
        {
            throw new NotImplementedException();
        }

        internal override ushort Unpack(byte[] buff, ref int pos_bit, UnpackInfo info, IUnpackRunExp ir)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region --SetValue--

        private void SetAutoValue(SetValueInfo inf)
        {
            inf.IsSetValue = true;
            inf.Tag = new FramePackerInfo(_beginidx, _endidx);
        }

        #endregion


    }
}
