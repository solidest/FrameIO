using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{


    internal class SegmentTextRun : SegmentBaseRun
    {
        private ushort _bytesize;
        internal SegmentTextRun(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte pos_bytesize = 48;

            _bytesize = GetTokenUShort(token, pos_bytesize);
        }

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

        internal override ushort Unpack(byte[] buff, ref int pos_bit, UnpackInfo info, IUnpackRunExp ir)
        {
            throw new NotImplementedException();
        }
    }
}
