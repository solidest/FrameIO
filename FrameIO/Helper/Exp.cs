using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //计算表达式
    public class Exp
    {
        public exptype Op { get; set; }
        public Exp LeftExp { get; set; }
        public Exp RightExp { get; set; }
        public string ConstStr { get; set; }
    }
}
