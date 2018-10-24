using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    internal class SegmentBlockIn : SegmentBaseRun
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

        internal override ushort GetBitLen(MemoryStream value_buff, ref int bitlen, SetValueInfo info, IPackRunExp ir)
        {
            var pos = _first_childseg_idx;
            while(pos!= _out_seg_idx)
            {
                var resl = ir.GetBitLen(value_buff,ref bitlen, pos);
                if (resl == 0)
                    pos += 1;
                else
                    pos = resl;
            }
            return 0;
        }

        internal override ushort Pack(MemoryStream value_buff, MemoryStream pack, ref byte odd, ref byte odd_pos, SetValueInfo info, IPackRunExp ir)
        {
            return 0;
        }

        #endregion

        #region --Unpack--

        internal override bool TryGetNeedBitLen(byte[] buff, ref int bitlen, ref ushort nextseg, UnpackInfo info, IUnpackRunExp ir)
        {
            nextseg = 0;
            return true;
        }

        internal override ushort Unpack(byte[] buff, ref int pos_bit, int end_bit_pos, UnpackInfo info, IUnpackRunExp ir)
        {
            return 0; 
        }

        #endregion

    }
}
