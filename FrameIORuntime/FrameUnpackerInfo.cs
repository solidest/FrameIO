using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    //数据帧解包信息
    internal class FrameUnpackerInfo
    {
        internal ushort StartIdx { get; private set; }
        internal ushort EndIdx { get; private set; }

        private static FrameRuntime _fi;
        UnpackInfo[] _segus;

        static FrameUnpackerInfo()
        {
            _fi = FrameRuntime.Info;
        }
        internal FrameUnpackerInfo(ushort startidx, ushort endidx)
        {
            StartIdx = startidx;
            EndIdx = endidx;
            _segus = new UnpackInfo[endidx - startidx + 1];
        }

        internal UnpackInfo this[ushort idx]
        {
            get
            {
                return _segus[idx - StartIdx];
            }
        }
    }


    public struct UnpackInfo
    {
        public bool IsUnpack;
        public int BitStart;
        public int BitLen;
        public object Tag;
    }
}
