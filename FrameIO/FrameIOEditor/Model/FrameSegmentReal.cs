using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    using PropertyTools.DataAnnotations;
    //isdouble、byteorder、value、encoded、repeated
    public class FrameSegmentReal:FrameSegmentBase
    {
        [Category("Data")]
        public bool IsDouble { get; set; } = false;
        [Category("Data")]
        public ByteOrderType ByteOrder { get; set; } = ByteOrderType.Small;
        [Category("Data")]
        public EncodedType Encoded { get; set; } = EncodedType.Primitive;

        [Category("Data")]
        [Converter(typeof(ComplexConverter))]
        public Exp Value { get; set; } = new Exp() { Op = exptype.EXP_INT, ConstStr = "0" };

        [Category("Validator")]
        public string ValidateMax { get; set; } = null;
        [Category("Validator")]
        public string ValidateMin { get; set; } = null;

    }
}
