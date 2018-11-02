
using System.Text;

namespace FrameIO.Main
{
    using Newtonsoft.Json;
    using PropertyTools.DataAnnotations;

    [JsonObject(MemberSerialization.OptIn)]
    public class FrameSegmentInteger : FrameSegmentBase
    {
        //signed、bitcount、value、byteorder、encoded、repeated
        [Category("Data")]
        [JsonProperty]
        public bool Signed { get; set; } = false;
        [Category("Data")]
        [JsonProperty]
        public int BitCount { get; set; } = 32;
        [Category("Data")]
        [Converter(typeof(ComplexConverter))]
        [JsonProperty]
        public Exp Value { get; set; } =  new Exp() { Op = exptype.EXP_INT, ConstStr="0" };
        [Category("Data")]
        [JsonProperty]
        public ByteOrderType ByteOrder { get; set; } = ByteOrderType.Small;
        [Category("Data")]
        [JsonProperty]
        public EncodedType Encoded { get; set; } = EncodedType.Primitive;

        [Category("Validator")]
        [JsonProperty]
        public string ValidateMax { get; set; } = null;
        [Category("Validator")]
        [JsonProperty]
        public string ValidateMin { get; set; } = null;
        [Browsable(false)]
        [JsonProperty]
        public string VCheckRangeBegin { get; set; } = null;
        [Browsable(false)]
        [JsonProperty]
        public string VCheckRangeEnd { get; set; } = null;
        [Category("Validator")]
        [JsonProperty]
        public CheckType ValidateCheck { get; set; } = CheckType.None;
        [Category("Other")]
        [JsonProperty]
        public string ToEnum { get; set; } = null;

        public override void AppendSegmentCode(StringBuilder code)
        {
            code.Append(string.Format("integer {0}", Name));
            if (Signed) code.Append(" signed=true");
            if (BitCount != 32) code.AppendFormat(" bitcount={0}", BitCount);
            if (ByteOrder == ByteOrderType.Big) code.Append(" byteorder=big");
            if (Encoded != EncodedType.Primitive) code.AppendFormat(" encoded={0}", GetEncodTypeName(Encoded));
            if (Value != null && !Value.IsIntZero()) code.AppendFormat(" value={0}", Value.ToString());
            if (!Repeated.IsIntOne()) code.AppendFormat(" repeated={0}", Repeated.ToString());
            if (ValidateMax != null && ValidateMax.Length>0) code.AppendFormat(" max={0}", ValidateMax);
            if (ValidateMin != null && ValidateMin.Length>0) code.AppendFormat(" min={0}", ValidateMin);
            if (ValidateCheck != CheckType.None) code.AppendFormat(" check={0}", GetCheckName(ValidateCheck));
            if (VCheckRangeBegin != null && VCheckRangeBegin.Length>0) code.AppendFormat(" checkrange=({0},{1})", VCheckRangeBegin, VCheckRangeEnd);
            if (ToEnum != null && ToEnum.Length>0) code.AppendFormat(" toenum={0}", ToEnum);

            code.Append(";" );
        }

        private string GetCheckName(CheckType ty)
        {
            var ret = ty.ToString().ToLower();
            return ret.Substring(6);
        }
    }
}
