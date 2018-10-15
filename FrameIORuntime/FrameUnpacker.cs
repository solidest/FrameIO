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
    public class FrameUnpacker : IFrameUnpack
    {
        FrameInfo _fi;
        SegmentUnpackInfo[] _segups;
        private MemoryStream _buff;
        private int _acceptbytes;           //已经接收的字节大小
        private int _bit_pos;               //缓冲位偏移
        private ushort _seg_pos;            //当前字段位置
        private int _nextsize;              //下一块内存大小

        private int _firstsize;

        public FrameUnpacker(FrameInfo fi)
        {
            _fi = fi;
            _segups = new SegmentUnpackInfo[fi.SegmentsCount];
            for(int i= 0; i<_segups.Length; i++)
            {
                _segups[i] = new SegmentUnpackInfo();
            }

            //_firstsize = 
        }

        private void Reset()
        {
            for (int i = 0; i < _segups.Length; i++)
            {
                _segups[i] = new SegmentUnpackInfo();
            }
            _buff.Seek(0, SeekOrigin.Begin);
            _acceptbytes = 0;                   //已经接收的字节大小
            _bit_pos = 0;                       //缓冲位偏移
            _seg_pos = 0;                       //当前字段位置
            _nextsize = _firstsize;             //下一块内存大小
        }

        public int FirstBlockSize => throw new NotImplementedException();

        public int AppendBlock(byte[] buff)
        {
            _buff.Write(buff, 0, _nextsize);
            _acceptbytes += _nextsize;
            var newpos = _fi[_seg_pos].Unpack(buff, ref _bit_pos, _segups[_seg_pos]);
            while(_bit_pos != _acceptbytes*8)
            {
                var res = _fi[newpos].Unpack(buff, ref _bit_pos, _segups[newpos]);
                if (res == 0)
                    newpos += 1;
                else
                    newpos = res;
            }

            _seg_pos = newpos;
            if (_seg_pos == _fi.SegmentsCount) return 0;

            newpos = _fi[_seg_pos].TryUnpack(_seg_pos, _segups[_seg_pos]);
            int nextbitlen = _segups[_seg_pos].NeedBitLen;
            while(true)
            {
                var res = _fi[newpos].TryUnpack(_seg_pos, _segups[newpos]);
                if (_segups[newpos].NeedBitLen < 0)
                    break;
                else
                    nextbitlen += _segups[newpos].NeedBitLen;
                if (res == 0)
                    newpos += 1;
                else
                    newpos = res;
                if (newpos == _fi.SegmentsCount)
                    break;
            }

            _nextsize = nextbitlen / 8;
            return _nextsize;
        }

        public IFrameData Unpack()
        {
            _buff.Close();

            Reset();
            return null; 
        }

    }

    public struct SegmentUnpackInfo
    {
        public bool IsUnpack;
        public int BitStart;
        public int BitLen;
        public int NeedBitLen;
    }
}
