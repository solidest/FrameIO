using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class SubsysChannelOption : INotifyPropertyChanged
    {
        public string Notes { get; set; }

        public string Name { get; set; }
        public object OptionValue { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
