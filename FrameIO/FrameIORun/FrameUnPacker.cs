using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;
using Newtonsoft.Json.Linq;

namespace FrameIO.Run
{
    internal class FrameUnpacker : IFrameUnpack
    {
        private FrameRecvBuffer _b;
        private int  _appendCount;
        private SegRunFrame _f;
        private FrameSegValueQueue _qq;
        public FrameObject RootValue { get; }


        internal FrameUnpacker(string frameName)
        {
            RootValue = new FrameObject(frameName);
            _f = IORunner.GetFrame(frameName);
            _qq = new FrameSegValueQueue(_f, RootValue.RootValue);
            _b = new FrameRecvBuffer();
            _appendCount = 0;
        }


        public int FirstBlockSize => _qq.FirstBytesLen;

        public int AppendBlock(byte[] buffer)
        {
            if (_appendCount == 0 && !_f.IsMatch(buffer)) return FirstBlockSize;
            _appendCount += 1;
            _b.Append(buffer);
            _qq.Unpack(_b);
            return _qq.GetNextBlockSize();
        }

        public ISegmentGettor Unpack()
        {
            return RootValue;
        }

    }
}
