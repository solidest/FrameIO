using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal class Slice
    {
        private ulong _v;
        private int _bytesLen;
        private int _bitOddLen;

        #region --Property--

        public bool IsEmpty
        {
            get
            {
                return (_bytesLen == 0 && _bitOddLen == 0);
            }
        }

        public int BitLen
        {
            get
            {
                return _bytesLen * 8 + _bitOddLen;
            }
        }

        public ulong TheValue
        {
            get
            {
                return (_v & (0xFFFFFFFFFFFFFFFF>>(64 - BitLen)));
            }
        }


        #endregion

        #region --Operation--

        //追加写入
        public Slice WriteAppend(MemoryStream buff, Slice s)
        {
            if (IsEmpty) return WriteSlice(buff, s);

            var newlen = BitLen + s.BitLen;
            if(newlen>64)
            {
                buff.Write(BitConverter.GetBytes(TheValue | s._v<<BitLen), 0, 8);
                return WriteSlice(buff, GetSlice(s.TheValue >> (64 * 2 - newlen), newlen - 64));
            }
            else
            {
                return WriteSlice(buff, GetSlice(TheValue | (s.TheValue << BitLen), newlen));
            }
        }

        //将Slice写入内存流
        public static Slice WriteSlice(MemoryStream buff, Slice s)
        {
            if (s._bytesLen == 0) return s;
            buff.Write(BitConverter.GetBytes(s._v), 0, s._bytesLen);
            return s.TheOdd();
        }

        #endregion

        #region --Initial--

        private Slice()
        {
            
        }

        private Slice(ulong v, int bitlen)
        {
            _v = v;
            _bytesLen = bitlen / 8;
            _bitOddLen = bitlen % 8;
        }

        #endregion

        #region --Helper--

        public Slice TheOdd()
        {
            if (_bitOddLen == 0) return Empty;
            return GetSlice((TheValue >> (_bytesLen * 8)), _bitOddLen);
        }

        private void SetEmpty()
        {
            _v = 0;
            _bitOddLen = 0;
            _bytesLen = 0;
        }

        static Slice()
        {
            Empty = new Slice();
        }

        public static Slice Empty { get;  private set; }

        public static Slice GetSlice(ulong v, int bitlen)
        {
            if (bitlen == 0) return Empty;
            return new Slice(v, bitlen);
        }

        #endregion

    }
}
