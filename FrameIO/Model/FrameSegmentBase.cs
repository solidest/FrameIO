using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class FrameSegmentBase : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public segmenttype SegmentType { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
