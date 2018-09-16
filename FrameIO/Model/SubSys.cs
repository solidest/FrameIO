using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO
{
    public class Subsys
    {
        public Subsys(string name)
        {
            SubsysName = name;
        }
        public string SubsysName { get; set; }
        public string SubsysNotes { get; set; }
    }
}
