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
        private Dictionary<object, int> _repeateds;
        private Dictionary<object, int> _pos;

        public bool CanRead => !_sr.IsEmpty;

        public object StopPosition { get; set; }


        public FrameRecvBuffer()
        {
            _cach = new MemoryStream();
            _sr = new SliceReader(null);
            _repeateds = new Dictionary<object, int>();
            _pos = new Dictionary<object, int>();
        }

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
    }
}
