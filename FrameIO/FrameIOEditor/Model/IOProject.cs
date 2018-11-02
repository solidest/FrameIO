using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FrameIO.Main
{
    [JsonObject(MemberSerialization.OptIn)]
    public class IOProject:INotifyPropertyChanged
    {
        private int _projectid;
        public string Name { get; set; }
        public string Notes { get; set; }
        public ObservableCollection<Subsys> SubsysList { get; set; } = new ObservableCollection<Subsys>();

        [JsonProperty]
        public ObservableCollection<Frame> FrameList { get; set; } = new ObservableCollection<Frame>();
        public ObservableCollection<Enumdef> EnumdefList { get; set; } = new ObservableCollection<Enumdef>();

        public event PropertyChangedEventHandler PropertyChanged;

        public IOProject(string name)
        {
            Name = name;
        }

        public IOProject(int projectid)
        {
            _projectid = projectid;
        }


    }
}
