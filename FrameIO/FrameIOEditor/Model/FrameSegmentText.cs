using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //tail、alignedlen、endfill、repeated
    public class FrameSegmentText :FrameSegmentBase
    {
        public string Tail { get; set; } = null;
        public int AlignedLen { get; set; } = 1;
        public Exp ByteSize { get; set; } = new Exp() { Op = exptype.EXP_INT, ConstStr = "0" };
        public int ByteSizeNumber { get; set; } = -1;

        public override void AppendSegmentCode(StringBuilder code)
        {
            throw new NotImplementedException();
        }
    }
}
