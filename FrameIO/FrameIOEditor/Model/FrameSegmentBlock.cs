using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{

    public class FrameSegmentBlock : FrameSegmentBase
    {
        public BlockSegType UsedType { get; set; } = BlockSegType.None;
        public string RefFrameName { get; set; } = null;
        public ObservableCollection<FrameSegmentBase> DefineSegments { get; set; } = null;
        public string OneOfFromSegment { get; set; } = null;
        public ObservableCollection<OneOfMap> OneOfCaseList { get; set; } = null;

    }

   
    public class OneOfMap
    {
        public string EnumItem { get; set; } = "";
        public string FrameName { get; set; } = "";
        public bool IsDefault { get; set; } = false;
    }

    public enum BlockSegType
    {
        RefFrame,
        DefFrame,
        OneOf,
        None
    }
}
