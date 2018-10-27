using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    using PropertyTools.DataAnnotations;

    public class FrameSegmentBlock : FrameSegmentBase
    {
        [Browsable(false)]
        public BlockSegType UsedType { get; set; } = BlockSegType.None;

        [VisibleBy(nameof(UsedType), BlockSegType.RefFrame)]
        [Category("Data")]
        public string RefFrameName { get; set; } = "";

        [Browsable(false)]
        public ObservableCollection<FrameSegmentBase> DefineSegments { get; set; } = null;

        [VisibleBy(nameof(UsedType), BlockSegType.OneOf)]
        [Category("Data")]
        public string OneOfBySegment { get; set; } = "";

        [VisibleBy(nameof(UsedType), BlockSegType.OneOf)]
        [Category("Data")]
        public ObservableCollection<OneOfMap> OneOfCaseList { get; set; } = new ObservableCollection<OneOfMap>();

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
