using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //运行时字段的基类
    internal abstract class SegRunBase
    {

        //名称
        internal abstract string Name { get; }
        
        //父级容器
        internal abstract SegRunContainer Parent { get; }

        //下一个兄弟
        internal abstract SegRunBase Next { get; }

        //上一个兄弟
        internal abstract SegRunBase Previous { get;  }

        //第一个子字段
        internal abstract SegRunBase First { get; }

        //最后一个子字段
        internal abstract SegRunBase Last { get; }

        //根字段 数据帧
        internal abstract SegRunContainer Root { get; }

    }
}
