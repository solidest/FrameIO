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

        internal ushort BeginIdx { get; private set; }
        internal ushort EndIdx { get; private set; }

        internal override ushort GetBitLen(ref int bitlen, SetValueInfo info, IPackRunExp ir)
        {
            throw new NotImplementedException();
        }

        internal override ushort Pack(MemoryStream value_buff, MemoryStream pack, ref byte odd, ref byte odd_pos, SetValueInfo info, IPackRunExp ir)
        {
            throw new NotImplementedException();
        }

        internal override bool TryGetBitLen(ref int bitlen, ref ushort nextseg, UnpackInfo info, IUnpackRunExp ir)
        {
            throw new NotImplementedException();
        }

        internal override ushort Unpack(byte[] buff, ref int pos_bit, int end_bit_pos, UnpackInfo info, IUnpackRunExp ir)
        {
            throw new NotImplementedException();
        }
    }
}
