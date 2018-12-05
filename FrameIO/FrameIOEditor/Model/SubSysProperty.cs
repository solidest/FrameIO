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
        public string Name { get; set; }
        public string Notes { get; set; }
        //public syspropertytype PropertyType { get; set; } = syspropertytype.SYSPT_UINT; //HACK 1111 修改类型为string
        public string PropertyType { get; set; }
        public bool IsArray { get; set; }
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
