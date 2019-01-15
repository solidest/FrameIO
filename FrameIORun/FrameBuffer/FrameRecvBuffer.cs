using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal class FrameRecvBuffer : IFrameReadBuffer
    {
        private MemoryStream _cach;
        private SliceReader _sr;
        private object _lasttoken;
        private Dictionary<object, int> _pos;

        public bool CanRead => !_sr.IsEmpty;


        public FrameRecvBuffer()
        {
            _cach = new MemoryStream();
            _sr = new SliceReader(null);
            _pos = new Dictionary<object, int>();
        }

        public void Append(byte[] cach)
        {
            _sr.FlushNew(cach);
            _cach.Write(cach, 0, cach.Length);
        }

        public ulong ReadBits(int bitLen, object token)
        {
            if(token != _lasttoken)
            {
                _pos.Add(token, (int)_cach.Position * 8 - _sr.NotReadBitLen);
                _lasttoken = token;
            }
            return _sr.ReadBits(bitLen);
        }

       
        public byte[] ReadBytes(int byteLen, object token)
        {
            _pos.Add(token, (int)_cach.Position * 8 - _sr.NotReadBitLen);
            if (!_sr.CanReadBytes(byteLen)) throw new Exception("unknow");
            return _sr.ReadBytes(byteLen);
        }

        public byte[] GetBuffer()
        {
            return _cach.GetBuffer();
        }

        public int GetBytePos(object token)
        {
            var bitpos = _pos[token];
            if (bitpos % 8 != 0) throw new Exception("runtime 数据帧字段未能整字节对齐");
            return bitpos / 8;
        }


    }
}
