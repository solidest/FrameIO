using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //运行时字段信息
    internal class SegRunInfo
    {
        //在内存流中的比特位置
        internal int BitPos { get; private set; }

        //对应的数值内容
        internal JToken Content { get; private set; }

        //对应的数据帧字段
        internal SegRunBase Segment { get; private set; }

        //在数组中的序号
        internal int Index { get; private set; }

    }
}
