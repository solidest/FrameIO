using System;
using System.ComponentModel;
using System.Text;


namespace FrameIO.Main
{
    using Newtonsoft.Json;
    using PropertyTools.DataAnnotations;
    using System.Runtime.Serialization;

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class FrameSegmentBase : INotifyPropertyChanged
    {

        [Category("Other")]
        [JsonProperty]
        public string Notes { get; set; }
        [Category("Data")]
        [Converter(typeof(ComplexConverter))]
        [JsonProperty]
        public Exp Repeated { get; set; } = new Exp() { Op = exptype.EXP_INT, ConstStr="1" };


        [Category("Main")]
        [ReadOnly(true)]
        [JsonProperty]
        public string Name { get; set; }

        [Browsable(false)]
        public int Syid { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public abstract void AppendSegmentCode(StringBuilder code);

        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;

        protected string GetEncodTypeName(EncodedType ty )
        {
            switch (ty)
            {
                case EncodedType.Primitive:
                    return "primitive";
                case EncodedType.Inversion:
                    return "inversion";
                case EncodedType.Complement:
                    return "complement";
            }
            return "";
        }

    }
}
