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
            if (!IsEmpty) throw new Exception("unknow");
            _cach = cach;
            _bytePos = 0;
            _bitOdd = 0;
        }


        public ulong ReadBits(int bitLen)
        {
            var len = _bitOdd + bitLen;
            var newOdd = len % 8;
            var newPos = _bytePos + len / 8 + (newOdd == 0 ? 0 : 1);

            ulong ret = 0;

            if (_bytePos + 7 > _cach.Length)
            {
                byte[] newb = new byte[8];
                for (int i = _bytePos, ii = 0; i < newPos; i++, ii++)
                    newb[ii] = _cach[i];
                ret = (BitConverter.ToUInt64(newb, 0) >> _bitOdd) & (0xFFFFFFFFFFFFFFFF >> (64 - bitLen));
            }
            else
            {
                ret = (BitConverter.ToUInt64(_cach, _bytePos) >> _bitOdd) & (0xFFFFFFFFFFFFFFFF >> (64 - bitLen));
            }

            _bytePos = newPos;
            _bitOdd = newOdd;
            return ret;
        }


        public bool IsEmpty { get => ( _bitOdd == 0 && _bytePos == (_cach?.Length??0)); }
    }
}
