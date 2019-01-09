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
    internal class FrameUnpacker : FrameIO.Interface.IFrameUnpack
    {
        private FrameRecvBuffer _b;
        private int  _appendCount;
        private SegRunFrame _f;
        private ISegRun _segPos;
        public FrameObject RootValue { get; private set; }

        internal FrameUnpacker(string frameName)
        {
            _f = IORunner.GetFrame(frameName);
            _segPos = _f;
            _b = new FrameRecvBuffer();
            _appendCount = 0;
            RootValue = new FrameObject(frameName);
        }

        public int FirstBlockSize => _f.GetFirstNeedBytes();

        public int AppendBlock(byte[] buffer)
        {
            if (_appendCount == 0 && !_f.IsMatch(buffer)) return FirstBlockSize;
            _appendCount += 1;
            _b.Append(buffer);
            _segPos = _f.UnpackFrom(_segPos, _b, RootValue.RootValue);

            if (_segPos == null) return 0;

            int needBitlen = 0;
            ISegRun next = _segPos;
            while(next != null)
            {
                _segPos.GetNeedBitLen(ref needBitlen, out next, null);
            }
            Debug.Assert(needBitlen != 0);
            if(needBitlen%8!=0) throw new Exception("runtime 数据帧字段未能整字节对齐");
            return needBitlen / 8;
        }

        public ISegmentGettor Unpack()
        {
            return RootValue;
        }

    }
}
