using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal class SliceReader
    {
        private byte[] _cach;
        private int _bytePos;
        private int _bitOdd;

        public SliceReader(byte[] cach)
        {
            _cach = cach;
            _bytePos = 0;
            _bitOdd = 0;
        }

        public void FlushNew(byte[] cach)
        {
            if (!IsEmpty)
                throw new Exception("unknow");
            _cach = cach;
            _bytePos = 0;
            _bitOdd = 0;
        }


        public ulong ReadBits(int bitLen)
        {
            //字节推进
            var newOdd = (_bitOdd + bitLen) % 8;
            var newPos = _bytePos + bitLen / 8;
            if (bitLen % 8 + _bitOdd >=8) newPos += 1;

            ulong ret = 0;

            if (_cach.Length < _bytePos + 8)
            {
                byte[] newb = new byte[8];
                for (int i = _bytePos, ii = 0; i <= (newPos==_cach.Length ? newPos-1:newPos); i++, ii++)
                    newb[ii] = _cach[i];
                
                ret = (BitConverter.ToUInt64(newb, 0) >> _bitOdd) & (Helper.FULL >> (64 - bitLen));
            }
            else
            {
                ret = (BitConverter.ToUInt64(_cach, _bytePos) >> _bitOdd) & (Helper.FULL >> (64 - bitLen));
            }

            _bytePos = newPos;
            _bitOdd = newOdd;
            return ret;
        }

        public byte[] ReadBytes(int byteLen)
        {
            if (_bytePos == 0 && _bitOdd == 0 && _cach.Length == byteLen)
                return _cach;
            //HACK read bytes
            return null;
        }


        public bool IsEmpty { get => ( _bitOdd == 0 && _bytePos == (_cach?.Length??0)); }

        public int NotReadBitLen { get => (_cach.Length - _bytePos) * 8 - _bitOdd;  }

        public bool CanReadBytes(int byteLen)
        {
            return NotReadBitLen >= byteLen * 8;
        }
    }
}
