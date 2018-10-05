using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class Frame:INotifyPropertyChanged
    {
        public Frame(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Notes { get; set; }

        public ObservableCollection<FrameSegmentBase> Channels { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
