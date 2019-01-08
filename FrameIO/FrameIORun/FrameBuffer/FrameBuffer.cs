
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //二进制缓冲流
    internal class FrameBuffer : IFrameWriteBuffer
    {
        private MemoryStream _cach;
        private SliceWriter _odd;
        private SliceReader _sr;
        private Dictionary<object, int> _pos;

        public FrameBuffer()
        {
            _cach = new MemoryStream();
            _odd = SliceWriter.Empty;
            _sr = new SliceReader(null);
            _pos = new Dictionary<object, int>();
        }

        #region --For Pack-- 

        //写入到缓冲
        public void Write(ulong rawValue, int bitLen, object token)
        {
            _odd = _odd.WriteAppend(_cach, SliceWriter.GetSlice(rawValue, bitLen));
            _pos.Add(token, GetBytePos());
        }

        #endregion

        #region --For Unpack--

        //添加数据
        public void Append(byte[] cach)
        {
            _sr.FlushNew(cach);
            _cach.Write(cach, 0, cach.Length);
        }

        #endregion

        #region --Helper--

        //当前字节位置
        public int GetBytePos()
        {
            if(!_odd.IsEmpty) throw new Exception("runtime 数据帧字段未能整字节对齐");
            return (int)_cach.Position;
        }

        public int GetPos(object token)
        {
            return _pos[token];
        }

        public byte[] GetBuffer()
        {
            return _cach.GetBuffer();
        }

        public ulong Read(int bitLen, object token)
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
