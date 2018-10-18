using FrameIO.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    //数据帧打包信息
    internal class FramePackerInfo
    {
        private SetValueInfo[] _segvi;
        private static FrameRuntime _fi;

        static FramePackerInfo()
        {
            _fi = FrameRuntime.Info;
        }

        internal FramePackerInfo(ushort startidx, ushort endidx)
        {
            _segvi = new SetValueInfo[endidx - startidx+1];
            StartIdx = startidx;
            EndIdx = endidx;
            Cach = new MemoryStream();
        }

        internal SetValueInfo this[ushort idx]
        {
            get
            {
                return _segvi[idx - StartIdx];
            }
        }

        internal MemoryStream Cach { get; }
        internal ushort StartIdx { get; private set; }
        internal ushort EndIdx { get; private set; }

        internal void Reset()
        {
            Cach.Seek(0, SeekOrigin.Begin);
            _segvi = new SetValueInfo[EndIdx - StartIdx + 1];
        }
    }

    //字段值设置信息
    public struct SetValueInfo
    {
        public bool IsSetValue;
        public int StartPos;
        public int BitLen;
        public int Count;
        public object Tag;
    }


}
