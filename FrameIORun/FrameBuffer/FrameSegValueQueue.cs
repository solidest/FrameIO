using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal class FrameSegValueQueue
    {
        private Queue<ValueInfo> _qq;
        private ValueInfo _last;
        public int FirstBytesLen { get; private set; }

        public FrameSegValueQueue(SegRunFrame frm, JObject root)
        {
            _qq = new Queue<ValueInfo>();
            var vi = new ValueInfo();

            var res = frm.LookUpFirstValueSeg(out vi.vSeg, out vi.vPc, out vi.vLen, root, null);

            Debug.Assert(res);

            if (frm.MatchHeaderBytesLen > 0)  //需要匹配包头
            {
                Debug.Assert(frm.MatchHeaderBytesLen == vi.vSeg.BitLen / 8 && vi.vSeg.BitLen % 8 == 0);
                FirstBytesLen = GetNextBlockSize(vi, false);
            }
            else
            {
                FirstBytesLen = GetNextBlockSize(vi, false);
            }

        }

        public int GetNextBlockSize()
        {
            return GetNextBlockSize(null, true);
        }

        //准备解包字段 返回所需下一块内存的大小
        private int GetNextBlockSize(ValueInfo header, bool needStep)
        {
            int bitLen = 0;

            
            if(header == null)
                Debug.Assert(_qq.Count == 0);
            else  //计算包头大小
            {
                _qq.Enqueue(header);
                _last = header;
                bitLen += header.vLen * header.vSeg.BitLen;
                if (!needStep) return bitLen / 8;
            }

            var nextSeg = _last.vSeg;
            JContainer pc = _last.vPc;
            int len = _last.vLen;

            while (nextSeg.LookUpNextValueSeg(out nextSeg, out pc, out len, pc, nextSeg))
            {
                if (nextSeg == null) break;
                bitLen += nextSeg.BitLen * len;
                var pos = new ValueInfo() { vSeg = nextSeg, vLen = len, vPc = pc };
                _qq.Enqueue(pos);
                _last = pos;
            }

            if (bitLen % 8 != 0) throw new Exception("runtime 数据帧字段未能整字节对齐");

            return bitLen/8;            
        }

        //解包
        public void Unpack(IFrameReadBuffer buff)
        {
            while(_qq.Count>0)
            {
                var vi = _qq.Dequeue();
                for (int i = 0; i < vi.vLen; i++)
                {
                    vi.vSeg.UnpackValue(buff, vi.vPc);
                }
            }
        }

        private class ValueInfo
        {
            public SegRunValue vSeg;
            public JContainer vPc;
            public int vLen;
        }

    }
}
