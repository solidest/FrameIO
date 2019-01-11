using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class SubsysProperty : INotifyPropertyChanged
    {
        private string _len;
        public string Name { get; set; }
        public string Notes { get; set; }

        public string PropertyType { get; set; }
        public bool IsArray { get; set; }
        public string ArrayLen
        {
            get
            {
                if (IsArray)
                    return _len;
                else
                    return "";
            }
            set
            {
                if (IsArray && value!="0") _len = value;
            }
        }
        public int Syid { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Name;
        }
        public bool IsBaseType()
        {
            return (PropertyType == "bool" || PropertyType == "byte" || PropertyType == "sbyte"
                || PropertyType == "short" || PropertyType == "ushort" || PropertyType == "int" || PropertyType == "uint"
                || PropertyType == "long" || PropertyType == "ulong" || PropertyType == "float" || PropertyType == "double");
        }
    }
}
