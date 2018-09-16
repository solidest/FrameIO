using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO
{
    public class Frame:INotifyPropertyChanged
    {
        public Frame(string name)
        {
            FrameName = name;
        }
        public string FrameName { get; set; }
        public string FrameNotes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
