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
        public syspropertytype PropertyType { get; set; } = syspropertytype.SYSPT_UINT;
        public bool IsArray { get; set; }
        public int Syid { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Name;
        }
    }
}
