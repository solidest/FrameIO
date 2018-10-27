using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    using PropertyTools.DataAnnotations;

    public class FrameSegmentInteger : FrameSegmentBase
    {
        //signed、bitcount、value、byteorder、encoded、repeated
        [Category("Data")]
        public bool Signed { get; set; } = false;
        [Category("Data")]
        public int BitCount { get; set; } = 32;
        [Category("Data")]
        [Converter(typeof(ComplexConverter))]
        public Exp Value { get; set; } =  new Exp() { Op = exptype.EXP_INT, ConstStr="0" };
        [Category("Data")]
        public ByteOrderType ByteOrder { get; set; } = ByteOrderType.Small;
        [Category("Data")]
        public EncodedType Encoded { get; set; } = EncodedType.Primitive;

        [Category("Validator")]
        public string ValidateMax { get; set; } = null;
        [Category("Validator")]
        public string ValidateMin { get; set; } = null;
        [Browsable(false)]
        public string VCheckRangeBegin { get; set; } = null;
        [Browsable(false)]
        public string VCheckRangeEnd { get; set; } = null;
        [Category("Validator")]
        public CheckType ValidateCheck { get; set; } = CheckType.None;
        [Category("Other")]
        public string ToEnum { get; set; } = null;

    }
}
