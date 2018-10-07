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
        public Exp ByteSize { get; set; } = new Exp() { Op = exptype.EXP_INT, ConstStr = "0" };
        public int ByteSizeNumber { get; set; } = -1;
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
    }

    public enum BlockSegType
    {
        RefFrame,
        DefFrame,
        OneOf,
        None
    }
}
