using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //数据帧
    internal class SegRunRootFrame : SegRunContainer
    {
        internal override SegRunContainer Parent => throw new NotImplementedException();

        internal override SegRunBase Next => throw new NotImplementedException();

        internal override SegRunBase Previous => throw new NotImplementedException();

        internal override SegRunBase First => throw new NotImplementedException();

        internal override SegRunBase Last => throw new NotImplementedException();

        internal override SegRunContainer Root => throw new NotImplementedException();

        internal override string Name => throw new NotImplementedException();
    }
}
