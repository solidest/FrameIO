using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class SubsysActionMap : INotifyPropertyChanged
    {
        public string Notes { get; set; }
        public string SysPropertyName { get; set; }
        public string FrameSegName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
