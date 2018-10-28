﻿using System;
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

        public override void AppendSegmentCode(StringBuilder code)
        {
            code.AppendFormat("block {0} type=", Name);
            switch (UsedType)
            {
                case BlockSegType.RefFrame:
                    code.Append(RefFrameName);
                    break;
                case BlockSegType.DefFrame:
                    code.Append("{{");
                    foreach (var seg in DefineSegments)
                        seg.AppendSegmentCode(code);
                    code.Append("}}");
                    break;
                case BlockSegType.OneOf:
                    code.AppendFormat("oneof({0}){{", OneOfBySegment);
                    int len = code.Length;
                    foreach(var oi in OneOfCaseList)
                    {
                        len = code.Length;
                        code.AppendFormat(" {0} : {1},", oi.EnumItem, oi.FrameName);
                    }
                    code.Replace(",", "", len, 1);
                    code.Append("}};");
                    break;
            }
            code.Append(";");
        }
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
