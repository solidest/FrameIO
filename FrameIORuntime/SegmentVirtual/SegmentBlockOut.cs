using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    internal class SegmentBlockOut : SegmentBaseRun
    {
        private ushort _in_seg_idx;
        private ushort _first_childseg_idx;
        private ushort _last_childseg_idx;
        public SegmentBlockOut(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte pos_first_childseg = 6;
            const byte pos_last_childseg = 22;
            const byte pos_in_seg = 38;
            //const byte pos_out_seg = 38;

            _first_childseg_idx = GetTokenUShort(token, pos_first_childseg);
            _last_childseg_idx = GetTokenUShort(token, pos_last_childseg);
            _in_seg_idx = GetTokenUShort(token, pos_in_seg);
        }

        internal override ushort GetBitLen(ref int bitlen, SetValueInfo info, IPackRunExp ir)
        {
            return 0;
        }

        internal override ushort Pack(MemoryStream value_buff, MemoryStream pack, ref byte odd, ref byte odd_pos, SetValueInfo info, IPackRunExp ir)
        {
            return 0;
        }

        internal override bool TryGetBitLen(ref int bitlen, ref ushort nextseg, UnpackInfo info, IUnpackRunExp ir)
        {
            nextseg = 0;
            return true;
        }

        internal override ushort Unpack(byte[] buff, ref int pos_bit, UnpackInfo info, IUnpackRunExp ir)
        {
            return 0;
        }
    }
}
