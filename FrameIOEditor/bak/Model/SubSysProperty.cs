using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class SubsysProperty : INotifyPropertyChanged
    {
        private string _len;

        public SubsysProperty(string name, SubsysProperty copyfrom)
        {
            if(!copyfrom.IsBaseType()) throw new Exception(copyfrom.Name +": 子系统属性只能是基础类型");
             Name = name;
           _len = copyfrom._len;
            Notes = copyfrom.Notes;
            PropertyType = copyfrom.PropertyType;
            IsArray = copyfrom.IsArray;
            ArrayLen = copyfrom.ArrayLen;
            Syid = copyfrom.Syid;
        }

        public SubsysProperty()
        {
        }

        public string Name { get; set; }
        public string Notes { get; set; }

        public string PropertyType { get; set; }
        public bool IsArray { get; set; }

        public string ArrayLen
        {
            get
            {
                if (IsArray)
                    return _len;
                else
                    return "";
            }
            set
            {
                if (IsArray && value!="0") _len = value;
            }
        }
        public int Syid { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Name;
        }
        public bool IsBaseType()
        {
            return (PropertyType == "bool" || PropertyType == "byte" || PropertyType == "sbyte"
                || PropertyType == "short" || PropertyType == "ushort" || PropertyType == "int" || PropertyType == "uint"
                || PropertyType == "long" || PropertyType == "ulong" || PropertyType == "float" || PropertyType == "double");
        }

        public bool IsEnum(IOProject pj)
        {
            return pj.IsEnum(PropertyType);
        }

        public bool IsInnerSubsys(IOProject pj)
        {
            return pj.IsInnerSubsys(PropertyType);
        }
    }
}
