using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using FrameIO.Interface;
using FrameIO.Main;

namespace FrameIO.Run
{
    public class FramePack : IFramePack, IPackFrameRun
    {
        //数据帧首字段组块
        private SegBlockInfoGroup _rootseggp;

        //运行时字段列表
        private Dictionary<string, SegRunPack> _runseglist = new Dictionary<string, SegRunPack>();

        //构造函数
        public FramePack(SegBlockInfoGroup fri)
        {
            _rootseggp = fri;

            //添加全部字段
            var next = fri;
            while(next!=null)
            {
                AddSegToDic(_runseglist, next);
                next = next.Next;
            }

        }

        //重置
        public void Reset()
        {
            foreach (var sp in _runseglist.Values)
                sp.Reset();
        }

        //添加字段到字典
        static private void AddSegToDic(Dictionary<string, SegRunPack> dic, SegBlockInfoGroup segi)
        {
            if(segi.IsOneOfGroup)
            {
                foreach(var sg in segi.OneOfGroupList.Values)
                {
                    AddSegToDic(dic, sg);
                }
            }
            else
            {
                foreach (var segb in segi.SegBlockList)
                {
                    segb.SegPack = new SegRunPack(segb);
                    dic.Add(segb.FullName, segb.SegPack);
                }
            }
        }

        //打包函数
        public byte[] Pack()
        {
            var commit = new MemoryStream();
            using (commit)
            {
                ulong cach = 0;
                int cach_pos = 0;
                int commitbytelen = 0;

                var seg = _rootseggp.SegBlockList[0];
                while(seg != null)
                {
                    seg.SegPack.WriteValue(commit, ref commitbytelen, ref cach, ref cach_pos, this);
                    seg = GetNextSegBlock(seg);
                }
                if(cach_pos!=0)
                {
                    if (cach_pos % 8 != 0) throw new Exception("打包完成时出现非整字节");
                    commit.Write(BitConverter.GetBytes(cach), 0, cach_pos / 8);
                }
            }
            return commit.ToArray();
        }

        #region --Next Step--


        //取给定字段组的下一字段
        private SegBlockInfo GetNextSegBlock(SegBlockInfo lastsegb, SegBlockInfoGroup segg)
        {
            if (segg == null) return null;
            if (segg.Next == null) return GetNextSegBlock(lastsegb, segg.Parent);
            var nextg = segg.Next;
            if (nextg.IsOneOfGroup)
            {
                ulong iv = 0;
                var refseg = _runseglist[nextg.OneOfSegFullName];
                if(refseg.IsSetValue)
                {
                    iv = refseg.NumberValue;
                    foreach (var gp in nextg.OneOfGroupList)
                    {
                        if (iv == gp.Key)
                        {
                            return gp.Value.SegBlockList.First();
                        }
                    }
                }
                else
                {
                    foreach (var gp in nextg.OneOfGroupList)
                    {
                        var retseg = gp.Value.SegBlockList.First();
                        if(retseg.SegPack.IsSetValue)
                        {
                            return retseg;
                        }
                    }
                }

                throw new Exception("未设置 oneof 分支的匹配项");
            }
            else
            {
                var ret = nextg.SegBlockList[0];
                return ret;
            }
        }

        //取给定字段的下一字段
        private SegBlockInfo GetNextSegBlock(SegBlockInfo segb)
        {
            if (segb.Idx == segb.Parent.SegBlockList.Count - 1)
                return GetNextSegBlock(segb, segb.Parent);
            else
            {
                var ret = segb.Parent.SegBlockList[segb.Idx + 1];
                return ret;
            }
        }


        #endregion

        #region --SegSegmentValue--

        //设置字段的数据内容

        public void SetSegmentValue(string segmentname, bool? value)
        {
            if(value==null)
                _runseglist[segmentname].SetNumberValue(0);
            else
            {
                if ((bool)value)
                    _runseglist[segmentname].SetNumberValue(1);
                else
                    _runseglist[segmentname].SetNumberValue(0);
            }
        }

        public void SetSegmentValue(string segmentname, byte? value)
        {
            if (value == null)
                _runseglist[segmentname].SetNumberValue(0);
            else
                _runseglist[segmentname].SetNumberValue((byte)value);
        }

        public void SetSegmentValue(string segmentname, sbyte? value)
        {
            if (value == null)
                _runseglist[segmentname].SetNumberValue(0);
            else
                _runseglist[segmentname].SetNumberValue(BitConverter.GetBytes((sbyte)value)[0]);
        }

        public void SetSegmentValue(string segmentname, ushort? value)
        {
            if (value == null)
                _runseglist[segmentname].SetNumberValue(0);
            else
                _runseglist[segmentname].SetNumberValue((ushort)value);
        }

        public void SetSegmentValue(string segmentname, short? value)
        {
            if (value == null)
                _runseglist[segmentname].SetNumberValue(0);
            else
                _runseglist[segmentname].SetNumberValue(BitConverter.ToUInt16(BitConverter.GetBytes((short)value), 0));
        }

        public void SetSegmentValue(string segmentname, uint? value)
        {
            if (value == null)
                _runseglist[segmentname].SetNumberValue(0);
            else
                _runseglist[segmentname].SetNumberValue((uint)value);
        }

        public void SetSegmentValue(string segmentname, int? value)
        {
            if (value == null)
                _runseglist[segmentname].SetNumberValue(0);
            else
                _runseglist[segmentname].SetNumberValue(BitConverter.ToUInt32(BitConverter.GetBytes((int)value), 0));
        }

        public void SetSegmentValue(string segmentname, ulong? value)
        {
            if (value == null)
                _runseglist[segmentname].SetNumberValue(0);
            else
                _runseglist[segmentname].SetNumberValue((ulong)value);
        }

        public void SetSegmentValue(string segmentname, long? value)
        {
            if (value == null)
                _runseglist[segmentname].SetNumberValue(0);
            else
                _runseglist[segmentname].SetNumberValue(BitConverter.ToUInt64(BitConverter.GetBytes((long)value), 0));
        }

        public void SetSegmentValue(string segmentname, float? value)
        {
            if (value == null)
                _runseglist[segmentname].SetNumberValue(0);
            else
                _runseglist[segmentname].SetNumberValue(BitConverter.ToUInt32(BitConverter.GetBytes((float)value), 0));
        }

        public void SetSegmentValue(string segmentname, double? value)
        {
            if (value == null)
                _runseglist[segmentname].SetNumberValue(0);
            else
                _runseglist[segmentname].SetNumberValue(BitConverter.ToUInt64(BitConverter.GetBytes((double)value), 0));
        }

        public void SetSegmentValue(string segmentname, bool?[] value)
        {
            var segrun = _runseglist[segmentname];
            segrun.SetNumberArraySize(value.Length);
            for(int i=0; i<value.Length; i++)
            {
                if (value[i] == null)
                    segrun.SetNumberArrayAt(i, 0);
                else
                {
                    if ((bool)value[i])
                        segrun.SetNumberArrayAt(i, 1);
                    else
                        segrun.SetNumberArrayAt(i, 0);
                }
            }
        }

        public void SetSegmentValue(string segmentname, byte?[] value)
        {
            var segrun = _runseglist[segmentname];
            segrun.SetNumberArraySize(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == null)
                    segrun.SetNumberArrayAt(i, 0);
                else
                {
                    segrun.SetNumberArrayAt(i, (byte)value[i]);
                }
            }
        }

        public void SetSegmentValue(string segmentname, sbyte?[] value)
        {
            var segrun = _runseglist[segmentname];
            segrun.SetNumberArraySize(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == null)
                    segrun.SetNumberArrayAt(i, 0);
                else
                {
                    segrun.SetNumberArrayAt(i, BitConverter.GetBytes((sbyte)value[i])[0]);
                }
            }
        }

        public void SetSegmentValue(string segmentname, ushort?[] value)
        {
            var segrun = _runseglist[segmentname];
            segrun.SetNumberArraySize(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == null)
                    segrun.SetNumberArrayAt(i, 0);
                else
                {
                    segrun.SetNumberArrayAt(i, (ushort)value[i]);
                }
            }
        }

        public void SetSegmentValue(string segmentname, short?[] value)
        {
            var segrun = _runseglist[segmentname];
            segrun.SetNumberArraySize(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == null)
                    segrun.SetNumberArrayAt(i, 0);
                else
                {
                    segrun.SetNumberArrayAt(i, BitConverter.ToUInt16(BitConverter.GetBytes((short)value[i]), 0));
                }
            }
        }

        public void SetSegmentValue(string segmentname, uint?[] value)
        {
            var segrun = _runseglist[segmentname];
            segrun.SetNumberArraySize(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == null)
                    segrun.SetNumberArrayAt(i, 0);
                else
                {
                    segrun.SetNumberArrayAt(i, (uint)value[i]);
                }
            }
        }

        public void SetSegmentValue(string segmentname, int?[] value)
        {
            var segrun = _runseglist[segmentname];
            segrun.SetNumberArraySize(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == null)
                    segrun.SetNumberArrayAt(i, 0);
                else
                {
                    segrun.SetNumberArrayAt(i, BitConverter.ToUInt32(BitConverter.GetBytes((int)value[i]), 0));
                }
            }
        }

        public void SetSegmentValue(string segmentname, ulong?[] value)
        {
            var segrun = _runseglist[segmentname];
            segrun.SetNumberArraySize(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == null)
                    segrun.SetNumberArrayAt(i, 0);
                else
                {
                    segrun.SetNumberArrayAt(i, (ulong)value[i]);
                }
            }
        }

        public void SetSegmentValue(string segmentname, long?[] value)
        {
            var segrun = _runseglist[segmentname];
            segrun.SetNumberArraySize(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == null)
                    segrun.SetNumberArrayAt(i, 0);
                else
                {
                    segrun.SetNumberArrayAt(i, BitConverter.ToUInt64(BitConverter.GetBytes((long)value[i]), 0));
                }
            }
        }

        public void SetSegmentValue(string segmentname, float?[] value)
        {
            var segrun = _runseglist[segmentname];
            segrun.SetNumberArraySize(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == null)
                    segrun.SetNumberArrayAt(i, 0);
                else
                {
                    segrun.SetNumberArrayAt(i, BitConverter.ToUInt32(BitConverter.GetBytes((float)value[i]), 0));
                }
            }
        }

        public void SetSegmentValue(string segmentname, double?[] value)
        {
            var segrun = _runseglist[segmentname];
            segrun.SetNumberArraySize(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == null)
                    segrun.SetNumberArrayAt(i, 0);
                else
                {
                    segrun.SetNumberArrayAt(i, BitConverter.ToUInt64(BitConverter.GetBytes((double)value[i]), 0));
                }
            }
        }


        #endregion

        #region --IRun--

        public int ByteSizeOf(string segfullname)
        {
            return _runseglist[segfullname].RefSegBlock.SegPack.GetBitLen(this) / 8;
        }

        public double GetSegValue(string segfullname)
        {
            return _runseglist[segfullname].GetEvalValue();
        }

        public SegRunPack FindPackSegRun(string fullname)
        {
            return _runseglist[fullname];
        }

        #endregion
    }
}
