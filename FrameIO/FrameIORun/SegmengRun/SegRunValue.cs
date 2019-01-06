using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal abstract class SegRunValue : SegRunBase
    {
        //单个字段的比特长度
        internal abstract int BitLen { get;}


        //取字段的内存流
        internal abstract ulong GetBuffer(JValue value);
    }
}
