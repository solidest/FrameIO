using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    class SegmentBlockOut : SegmentBaseRun
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

        public override ushort GetBitLen(ref int bitlen, SegmentValueInfo info, IPackRunExp ir)
        {
            return 0;
        }

        public override ushort Pack(IList<ulong> value_buff, MemoryStream pack, ref ulong cach, ref int pos, SegmentValueInfo info, IPackRunExp ir)
        {
            return 0;
        }

        public override bool TryGetBitLen(ref int bitlen, ref ushort nextseg, SegmentUnpackInfo info, IUnpackRunExp ir)
        {
            nextseg = 0;
            return true;
        }

        public override ushort Unpack(byte[] buff, ref int pos_bit, SegmentUnpackInfo info, IUnpackRunExp ir)
        {
            return 0;
        }
    }
}
