using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO
{
    public class IOProject:INotifyPropertyChanged
    {
        public IOProject(string name)
        {
            ProjectName = name;
        }

        public IOProject()
        {
        }

        public string ProjectName { get; set; }
        public string ProjectNotes { get; set; }
        public ObservableCollection<Subsys> SubSysList { get; } = new ObservableCollection<Subsys>();

        public ObservableCollection<Frame> FrameList { get; } = new ObservableCollection<Frame>();

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
