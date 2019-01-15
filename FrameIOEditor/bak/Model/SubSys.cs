using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class Subsys : INotifyPropertyChanged
    {
        public Subsys(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int Syid { get; set; }
        public ObservableCollection<SubsysChannel> Channels { get; set; } = new ObservableCollection<SubsysChannel>();
        public ObservableCollection<SubsysAction> Actions { get; set; } = new ObservableCollection<SubsysAction>();
        public ObservableCollection<SubsysProperty> Propertys { get; set; } = new ObservableCollection<SubsysProperty>();


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
