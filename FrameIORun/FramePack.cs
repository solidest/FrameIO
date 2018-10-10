using System;
using System.Collections.Generic;
using FrameIO.Interface;
using FrameIO.Main;

namespace FrameIO.Run
{
    public class FramePack : IFramePack
    {
        //数据帧首字段组块
        private SegBlockInfoGroup _rootseggp;

        //运行时字段列表
        private Dictionary<string, SegPack> _runseglist = new Dictionary<string, SegPack>();


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

        //添加字段到字典
        static private void AddSegToDic(Dictionary<string, SegPack> dic, SegBlockInfoGroup segi)
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
                    dic.Add(segb.FullName, new SegPack(segb));
                }
            }
        }

        //打包函数
        public byte[] Pack()
        {
            throw new NotImplementedException();
        }

        //设置字段的数据内容

        public void SetSegmentValue(string segmentname, ushort value)
        {
            throw new NotImplementedException();
        }

        public void SetSegmentValue(string segmentname, ushort? value)
        {
            throw new NotImplementedException();
        }

        public void SetSegmentValue(string segmentname, ushort?[] value)
        {
            throw new NotImplementedException();
        }

        public void SetSegmentValue(string segmentname, bool? value)
        {
            throw new NotImplementedException();
        }
    }
}
