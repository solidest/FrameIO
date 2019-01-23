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
        private MatchHeader _match;

        public FrameObject RootValue { get; }


        internal FrameUnpacker(string frameName)
        {
            RootValue = new FrameObject(frameName);
            _f = IORunner.GetFrame(frameName);
            _qq = new FrameSegValueQueue(_f, RootValue.RootValue);
            _b = new FrameRecvBuffer();
            _appendCount = 0;
            _match = new MatchHeader(_f.MatchValue, _f.MatchHeaderBytesLen);
        }


        public int FirstBlockSize => _qq.FirstBytesLen;

        public int AppendBlock(byte[] buffer)
        {
            //需要匹配包头 逐字节匹配
            if (_appendCount == 0 && _match.NeedMatch && !_match.AppendAndMatch(buffer))
            {
                return 1;
            }
            _appendCount += 1;
            if (_appendCount == 1 && _match.NeedMatch)
                _b.Append(_match.Header);
            else
                _b.Append(buffer);

            _qq.Unpack(_b);
            var ret =  _qq.GetNextBlockSize();
            return ret;
        }

        public ISegmentGettor Unpack()
        {
            return RootValue;
        }

    }
}
