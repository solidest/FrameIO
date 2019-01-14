
using System.Text;

namespace FrameIO.Main
{
    using Newtonsoft.Json;
    using PropertyTools.DataAnnotations;

    //isdouble、byteorder、value、encoded、repeated
    [JsonObject(MemberSerialization.OptIn)]
    public class FrameSegmentReal:FrameSegmentBase
    {
        [Category("Data")]
        [JsonProperty]
        public bool IsDouble { get; set; } = false;
        [Category("Data")]
        [JsonProperty]
        public ByteOrderType ByteOrder { get; set; } = ByteOrderType.Small;
        [Category("Data")]
        [JsonProperty]
        public EncodedType Encoded { get; set; } = EncodedType.Primitive;

        [Category("Data")]
        [Converter(typeof(ComplexConverter))]
        [JsonProperty]
        public Exp Value { get; set; } = new Exp() { Op = exptype.EXP_INT, ConstStr = "0" };

        [Category("Validator")]
        [JsonProperty]
        public string ValidateMax { get; set; } = null;
        [Category("Validator")]
        [JsonProperty]
        public string ValidateMin { get; set; } = null;

        public override void AppendSegmentCode(StringBuilder code)
        {
            code.Append(string.Format("real {0}", Name));
            if (IsDouble) code.Append(" isdouble=true");
            if (ByteOrder == ByteOrderType.Big) code.Append(" byteorder=big");
            if (Encoded != EncodedType.Primitive) code.AppendFormat(" encoded={0}", GetEncodTypeName(Encoded));
            if (Value != null && !Value.IsIntZero()) code.AppendFormat(" value={0}", Value.ToString());
            if (!Repeated.IsIntOne()) code.AppendFormat(" repeated={0}", Repeated.ToString());
            if (ValidateMax != null && ValidateMax.Length>0) code.AppendFormat(" max={0}", ValidateMax);
            if (ValidateMin != null && ValidateMin.Length>0) code.AppendFormat(" min={0}", ValidateMin);
            code.Append(";" );
        }
    }
}
