using Newtonsoft.Json;
using System.Text;

namespace FrameIO.Main
{
    //tail、alignedlen、endfill、repeated
    [JsonObject(MemberSerialization.OptIn)]
    public class FrameSegmentText : FrameSegmentBase
    {
        [JsonProperty]
        public string Tail { get; set; } = null;
        [JsonProperty]
        public int AlignedLen { get; set; } = 1;
        [JsonProperty]
        public Exp ByteSize { get; set; } = new Exp() { Op = exptype.EXP_INT, ConstStr = "0" };
        [JsonProperty]
        public int ByteSizeNumber { get; set; } = -1;

        public override void AppendSegmentCode(StringBuilder code)
        {
            return;
        }
    }
}
