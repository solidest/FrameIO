using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

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

        public List<string> GetPropertyTypeList(string owner)
        {
            var ret = new List<string>();
            ret.Add("bool");
            ret.Add("byte");
            ret.Add("sbyte");
            ret.Add("short");
            ret.Add("ushort");
            ret.Add("int");
            ret.Add("uint");
            ret.Add("long");
            ret.Add("ulong");
            ret.Add("float");
            ret.Add("double");

            foreach (var subs in SubsysList)
            {
                if (subs.Propertys.Count > 0 && subs.Actions.Count == 0 && subs.Channels.Count == 0 && subs.Name != owner)
                    ret.Add(subs.Name);
            }

            foreach (var ename in EnumdefList)
            {
                ret.Add(ename.Name);
            }
            return ret;
        }

        public bool IsEnum(string name)
        {
            return EnumdefList.Where(p => p.Name == name).Count() > 0;
        }

        public List<string> GetPropertyList(ObservableCollection<SubsysProperty> props)
        {
            var ret = new List<string>();
            foreach(var p in props)
            {
                if (p.IsBaseType() || IsEnum(p.Name))
                    ret.Add(p.Name);
                else
                    ret.AddRange(GetSubPropertyList(p.PropertyType,p.Name));
            }
            return ret;
        }

        private List<string> GetSubPropertyList(string sysname, string parentname)
        {
            var ret = new List<string>();
            var fsubsys = SubsysList.Where(p => p.Name == sysname);
            if (fsubsys.Count() == 0) return ret;
            var subsys = fsubsys.First();
            foreach(var p in subsys.Propertys)
            {
                if (p.IsBaseType() || IsEnum(p.Name))
                    ret.Add(parentname + "." + p.Name);
            }
            return ret;
        }

    }
}
