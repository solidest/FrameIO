using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class FrameSegmentInteger : FrameSegmentBase
    {
        //signed、bitcount、value、byteorder、encoded、repeated
        public bool Signed { get; set; } = false;
        public int BitCount { get; set; } = 32;
        public string Value { get; set; } = "0";
        public ByteOrderType ByteOrder { get; set; } = ByteOrderType.Small;
        public EncodedType Encoded { get; set; } = EncodedType.Primitive;

        public string VMax { get; set; } = null;
        public string VMin { get; set; } = null;
        public string VCheckRangeBegin { get; set; } = null;
        public string VCheckRangeEnd { get; set; } = null;
        public CheckType VCheck { get; set; } = CheckType.None;
        public string VToEnum { get; set; } = null;

    }
}
