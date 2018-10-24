using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    internal class SegmentFramBegin : SegmentBaseRun
    {
        internal SegmentFramBegin(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte pos_refBegin = 16;
            const byte pos_refEnd = 32;
            BeginIdx = GetTokenUShort(token, pos_refBegin);
            EndIdx = GetTokenUShort(token, pos_refEnd);
        }

        internal FramePacker GetFramePacker()
        {
            return new FramePacker(BeginIdx, EndIdx);
        }
        internal FrameUnpacker GetFrameUnpacker()
        {
            return new FrameUnpacker(BeginIdx, EndIdx, null);
        }


        internal ushort BeginIdx { get; private set; }
        internal ushort EndIdx { get; private set; }

        internal override ushort GetBitLen(MemoryStream value_buff, ref int bitlen, SetValueInfo info, IPackRunExp ir)
        {
            var pos = (ushort)(BeginIdx + 1);
            while (pos != EndIdx)
            {
                var res = FrameRuntime.Run[pos].GetBitLen(value_buff, ref bitlen, ir.GetSetValueInfo(pos), ir);
                if (res == 0)
                    pos += 1;
                else
                    pos = res;
            }
            return 0;

        }

        internal override ushort Pack(MemoryStream value_buff, MemoryStream pack, ref byte odd, ref byte odd_pos, SetValueInfo info, IPackRunExp ir)
        {
            throw new Exception("runtime");
        }

        internal override bool TryGetNeedBitLen(byte[] buff, ref int bitlen, ref ushort nextseg, UnpackInfo info, IUnpackRunExp ir)
        {
            throw new Exception("runtime");
        }


        internal override ushort Unpack(byte[] buff, ref int pos_bit, int end_bit_pos, UnpackInfo info, IUnpackRunExp ir)
        {
            throw new Exception("runtime");
        }
    }
}
