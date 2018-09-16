using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO
{
    public class Subsys: INotifyPropertyChanged
    {
        public Subsys(string name)
        {
            SubsysName = name;
        }
        public string SubsysName { get; set; }
        public string SubsysNotes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
