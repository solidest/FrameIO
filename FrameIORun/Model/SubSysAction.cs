using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class SubsysAction : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public actioniotype IOType { get; set; }
        public string ChannelName { get; set; }
        public string FrameName { get; set; }
        public ObservableCollection<SubsysActionMap> Maps { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
