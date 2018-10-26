using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class SubsysChannel : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public syschanneltype ChannelType { get; set; }
        public int Syid { get; set; }

        public ObservableCollection<SubsysChannelOption> Options { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
