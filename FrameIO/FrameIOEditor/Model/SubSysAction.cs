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
        public actioniotype IOType { get; set; } = actioniotype.AIO_SEND;
        public string ChannelName { get; set; } = "";
        public string FrameName { get; set; } = "";
        public int Syid { get; set; }

        public ObservableCollection<SubsysActionMap> Maps { get; set; } = new ObservableCollection<SubsysActionMap>();
        public ObservableCollection<SubsysActionMap> LiteMaps { get; set; } = new ObservableCollection<SubsysActionMap>();
        public List<string> BeginCodes { get; set; } = new List<string>();
        public List<string> EndCodes { get; set; } = new List<string>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
