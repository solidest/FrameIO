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
        public ObservableCollection<InnerSubsys> InnerSubsysList { get; set; } = new ObservableCollection<InnerSubsys>();

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

        public List<string> GetPropertyTypeList()
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

            foreach (var subs in InnerSubsysList)
            {
                ret.Add(subs.Name);
            }

            foreach (var ename in EnumdefList)
            {
                ret.Add(ename.Name);
            }
            return ret;
        }

        public List<SelectItem> GetPropertySelectTypeList()
        {
            var ret = new List<SelectItem>();
            ret.Add(new SelectItem("bool", "bool"));
            ret.Add(new SelectItem("byte", "byte"));
            ret.Add(new SelectItem("sbyte", "sbyte"));
            ret.Add(new SelectItem("short", "short"));
            ret.Add(new SelectItem("ushort", "ushort"));
            ret.Add(new SelectItem("int", "int"));
            ret.Add(new SelectItem("uint", "uint"));
            ret.Add(new SelectItem("long", "long"));
            ret.Add(new SelectItem("ulong", "ulong"));
            ret.Add(new SelectItem("float", "float"));
            ret.Add(new SelectItem("double", "double"));

            foreach (var subs in InnerSubsysList)
            {
                ret.Add(new SelectItem("[subsys] " + subs.Name , subs.Name));
            }

            foreach (var ename in EnumdefList)
            {
                ret.Add(new SelectItem( "[enum] " + ename.Name, ename.Name));
            }
            return ret;
        }

        public bool IsEnum(string name)
        {
            return EnumdefList.Where(p => p.Name == name).Count() > 0;
        }

        //public List<string> GetPropertyList(ObservableCollection<SubsysProperty> props)
        //{
        //    var ret = new List<string>();
        //    foreach(var p in props)
        //    {
        //        if (p.IsBaseType() || IsEnum(p.PropertyType))
        //            ret.Add(p.Name);
        //        else
        //            ret.AddRange(GetSubPropertyList(p.PropertyType,p.Name));
        //    }
        //    return ret;
        //}

        //private List<string> GetSubPropertyList(string sysname, string parentname)
        //{
        //    var ret = new List<string>();
        //    var fsubsys = SubsysList.Where(p => p.Name == sysname);
        //    if (fsubsys.Count() == 0) return ret;
        //    var subsys = fsubsys.First();
        //    foreach(var p in subsys.Propertys)
        //    {
        //        if (p.IsBaseType() || IsEnum(p.Name))
        //            ret.Add(parentname + "." + p.Name);
        //    }
        //    return ret;
        //}

        public void UpdateChildSys()
        {
            InnerSubsysList.Clear();
            foreach(var fr in FrameList)
            {
                if (fr.SubSysName != null && fr.SubSysName.Length > 0)
                {
                    InnerSubsysList.Add(new InnerSubsys(fr.SubSysName, fr.Segments));
                }

                foreach(var seg in fr.Segments)
                {
                    if(seg.GetType() == typeof(FrameSegmentBlock))
                    {
                        var bseg = (FrameSegmentBlock)seg;
                        if(bseg.UsedType== BlockSegType.DefFrame && bseg.SubSysName!=null && bseg.SubSysName.Length>0)
                        {
                            InnerSubsysList.Add(new InnerSubsys(bseg.SubSysName, bseg.DefineSegments));
                        }
                    }
                }

            }
        }

    }
}
