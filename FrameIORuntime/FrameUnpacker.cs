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
    internal class FrameUnpacker : IFrameUnpack, IUnpackRunExp
    {
        public int FirstBlockSize { get; private set; }

        private static FrameRuntime _fi;
        private MemoryStream _buff;
        private int _nextsize;              //下一块内存大小

        internal ushort SegPosition { get; set; }

        static FrameUnpacker()
        {
            _fi = FrameRuntime.Run;
        }

        internal FrameUnpacker(ushort startidx, ushort endidx, FrameUnpackerInfo parent)
        {
            Info = new FrameUnpackerInfo(startidx, endidx, parent);
            SegPosition = (ushort)(startidx + 1);
            FirstBlockSize = GetNextByteSize(SegPosition, null);
            _nextsize = FirstBlockSize;
        }

        internal FrameUnpackerInfo Info { get; private set; }

        #region --Unpack--

        public int AppendBlock(byte[] block)
        {
            if(_buff == null) _buff = new MemoryStream();
            if (_nextsize == 0 || block.Length != _nextsize) throw new Exception("runtime");
            var bit_pos = (int)_buff.Position * 8;
            _buff.Write(block, 0, _nextsize);
            int end_bit_pos = (int)_buff.Position * 8;

            _buff.SetLength(_buff.Length + 16);  //HACK 追加16个字节长度
            var buff = _buff.GetBuffer();
            var newpos = SegPosition;
            while(bit_pos != end_bit_pos)
            {
                var res = _fi[newpos].Unpack(buff, ref bit_pos, end_bit_pos, Info[newpos], this);
                if (res == 0)
                    newpos += 1;
                else if (res == ushort.MaxValue)
                    break;
                else
                    newpos = res;
            }

            SegPosition = newpos;

            if(SegPosition == Info.EndIdx)
                _nextsize = 0;
            else
                _nextsize = GetNextByteSize(newpos, buff);

            return _nextsize;
        }

        public ISegmentGettor Unpack()
        {
            var ret = new SegmentGettor(_buff.ToArray(), Info);
            Reset();
            return ret;
        }

        private void Reset()
        {
            SegPosition = (ushort)(Info.StartIdx + 1);        //当前字段位置
            _nextsize = FirstBlockSize;                 //下一块需要的内存大小
            Info = new FrameUnpackerInfo(Info.StartIdx, Info.EndIdx, null);
            if(_buff!=null)_buff.Seek(0, SeekOrigin.Begin);
        }

        private int GetNextByteSize(ushort startidx, byte[] buff)
        {
            int bitlen = 0;
            while (startidx != Info.EndIdx)
            {
                ushort next_seg = 0;
                if (_fi[startidx].TryGetBitLen(buff, ref bitlen, ref next_seg, Info[startidx], this))
                {
                    if (next_seg == 0)
                        startidx += 1;
                    else
                        startidx = next_seg;
                }
                else
                    break;
            }
            if (bitlen % 8 != 0) throw new Exception("runtime");
            return bitlen/8;
        }

        public void AddErrorInfo(string info, SegmentBaseRun seg)
        {
            ushort segidx = 0;
            for(ushort i =Info.StartIdx; i<Info.EndIdx; i++)
            {
                if(_fi[i] == seg)
                {
                    segidx = i;
                    break;
                }
            }
            if(segidx != 0)
            {
                Info.AddErrorInfo(info, segidx);
            }
                
        }

        #endregion


        #region --IRunExp--


        public bool TryGetSegmentValue(ref double value, ushort idx)
        {
            return _fi[idx].TryGetValue(ref value, _buff.GetBuffer(), Info[idx]);
        }

        public bool TryGetSegmentByteSize(ref double size, ushort idx)
        {
            int len = 0;
            ushort nextseg = 0;
            if(_fi[idx].TryGetBitLen(_buff.GetBuffer(), ref len, ref nextseg, Info[idx], this))
            {
                if (len % 8 != 0)
                    throw new Exception("runtime");
                else
                    size = len / 8;
                return true;
            }

            return false;

        }

        public bool TryGetBitLen(byte[] buff, ref int bitlen, ref ushort nextseg, ushort idx)
        {
            return _fi[idx].TryGetBitLen(buff, ref bitlen, ref nextseg, Info[idx], this);
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

        public UnpackInfo GetUnpackInfo(ushort idx)
        {
            return Info[idx];
        }

        public double GetConstValue(ushort exp_idx)
        {
            return _fi.GetConstValue(exp_idx);
        }


        #endregion


    }

}
