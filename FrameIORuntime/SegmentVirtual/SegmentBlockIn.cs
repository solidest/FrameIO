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

        #region --Pack--

        public override ushort GetBitLen(ref int bitlen, SegmentValueInfo info, IPackRunExp ir)
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

        public override ushort Pack(IList<ulong> value_buff, MemoryStream pack, ref ulong cach, ref int pos, SegmentValueInfo info, IPackRunExp ir)
        {
            return 0;
        }

        #endregion

        #region --Unpack--

        public override bool TryGetBitLen(ref int bitlen, ref ushort nextseg, SegmentUnpackInfo info, IUnpackRunExp ir)
        {
            var pos = _first_childseg_idx;
            while (pos != _out_seg_idx)
            {
                ushort resl = 0;
                if (ir.TryGetBitLen(ref bitlen, ref resl, pos))
                {
                    nextseg = pos;
                    if (resl == 0)
                        pos += 1;
                    else
                        pos = resl;
                }
                else
                    return false;
            }

            return true;
        }

        public override ushort Unpack(byte[] buff, ref int pos_bit, SegmentUnpackInfo info, IUnpackRunExp ir)
        {
            return 0; 
        }

        #endregion

    }
}
