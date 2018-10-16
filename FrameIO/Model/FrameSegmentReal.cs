using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //isdouble、byteorder、value、encoded、repeated
    [Serializable]
    public class FrameSegmentReal:FrameSegmentBase
    {
        public bool IsDouble { get; set; } = false;
        public ByteOrderType ByteOrder { get; set; } = ByteOrderType.Small;
        public EncodedType Encoded { get; set; } = EncodedType.Primitive;

        public Exp Value { get; set; } = new Exp() { Op = exptype.EXP_INT, ConstStr = "0" };

        public string VMax { get; set; } = null;
        public string VMin { get; set; } = null;

    }
}
