using System.Collections.ObjectModel;
using System.Text;

namespace FrameIO.Main
{
    using Newtonsoft.Json;
    using PropertyTools.DataAnnotations;
    using System.Runtime.Serialization;

    [JsonObject(MemberSerialization.OptIn)]
    public class FrameSegmentBlock : FrameSegmentBase
    {
        [Browsable(false)]
        [JsonProperty]
        public BlockSegType UsedType { get; set; } = BlockSegType.None;

        [VisibleBy(nameof(UsedType), BlockSegType.RefFrame)]
        [Category("Data")]
        [JsonProperty]
        public string RefFrameName { get; set; } = "";

        [Browsable(false)]
        [JsonProperty]
        public ObservableCollection<FrameSegmentBase> DefineSegments { get; set; } = null;

        [VisibleBy(nameof(UsedType), BlockSegType.OneOf)]
        [Category("Data")]
        [JsonProperty]
        public string OneOfBySegment { get; set; } = "";

        [VisibleBy(nameof(UsedType), BlockSegType.OneOf)]
        [Category("Data")]
        [JsonProperty]
        public ObservableCollection<OneOfMap> OneOfCaseList { get; set; } = new ObservableCollection<OneOfMap>();

        [VisibleBy(nameof(UsedType), BlockSegType.DefFrame)]
        [Category("Other")]
        [JsonProperty]
        public string SubSys { get; set; }

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
                    code.AppendFormat("oneof({0}){{", OneOfBySegment.Length==0?"_":OneOfBySegment);
                    int i = -1;
                    foreach(var oi in OneOfCaseList)
                    {
                        i++;
                        code.AppendFormat(" {0} : {1}{2}", oi.EnumItem.Length==0?"_": oi.EnumItem, oi.FrameName.Length==0?"_": oi.FrameName, i==OneOfCaseList.Count-1?"":",");
                    }
                    code.Append("}" );
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
