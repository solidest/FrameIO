using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;
using FrameIO.Main;

namespace FrameIO.Run
{
    public class FrameUnpack : IFrameUnpack
    {
        //根字段组
        private SegBlockInfoGroup _rootbkgr;

        //当前字段位置
        private SegBlockInfo _seg_pos = null;

        //第一个需要计算大小的字段位置
        private SegBlockInfo _firstseg_pos = null;

        //下一块内存大小
        private int _nextsize;

        //内存缓存
        private MemoryStream _cach = null;

        //当前内存缓冲指针位置
        private int _mem_pos = 0;

        public FrameUnpack(SegBlockInfoGroup fri)
        {
            _rootbkgr = fri;
            int bitlen = 0;
            var nextseg = _rootbkgr.SegBlockList[0];

            //初始化首块内存大小
            while(nextseg!=null)
            {
                if (nextseg.IsFixed)
                {
                    bitlen += nextseg.BitLenNumber;
                    _seg_pos = nextseg;
                }
                else
                    break;

                nextseg = GetNextSegBlock(nextseg, false);
            }
            
            if (bitlen % 8 != 0) throw new Exception("数据解析时出现非整字节");
            FirstBlockSize = bitlen / 8;
            _nextsize = FirstBlockSize;
            _firstseg_pos = _seg_pos;
        }


        //取下一可计算字段内存块大小 到达结尾返回0
        private int GetNextSize(SegBlockInfo pos)
        {
            var npos = GetNextSegBlock(pos, true);

            //结尾
            if(npos == null) 
            {
                _seg_pos = null;
                return 0;
            }

            //计算大小
            int bitlen = npos.BitLenNumber; //TODO
            _seg_pos = npos;

            //追加固定大小字段
            npos = GetNextSegBlock(npos, false);
            while (npos != null)
            {
                if (npos.IsFixed)
                {
                    bitlen += npos.BitLenNumber;
                    _seg_pos = npos;
                }
                else
                    break;

                npos = GetNextSegBlock(npos, false);
            }
                       
            if (bitlen % 8 != 0) throw new Exception("数据解析时出现非整字节");
            return bitlen/8;
        }

        //取给定字段组的下一字段
        private SegBlockInfo GetNextSegBlock(SegBlockInfoGroup segg, bool intoOneOf)
        {
            if (segg == null) return null;
            if (segg.Next == null) return GetNextSegBlock(segg.Parent, intoOneOf);
             var nextg = segg.Next;         
            if(nextg.IsOneOfGroup)
            {
                if (!intoOneOf) return null;
                //TODO 计算分支跳转
                return null;
            }
            else
            {
                return nextg.SegBlockList[0];
            }
        }

        //取给定字段的下一字段
        private SegBlockInfo GetNextSegBlock(SegBlockInfo segb, bool intoOneOf)
        {
            if (segb.Idx == segb.Parent.SegBlockList.Count - 1)
                return GetNextSegBlock(segb.Parent, intoOneOf);
            else
                return segb.Parent.SegBlockList[segb.Idx + 1];
        }

        //首块内存大小
        public int FirstBlockSize { get; }

        //追加内存块
        public int AppendBlock(byte[] buffer)
        {
            if (_cach == null) _cach = new MemoryStream();

            if (buffer.Length != _nextsize)
            {
                throw new Exception("AppendBlock调用出错");
            }
            _cach.Write(buffer, 0, _nextsize);
            var buff = _cach.GetBuffer();
            //TODO 填充对应的字段值
            _nextsize = GetNextSize(_seg_pos);
            return _nextsize;
        }

        //解包函数
        public FrameBase Unpack()
        {
            if(_nextsize!=0)
                throw new Exception("AppendBlock调用出错");

            //填充全部字段的值
            //TODO

            //检查数值准确性

            //清除
            _cach = null;
            _seg_pos = _firstseg_pos;
            return null;
        }
    }
}
