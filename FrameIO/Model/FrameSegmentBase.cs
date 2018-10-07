using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public abstract class FrameSegmentBase : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public Exp Repeated { get; set; } = new Exp() { Op = exptype.EXP_INT, ConstStr="1" };
  
        public int Syid { get; set; }     


        public event PropertyChangedEventHandler PropertyChanged;

    }
}
