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
        public InnerSubsys(string name, ObservableCollection<FrameSegmentBase> seglist)
        {
            Name = name;
            AddPropertys(seglist);
        }
        public string Name { get; set; }

        public string Notes { get; set; }

        public int Syid { get; set; }

        public ObservableCollection<SubsysProperty> Propertys { get; set; } = new ObservableCollection<SubsysProperty>();


        //添加子系统字段 来自数据帧字段
        private void AddPropertys(ObservableCollection<FrameSegmentBase> seglist)
        {
            foreach (var seg in seglist)
            {
                var protype = Helper.ConvertSegType2ProType(seg);
                if (protype == "") continue;
                var len = seg.Repeated.IsConst() ? ((long)seg.Repeated.GetConstValue()).ToString() : "";
                Propertys.Add(new SubsysProperty()
                {
                    Name = seg.Name,
                    IsArray = !seg.Repeated.IsIntOne(),
                    PropertyType = protype,
                    ArrayLen = len
                });
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
