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

        public override void AppendSegmentCode(StringBuilder code)
        {
            code.Append(string.Format("real {0}", Name));
            if (IsDouble) code.Append(" isdouble=true");
            if (ByteOrder == ByteOrderType.Big) code.Append(" byteorder=big");
            if (Encoded != EncodedType.Primitive) code.AppendFormat(" encoded={0}", GetEncodTypeName(Encoded));
            if (Value != null && !Value.IsIntZero()) code.AppendFormat(" value={0}", Value.ToString());
            if (ValidateMax != null && ValidateMax.Length>0) code.AppendFormat(" max={0}", ValidateMax);
            if (ValidateMin != null && ValidateMin.Length>0) code.AppendFormat(" min={0}", ValidateMin);
            code.Append(";" + Environment.NewLine);
        }
    }
}
