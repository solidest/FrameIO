﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;
using FrameIO.Main;

namespace FrameIO.Run
{
    public class FrameUnpack : IFrameUnpack, IUnpackFrameRun
    {
        //根字段组
        private SegBlockInfoGroup _rootbkgr;

        //当前字段位置
        private SegBlockInfo _seg_pos = null;

        //未填充数值的首字段位置
        private SegBlockInfo _seg_needfill = null;

        //第一个需要计算大小的字段位置
        private SegBlockInfo _firstrunseg_pos = null;

        //下一块内存大小
        private int _nextsize;

        //内存缓存
        private MemoryStream _cach = null;

        //缓存区保存的内存总大小
        private int _totalbytesize = 0;

        //当前内存缓冲指针位偏移
        private int _bit_offset = 0;

        //运行时字段列表
        private Dictionary<string, SegRunUnpack> _runseglist = new Dictionary<string, SegRunUnpack>();

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
                    bitlen += nextseg.BitSizeNumber * nextseg.RepeatedNumber;
                    _seg_pos = nextseg;
                }
                else
                    break;

                nextseg = GetNextSegBlock(nextseg, false);
            }
            
            if (bitlen % 8 != 0) throw new Exception("数据解析时出现非整字节");
            FirstBlockSize = bitlen / 8;
            _nextsize = FirstBlockSize;
            _firstrunseg_pos = _seg_pos;
            _seg_needfill = _rootbkgr.SegBlockList[0];
        }

        //重置
        private void Reset()
        {
            _cach = null;
            _totalbytesize = 0;
            _nextsize = FirstBlockSize;
            _seg_pos = _firstrunseg_pos;
            _seg_needfill = _rootbkgr.SegBlockList[0];

            _bit_offset = 0;
            foreach(var s in _runseglist.Values)
            {
                s.Reset();
            }
            _runseglist.Clear(); 
        }
        
        //填充一组字段的值 并返回下一组内存大小
        private int FillValueAndStep()
        {
            //填充字段：从 _seg_needfill 到 _seg_pos
            var buff = _cach.GetBuffer();
            var runseg = _seg_needfill.SegUnpack;
            while (true)
            {
                runseg.ReadValue(buff, ref _bit_offset, this);
                if (runseg.RefSegBlock == _seg_pos) break;
                runseg = runseg.NextRunSeg;
                if (runseg == null) break;
            } ;
            _seg_needfill = GetNextSegBlock(_seg_pos, true);

            //结尾
            if (_seg_needfill == null)
            {
                _seg_pos = null;
                return 0;
            }

            //至少需要追加一个字段
            var npos = _seg_needfill;
            int bitlen = (int)npos.SegUnpack.GetBitLen(this);
            _seg_pos = npos;

            //还需继续追加固定大小字段
            npos = GetNextSegBlock(npos, false);
            while (npos != null)
            {
                if (npos.IsFixed)
                {
                    bitlen += npos.BitSizeNumber * npos.RepeatedNumber;
                    _seg_pos = npos;
                }
                else
                    break;

                npos = GetNextSegBlock(npos, false);
            }

            if (bitlen % 8 != 0) throw new Exception("数据解析时出现非整字节");
            return bitlen / 8;
        }

        //首块内存大小
        public int FirstBlockSize { get; }

        //追加内存块
        public int AppendBlock(byte[] buffer)
        {
            if (buffer.Length != _nextsize)
            {
                throw new Exception("AppendBlock调用出错");
            }

            if (_cach == null) _cach = new MemoryStream();
            
            _cach.Write(buffer, 0, _nextsize);
            _totalbytesize += buffer.Length;
            _nextsize = FillValueAndStep();
            return _nextsize;
        }

        //解包函数
        public IFrameData Unpack()
        {
            if(_cach == null || _nextsize !=0)
                throw new Exception("AppendBlock调用出错");
            var ret = new FrameData(_runseglist);
            Reset();
            return ret;
        }


        #region --Helper--

        //取给定字段组的下一字段
        private SegBlockInfo GetNextSegBlock(SegBlockInfo lastsegb, SegBlockInfoGroup segg, bool intoOneOf)
        {
            if (segg == null) return null;
            if (segg.Next == null) return GetNextSegBlock(lastsegb, segg.Parent, intoOneOf);
            var nextg = segg.Next;
            if (nextg.IsOneOfGroup)
            {
                if (!intoOneOf) return null;
                var iv = FindUnpackSegRun(nextg.OneOfSegFullName).NumberValue;
                foreach(var gp in nextg.OneOfGroupList)
                {
                    if(iv == gp.Key)
                    {
                        var ret = gp.Value.SegBlockList.First();
                        UnionSegRun(lastsegb, ret);
                        return ret;
                    }
                }
                throw new Exception("进入 oneof 未找到匹配分支");
            }
            else
            {
                var ret = nextg.SegBlockList[0];
                UnionSegRun(lastsegb, ret);
                return ret;
            }
        }

        //取给定字段的下一字段
        private SegBlockInfo GetNextSegBlock(SegBlockInfo segb, bool intoOneOf)
        {
            if (segb.Idx == segb.Parent.SegBlockList.Count - 1)
                return GetNextSegBlock(segb, segb.Parent, intoOneOf);
            else
            {
                var ret = segb.Parent.SegBlockList[segb.Idx + 1];
                UnionSegRun(segb, ret);
                return ret;
            }
        }

        //连接两个字段的运行时
        private static void UnionSegRun(SegBlockInfo l, SegBlockInfo r)
        {
            if (r.SegUnpack == null) r.SegUnpack = new SegRunUnpack(r);
            if (l.SegUnpack == null) l.SegUnpack = new SegRunUnpack(l);

            l.SegUnpack.NextRunSeg = r.SegUnpack;
            r.SegUnpack.NextRunSeg = null;
        }

        #endregion

        #region --CalcuValue--

        public int ByteSizeOf(string segname)
        {
            if (_runseglist.ContainsKey(segname))
                return  _runseglist[segname].RefSegBlock.SegUnpack.GetBitLen(this)/8;
            throw new Exception("解包过程中意外调用了ByteSizeOf");
        }

        public double GetSegValue(string idfullname)
        {
            return _runseglist[idfullname].GetEvalValue();
        }

        public void AddUnpackSeg(string idfullname, SegRunUnpack seg)
        {
            _runseglist.Add(idfullname, seg);
        }

        public SegRunUnpack FindUnpackSegRun(string fullname)
        {
            if (_runseglist.ContainsKey(fullname))
                return _runseglist[fullname];
            return null;
        }

        #endregion


    }
}