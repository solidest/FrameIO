using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class InnerSubsys : INotifyPropertyChanged
    {
        public InnerSubsys(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Notes { get; set; }
        public ObservableCollection<SubsysProperty> Propertys { get; set; } = new ObservableCollection<SubsysProperty>();


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
