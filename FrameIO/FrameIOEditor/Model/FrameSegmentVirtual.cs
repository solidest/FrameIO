using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class FrameSegmentVirtual : FrameSegmentBase
    {
        public FrameSegmentVirtual(string name)
        {
            Name = name;
        }

        public ulong IDValue { get; set; }
    }
}
