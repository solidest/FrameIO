using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class IOProject:INotifyPropertyChanged
    {

        public string ProjectName { get; set; }
        public string ProjectNotes { get; set; }
        public ObservableCollection<Subsys> SubSysList { get; set; } = new ObservableCollection<Subsys>();

        public ObservableCollection<Frame> FrameList { get; set; } = new ObservableCollection<Frame>();
        public ObservableCollection<Enumdef> EnumdefList { get; set;  }

        public event PropertyChangedEventHandler PropertyChanged;

        public IOProject(string name)
        {
            ProjectName = name;
        }

        public IOProject()
        {
        }

        //创建代码
        public string CreateCode()
        {
            return "";
        }

        //语义检查
        public IList<ParseError> CheckSemantics()
        {
            return null;
        }

    }
}
