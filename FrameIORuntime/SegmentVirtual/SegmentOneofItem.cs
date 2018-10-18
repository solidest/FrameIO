using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    internal class SegmentOneofItem : SegmentFrameRef
    {
        internal long IntoValue { get; private set; }
        internal ushort OutOneOfIdx { get; private set; }
        internal bool IsDefault { get; private set; }
        public SegmentOneofItem(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte pos_isDefault = 6;
            const byte pos_into_value = 16;
            const byte pos_ref_outoneof = 32;
            //const byte POS_REF_FRAME = 48;

            IsDefault = GetTokenBool(token, pos_isDefault);
            IntoValue = GetTokenUShort(token, pos_into_value);
            OutOneOfIdx = GetTokenUShort(token, pos_ref_outoneof);
        }
    }
}
