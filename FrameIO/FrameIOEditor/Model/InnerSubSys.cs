using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class InnerSubsys : INotifyPropertyChanged
    {
        public InnerSubsys(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Notes { get; set; }
        public ObservableCollection<SubsysProperty> Propertys { get; set; } = new ObservableCollection<SubsysProperty>();


        public void AddProperty(string name, string protype, bool isArray)
        {
            if (protype == "") return;
            Propertys.Add(new SubsysProperty()
            {
                Name = name,
                IsArray = isArray,
                PropertyType = protype
            });
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
