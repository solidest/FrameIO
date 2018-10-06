using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //isdouble、byteorder、value、encoded、repeated
    public class FrameSegmentReal:FrameSegmentBase
    {
        public bool IsDouble { get; set; } = false;
        public ByteOrderType ByteOrder { get; set; } = ByteOrderType.Small;
        public EncodedType Encoded { get; set; } = EncodedType.Primitive;

        public string Value { get; set; } = "0";

        public string VMax { get; set; } = null;
        public string VMin { get; set; } = null;
    }
}
