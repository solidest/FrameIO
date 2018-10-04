using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class Enumdef : INotifyPropertyChanged
    {
        public Enumdef(string name)
        {
            EnumName = name;
        }
        public string EnumName { get; set; }
        public string EnumNote { get; set; }

        public ObservableCollection<EnumdefItem> ItemsList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
