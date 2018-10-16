using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{


    public class SegmentTextRun : SegmentBaseRun
    {
        private ushort _bytesize;
        public SegmentTextRun(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte pos_bytesize = 48;

            _bytesize = GetTokenUShort(token, pos_bytesize);
        }

        public override ushort GetBitLen(ref int bitlen, SegmentValueInfo info, IRunExp ir)
        {
            throw new NotImplementedException();
        }

        public override ushort Pack(IList<ulong> value_buff, MemoryStream pack, ref ulong cach, ref int pos, SegmentValueInfo info, IRunExp ir)
        {
            throw new NotImplementedException();
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
