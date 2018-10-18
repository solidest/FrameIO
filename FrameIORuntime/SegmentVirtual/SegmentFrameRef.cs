using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    internal class SegmentFrameRef : SegmentBaseRun
    {
        private ushort _refframe;
        private ushort _myself;
        internal SegmentFrameRef(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte POS_REF_FRAME = 48;
            const byte pos_myself = 16;
            _refframe = GetTokenUShort(token, POS_REF_FRAME);
            _myself = GetTokenUShort(token, pos_myself);
        }

        #region --Pack--

        internal override ushort GetBitLen(ref int bitlen, SetValueInfo info, IPackRunExp ir)
        {
            var fr = (SegmentFramBegin)FrameRuntime.Info[_refframe];
            if (!info.IsSetValue) SetAutoValue(info, fr);
            var fp = (FramePacker)info.Tag;
            //取全部字段的位长
            var pos = (ushort)(fr.BeginIdx+1);
            while (pos != fr.EndIdx)
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
            var fr = (SegmentFramBegin)FrameRuntime.Info[_refframe];
            if (!info.IsSetValue) SetAutoValue(info, fr);

            var fp = (FramePacker)info.Tag;
            ushort idx = (ushort)(fr.BeginIdx + 1);
            while (idx != fr.EndIdx)
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
                var fr = (SegmentFramBegin)FrameRuntime.Info[_refframe];
                var ret = new FramePacker(fr.BeginIdx, fr.EndIdx);
                info.IsSetValue = true;
                info.Tag = ret;
                return ret;
            }
        }

        #endregion

        #region --Unpack--
        internal override bool TryGetBitLen(ref int bitlen, ref ushort nextseg, UnpackInfo info, IUnpackRunExp ir)
        {
            if (!info.IsUnpack) return false;
            var upi = (FrameUnpacker)info.Tag;
            var fr = (SegmentFramBegin)FrameRuntime.Info[_refframe];
            int len = 0;
            var mynextseg = fr.BeginIdx;
            while (mynextseg != fr.EndIdx)
            {
                ushort _nseg = 0;
                if (!FrameRuntime.Info[mynextseg].TryGetBitLen(ref len, ref _nseg, upi.Info[mynextseg], upi)) break;
                if (_nseg == 0)
                    mynextseg += 1;
                else
                    mynextseg = _nseg;
            }
            if (mynextseg == fr.EndIdx)
            {
                nextseg = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        internal override ushort Unpack(byte[] buff, ref int pos_bit, int end_bit_pos, UnpackInfo info, IUnpackRunExp ir)
        {
            var fr = (SegmentFramBegin)FrameRuntime.Info[_refframe];
            if (!info.IsUnpack)
            {
                info.IsUnpack = true;
                info.Tag = new FrameUnpacker(fr.BeginIdx, fr.EndIdx);
            }
            var myup = (FrameUnpacker)info.Tag;
            while(pos_bit!=end_bit_pos && myup.SegPosition!=fr.EndIdx)
            {
                var res = FrameRuntime.Info[myup.SegPosition].Unpack(buff, ref pos_bit, end_bit_pos, myup.Info[myup.SegPosition], myup);
                if (res == 0)
                    myup.SegPosition += 1;
                else
                    myup.SegPosition = res;
            }
            if(myup.SegPosition == fr.EndIdx)
            {
                myup.SegPosition = fr.BeginIdx;
                return 0;
            }
            else
            {
                return _myself;
            }
        }

        #endregion

        #region --SetValue--

        private void SetAutoValue(SetValueInfo inf, SegmentFramBegin fr)
        {
            inf.IsSetValue = true;
            inf.Tag = new FramePackerInfo(fr.BeginIdx, fr.EndIdx);
        }

        #endregion


    }
}
