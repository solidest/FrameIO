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
    }
}
