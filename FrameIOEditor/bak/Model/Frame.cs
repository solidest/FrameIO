using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FrameIO.Main
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Frame:INotifyPropertyChanged
    {
        public Frame(string name)
        {
            Name = name;
        }
        [JsonProperty]
        public string Name { get; set; }
        public int Syid { get; set; }
        public string Notes { get; set; }
        public string SubSysName { get; set; }

        public override string ToString()
        {
            return Name;
        }

        [JsonProperty]
        public ObservableCollection<FrameSegmentBase> Segments { get; set; } = new ObservableCollection<FrameSegmentBase>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
