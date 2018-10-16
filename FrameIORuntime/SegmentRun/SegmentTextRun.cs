﻿using System;
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

        public override ushort GetBitLen(ref int bitlen, SegmentValueInfo info, IPackRunExp ir)
        {
            throw new NotImplementedException();
        }

        public override ushort Pack(IList<ulong> value_buff, MemoryStream pack, ref ulong cach, ref int pos, SegmentValueInfo info, IPackRunExp ir)
        {
            throw new NotImplementedException();
        }

        public override bool TryGetBitLen(ref int bitlen, ref ushort nextseg, SegmentUnpackInfo info, IUnpackRunExp ir)
        {
            throw new NotImplementedException();
        }

        public override ushort Unpack(byte[] buff, ref int pos_bit, SegmentUnpackInfo info, IUnpackRunExp ir)
        {
            throw new NotImplementedException();
        }

    }
}
