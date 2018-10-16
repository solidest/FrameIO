using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    class SegmentBlockIn : SegmentBaseRun
    {
        private ushort _out_seg_idx;
        private ushort _first_childseg_idx;
        private ushort _last_childseg_idx;
        public SegmentBlockIn(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte pos_first_childseg = 6;
            const byte pos_last_childseg = 22;
            //const byte pos_in_seg = 38;
            const byte pos_out_seg = 38;

            _first_childseg_idx = GetTokenUShort(token, pos_first_childseg);
            _last_childseg_idx = GetTokenUShort(token, pos_last_childseg);
            _out_seg_idx = GetTokenUShort(token, pos_out_seg);
        }

        public override ushort GetBitLen(ref int bitlen, SegmentValueInfo info, IRunExp ir)
        {
            var pos = _first_childseg_idx;
            while(pos!= _out_seg_idx)
            {
                var resl = ir.GetBitLen(ref bitlen, pos);
                if (resl == 0)
                    pos += 1;
                else
                    pos = resl;
            }
            return 0;
        }

        public override ushort Pack(IList<ulong> value_buff, MemoryStream pack, ref ulong cach, ref int pos, SegmentValueInfo info, IRunExp ir)
        {
            return 0;
        }

        public override ushort TryUnpack(ushort next_fill_seg, SegmentUnpackInfo info)
        {
            throw new NotImplementedException();
        }

        public override ushort Unpack(byte[] buff, ref int pos_bit, SegmentUnpackInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
