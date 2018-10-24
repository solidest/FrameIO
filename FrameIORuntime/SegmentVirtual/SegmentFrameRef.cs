using FrameIO.Interface;
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

        internal SegmentFrameRef(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte POS_REF_FRAME = 48;
            _refframe = GetTokenUShort(token, POS_REF_FRAME);

        }

        #region --Pack--

        internal override ushort GetBitLen(MemoryStream value_buff, ref int bitlen, SetValueInfo info, IPackRunExp ir)
        {
            var fr = (SegmentFramBegin)FrameRuntime.Run[_refframe];
            if (!info.IsSetValue) SetAutoValue(info, fr);
            var fp = (FramePacker)info.Tag;
            //取全部字段的位长
            fr.GetBitLen(fp.Info.Cach, ref bitlen, info, fp);
            return 0;
        }

        internal override ushort Pack(MemoryStream value_buff, MemoryStream pack, ref byte odd, ref byte oddlen, SetValueInfo info, IPackRunExp ir)
        {
            var fr = (SegmentFramBegin)FrameRuntime.Run[_refframe];
            if (!info.IsSetValue) SetAutoValue(info, fr);

            var fp = (FramePacker)info.Tag;
            ushort idx = (ushort)(fr.BeginIdx + 1);
            while (idx != fr.EndIdx)
            {
                var result = FrameRuntime.Run[idx].Pack(fp.Info.Cach, pack, ref odd, ref oddlen, fp.Info[idx], fp);
                if (result == 0)
                    idx += 1;
                else
                    idx = result;
            }
            return 0;

        }

        internal FramePacker GetSegmentSettor(SetValueInfo info)
        {
            if (info.IsSetValue)
                return (FramePacker)info.Tag;
            else
            {
                var fr = (SegmentFramBegin)FrameRuntime.Run[_refframe];
                var ret = new FramePacker(fr.BeginIdx, fr.EndIdx);
                info.IsSetValue = true;
                info.Tag = ret;
                return ret;
            }
        }

        #endregion

        #region --Unpack--

        internal override bool TryGetNeedBitLen(byte[] buff, ref int bitlen, ref ushort nextseg, UnpackInfo info, IUnpackRunExp ir)
        {
            var fr = (SegmentFramBegin)FrameRuntime.Run[_refframe];
            if (info.Tag==null) info.Tag =  new FrameUnpacker(fr.BeginIdx, fr.EndIdx, null);

            var myup = (FrameUnpacker)info.Tag;

            var mynextseg = (ushort)(fr.BeginIdx+1);
            while (mynextseg != fr.EndIdx)
            {
                ushort _nseg = 0;
                if (!FrameRuntime.Run[mynextseg].TryGetNeedBitLen(buff, ref bitlen, ref _nseg, myup.Info[mynextseg], myup))
                {
                    nextseg = ushort.MaxValue;
                    return false;
                }
                if (_nseg == 0)
                    mynextseg += 1;
                else
                    mynextseg = _nseg;
            }
            nextseg = 0;
            return true;

        }

        internal override ushort Unpack(byte[] buff, ref int pos_bit, int end_bit_pos, UnpackInfo info, IUnpackRunExp ir)
        {
            
            var fr = (SegmentFramBegin)FrameRuntime.Run[_refframe];
            if (info.Tag == null) info.Tag = new FrameUnpacker(fr.BeginIdx, fr.EndIdx, null);
            var myup = (FrameUnpacker)info.Tag;

            while (pos_bit!=end_bit_pos && myup.SegPosition!=fr.EndIdx)
            {
                var res = FrameRuntime.Run[myup.SegPosition].Unpack(buff, ref pos_bit, end_bit_pos, myup.Info[myup.SegPosition], myup);
                if (res == 0)
                    myup.SegPosition += 1;
                else
                    myup.SegPosition = res;
            }
            if(myup.SegPosition == fr.EndIdx)
            {
                myup.SegPosition = (ushort)(fr.BeginIdx+1);
                return 0;
            }
            else
            {
                return ushort.MaxValue;
            }
        }

        #endregion

        #region --SetValue--

        private void SetAutoValue(SetValueInfo inf, SegmentFramBegin fr)
        {
            inf.IsSetValue = true;
            inf.Tag = new FramePacker(fr.BeginIdx, fr.EndIdx);
        }

        #endregion


    }
}
