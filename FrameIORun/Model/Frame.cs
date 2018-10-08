using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    [Serializable]
    public class Frame:INotifyPropertyChanged
    {
        public Frame(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public int Syid { get; set; }
        public string Notes { get; set; }

        public ObservableCollection<FrameSegmentBase> Segments { get; set; }
        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
