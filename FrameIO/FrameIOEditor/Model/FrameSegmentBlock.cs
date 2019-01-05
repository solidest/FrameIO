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
        public ObservableCollection<FrameSegmentBase> DefineSegments { get; set; } = new ObservableCollection<FrameSegmentBase>();

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
        public string SubSysName { get; set; }

        public override void AppendSegmentCode(StringBuilder code)
        {
            if (UsedType == BlockSegType.DefFrame && SubSysName != null && SubSysName.Length > 0)
            {
                code.AppendFormat("[subsys: {0}]\n\t\t", SubSysName);
            }
            var repstr = Repeated.IsIntOne() ? "" : string.Format(" repeated={0}", Repeated.ToString());

            code.AppendFormat("block {0}{1} type=", Name, repstr);
            switch (UsedType)
            {
                case BlockSegType.RefFrame:
                    code.Append(RefFrameName);
                    break;
                case BlockSegType.DefFrame:
                    code.Append("{\n");
                    foreach (var seg in DefineSegments)
                    {
                         code.Append("\t\t\t");
                         seg.AppendSegmentCode(code);
                        code.Append("\n");
                    }
                    code.Append("\t\t}");
                    break;
                case BlockSegType.OneOf:
                    code.AppendFormat("oneof({0}){{\n", OneOfBySegment.Length==0?"_":OneOfBySegment);
                    int i = -1;
                    foreach(var oi in OneOfCaseList)
                    {
                        i++;
                        code.AppendFormat("\t\t\t {0} : {1}{2}\n", oi.EnumItem.Length==0?"_": oi.EnumItem, oi.FrameName.Length==0?"_": oi.FrameName, i==OneOfCaseList.Count-1?"":",");
                    }
                    code.Append("\t\t}" );
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
