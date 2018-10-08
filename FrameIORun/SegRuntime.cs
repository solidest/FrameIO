using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Main;

namespace FrameIO.Run
{
    //运行时字段信息
    public class SegRuntime
    {
        public ulong IntValue { get; set; }
        public string TextValue { get; set; }
        public double RealValue { get; set; }
        public syspropertytype ValueType { get; set; }
        public SegRuntime NextRunSeg { get; set; }

    }


}
