using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    [Serializable]
    public class FrameSegmentAuto : FrameSegmentBase
    {
        public FrameSegmentAuto(string name)
        {
            Name = name;
        }
    }
}
