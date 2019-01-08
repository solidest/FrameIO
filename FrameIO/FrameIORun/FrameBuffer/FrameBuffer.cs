
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //二进制缓冲流
    internal class FrameBuffer : IFrameWriteBuffer, IFrameReadBuffer
    {
        private MemoryStream _cach;
        private SliceWriter _odd;
        private SliceReader _sr;
        private Dictionary<object, int> _pos;
        private Dictionary<object, int> _repeateds;

        public bool CanRead => !_sr.IsEmpty;

        public FrameBuffer()
        {
            _cach = new MemoryStream();
            _odd = SliceWriter.Empty;
            _sr = new SliceReader(null);
            _pos = new Dictionary<object, int>();
            _repeateds = new Dictionary<object, int>();
        }

        #region --Writer-- 

        //写入到缓冲
        public void Write(ulong rawValue, int bitLen, object token)
        {
            _pos.Add(token,(int)_cach.Position*8 + _odd.BitLen);
            _odd = _odd.WriteAppend(_cach, SliceWriter.GetSlice(rawValue, bitLen));
        }

        #endregion

        #region --Reader--

        //添加数据
        public void Append(byte[] cach)
        {
            _sr.FlushNew(cach);
            _cach.Write(cach, 0, cach.Length);
        }

        public ulong Read(int bitLen, object token)
        {
            _pos.Add(token, (int)_cach.Position * 8 - _sr.NotReadBitLen);
            return _sr.ReadBits(bitLen);
        }

        #endregion

        #region --Helper--


        public int GetBytePos(object token)
        {
            var bitpos =  _pos[token];
            if(bitpos%8 != 0) throw new Exception("runtime 数据帧字段未能整字节对齐");
            return bitpos / 8;
        }

        public byte[] GetBuffer()
        {
            return _cach.GetBuffer();
        }

        public void SaveRepeated(object token, int index)
        {
            if (_repeateds.ContainsKey(token))
                _repeateds[token] = index;
            else
                _repeateds.Add(token, index);
        }

        public int LoadRepeated(object token)
        {
            return _repeateds[token];
        }


        #endregion


    }
}
