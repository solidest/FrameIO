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
        private FrameUnpackerInfo _parent;
        private  UnpackInfo[] _segus;

        static FrameUnpackerInfo()
        {
            _fi = FrameRuntime.Run;
        }
        internal FrameUnpackerInfo(ushort startidx, ushort endidx, FrameUnpackerInfo parent)
        {
            StartIdx = startidx;
            EndIdx = endidx;
            _segus = new UnpackInfo[endidx - startidx + 1];
            for (ushort i = startidx; i <= endidx; i++)
                _segus[i-startidx] = new UnpackInfo();
            _parent = parent;
        }

        internal UnpackInfo this[ushort idx]
        {
            get
            {
                return _segus[idx - StartIdx];
            }
        }

        internal void AddErrorInfo(string info, ushort idx)
        {
            throw new FrameIO.Interface.FrameIOException(Interface.FrameIOErrorType.RecvErr, FrameRuntime.GetName(idx), info);
        }

        //internal void AddErrorInfo(ErrorInfo err)
        //{
        //    err.idx.Add(StartIdx);
        //    if (_parent != null)
        //        _parent.AddErrorInfo(err);
        //    else
        //        throw new Exception(err.Info);
        //}
    }

    internal class UnpackInfo
    {
        public bool IsUnpack;
        public int BitStart;
        public int BitLen;
        public object Tag;
    }
 
}
