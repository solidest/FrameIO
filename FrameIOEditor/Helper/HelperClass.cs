using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class SelectItem
    {
        public SelectItem(string name, string value)
        {
            SelectName = name;
            SelectValue = value;
        }
        public string SelectName { get; set; }
        public string SelectValue { get; set; }
    }
}
