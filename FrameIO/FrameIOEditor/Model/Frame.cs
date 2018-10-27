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
        public int Syid { get; set; }
        public string Notes { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public ObservableCollection<FrameSegmentBase> Segments { get; set; } = new ObservableCollection<FrameSegmentBase>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
