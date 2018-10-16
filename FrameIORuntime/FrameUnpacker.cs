using FrameIO.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    public class FrameUnpacker : IFrameUnpack, IUnpackRunExp
    {
        FrameInfo _fi;
        private MemoryStream _buff;

        SegmentUnpackInfo[] _segis;
        private ushort _seg_pos;            //当前字段位置
        private int _nextsize;              //下一块内存大小

        public FrameUnpacker(FrameInfo fi)
        {
            _fi = fi;
            _buff = new MemoryStream();
            FirstBlockSize = GetNextBitLen(1);     
            Reset();
        }


        #region --Unpack--

        public int FirstBlockSize { get; private set;  }

        public int AppendBlock(byte[] block)
        {
            if (_nextsize == 0 || block.Length != _nextsize) throw new Exception("runtime");
            var bit_pos = (int)_buff.Position * 8;
            _buff.Write(block, 0, _nextsize);
            int end_bit_pos = (int)_buff.Position * 8;

            var buff = _buff.GetBuffer();
            var newpos = _seg_pos;
            while(bit_pos != end_bit_pos)
            {
                var res = _fi[newpos].Unpack(buff, ref bit_pos, _segis[newpos], this);
                if (res == 0)
                    newpos += 1;
                else
                    newpos = res;
            }

            _seg_pos = newpos;

            if(_seg_pos == _fi.SegmentsCount)
                _nextsize = 0;
            else
            {
                var needbitlen = GetNextBitLen(newpos);
                if (needbitlen % 8 != 0) throw new Exception("runtime");
                _nextsize = needbitlen / 8;
            }

            return _nextsize;
        }

        public ISegmentGettor Unpack()
        {
            _buff.Close();
            var ret = new SegmentGettor(_fi, _buff.ToArray(), _segis);
            Reset();
            return ret;
        }


        private void Reset()
        {
            _seg_pos = 1;                       //当前字段位置
            _nextsize = FirstBlockSize;         //下一块需要的内存大小

            _segis = new SegmentUnpackInfo[_fi.SegmentsCount];
            for (int i = 0; i < _segis.Length; i++)
            {
                _segis[i] = new SegmentUnpackInfo();
            }
            _buff.Seek(0, SeekOrigin.Begin);

        }

        private int GetNextBitLen(ushort startidx)
        {
            int bitlen = 0;
            while(startidx != _fi.SegmentsCount)
            {
                ushort next_seg = 0;
                if (_fi[startidx].TryGetBitLen(ref bitlen, ref next_seg, _segis[startidx], this))
                {
                    if (next_seg == 0)
                        startidx += 1;
                    else
                        startidx = next_seg;
                }
                else
                    break;
            }
            return bitlen;
        }

        #endregion


        #region --IRunExp--


        public bool TryGetSegmentValue(ref double value, ushort idx)
        {
            return _fi[idx].TryGetValue(ref value, _buff.GetBuffer(), _segis[idx]);
        }

        public bool TryGetSegmentByteSize(ref double size, ushort idx)
        {
            int len = 0;
            ushort nextseg = 0;
            if(_fi[idx].TryGetBitLen(ref len, ref nextseg, _segis[idx], this))
            {
                if (len % 8 != 0)
                    throw new Exception("runtime");
                else
                    size = len / 8;
                return true;
            }

            return false;

        }

        public bool TryGetBitLen(ref int bitlen, ref ushort nextseg, ushort idx)
        {
            return _fi[idx].TryGetBitLen(ref bitlen, ref nextseg, _segis[idx], this);
        }

        public bool TryGetExpValue(ref double value, ushort idx)
        {
            return _fi.GetExp(idx).TryGetExpValue(ref value, this);
        }

        public double GetConst(ushort idx)
        {
            return _fi.GetConst(idx);
        }

        public bool IsConst(ushort idx)
        {
            return _fi.IsConst(idx);
        }

        public bool IsConstOne(ushort idx)
        {
            return _fi.IsConstOne(idx);
        }

        public SegmentValidator GetValidator(ushort idx, ValidateType type)
        {
            return _fi.GetValidator(idx, type);
        }


        #endregion


    }

    public struct SegmentUnpackInfo
    {
        public bool IsUnpack;
        public int BitStart;
        public int BitLen;
    }
}
