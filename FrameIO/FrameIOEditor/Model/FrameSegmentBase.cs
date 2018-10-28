using PropertyTools.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    using PropertyTools.DataAnnotations;

    public abstract class FrameSegmentBase : INotifyPropertyChanged
    {

        [Category("Other")]
        public string Notes { get; set; }
        [Category("Data")]
        [Converter(typeof(ComplexConverter))]
        public Exp Repeated { get; set; } = new Exp() { Op = exptype.EXP_INT, ConstStr="1" };


        [Category("Main")]
        [ReadOnly(true)]
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
