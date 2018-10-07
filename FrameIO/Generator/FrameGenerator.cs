using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //数据帧代码生成器
    public class FrameGenerator
    {
        //static private FrameSegmentInfo _rootseg = null;
        //static private Frame _rootframe = null;
        static private IOProject _pj = null;

        static public string LastErrorInfo { get; private set; }
        static public int LastErrorSyid { get; private set; }

        static private void Reset()
        {
            LastErrorInfo = "";
            LastErrorSyid = -1;
            _pj = null;
        }

        //生成代码结构
        static public List<FrameInfo> Generate(IOProject pj)
        {
            Reset();
            _pj = pj;

            var ret = new List<FrameInfo>();
            foreach(var fr in pj.FrameList)
            {

                var rootseginfo = new FrameSegmentInfo();

                if (!CreateSegTree(fr, rootseginfo)) return null;
                int offset = 0;
                if (!CreateUnpack(rootseginfo, ref offset)) return null;

                var fri = new FrameInfo()
                {
                    RootSegmentInfo = rootseginfo,
                    TheFrame = fr
                };
                ret.Add(fri);
            }
            return ret;

        }

        //创建解包代码
        static private bool CreateUnpack(FrameSegmentInfo root, ref int offset)
        {

            return true;
        }

       

        //生成字段树
        static private bool CreateSegTree(Frame rootFrame, FrameSegmentInfo rootSegInfo)
        {
            foreach(var seg in rootFrame.Segments)
            {
                if(!AppendSegToTree(seg, rootFrame, rootSegInfo, false, seg.Syid)) return false;
            }

            if (rootSegInfo.Children.Count == 0)
            {
                if (LastErrorInfo.Length == 0)
                {
                    LastErrorSyid = rootFrame.Syid;
                    LastErrorInfo = string.Format("未能生成数据帧【{0}】的字段", rootFrame.Name);
                }
                return false;
            }
            return true;
        }

        //添加字段到字段树  seg：字段  owner：字段所属数据帧   parentseg：父字段信息   isrefFrame：是否引用的数据帧   syid：解析代码位置标识
        static private bool AppendSegToTree(FrameSegmentBase seg, Frame owner, FrameSegmentInfo parentseg, bool isrefFrame, int syid)
        {
            var fin = new FrameSegmentInfo()
            {
                Segment = seg,
                SegmentOwnerFrame = owner,
                Parent = parentseg,
                IsRefFrame = isrefFrame,
                Syid = syid,
                ID = seg.Name
            };

            if (CyclicRef(fin)) return false;
            parentseg.Children.Add(fin);

            if(typeof(FrameSegmentBlock) == seg.GetType())
            {
                var bseg = (FrameSegmentBlock)seg;
                switch(bseg.UsedType)
                {
                    case BlockSegType.DefFrame:
                        foreach(var sg in bseg.DefineSegments)
                        {
                            if(!AppendSegToTree(sg, null, fin, false, sg.Syid)) return false;
                        }
                        break;

                    case BlockSegType.RefFrame:
                        var fr = FindFrame(bseg.RefFrameName);
                        if(fr == null)
                        {
                            LastErrorSyid = bseg.Syid;
                            LastErrorInfo = string.Format("未定义的数据帧【{0}】引用", bseg.RefFrameName);
                            return false;
                        }
                        foreach (var sg in fr.Segments)
                        {
                            if (!AppendSegToTree(sg, fr, fin, true, syid)) return false;
                        }
                        break;
                    case BlockSegType.OneOf:
                        foreach(var oi in bseg.OneOfCaseList)
                        {
                            //TODO 检查Enum正确性
                            var aseg = new FrameSegmentAuto(oi.EnumItem);
                            var afin = new FrameSegmentInfo()
                            {
                                Segment = aseg,
                                Parent = fin,
                                ID = oi.EnumItem
                            };
                            fin.Children.Add(afin);
                            var ofr = FindFrame(oi.FrameName);
                            if (ofr == null)
                            {
                                LastErrorSyid = bseg.Syid;
                                LastErrorInfo = string.Format("未定义的数据帧【{0}】引用", oi.FrameName);
                                return false;
                            }
                            foreach(var sg in ofr.Segments)
                            {
                                if (!AppendSegToTree(sg, ofr, afin, true, syid)) return false;
                            }
                        }

                        break;
                }
            }
            

            return true;

        }

        //是否存在字段数据帧的循环引用
        static private bool CyclicRef(FrameSegmentInfo child)
        {
            if (!child.IsRefFrame) return false;
            var reffr = child.SegmentOwnerFrame;
            var fri = child.Parent;

            while (fri != null)
            {
                if (fri.SegmentOwnerFrame == reffr)
                {
                    LastErrorSyid = child.Syid;
                    LastErrorInfo = string.Format("循环引用了数据帧【{0}】", reffr.Name );
                    return true;
                }
                fri = fri.Parent;
            }
            return false;
        }

        //查找数据帧
        static private Frame FindFrame(string name)
        {
            foreach(var f in _pj.FrameList)
            {
                if (f.Name == name) return f;
            }
            return null;
        }

        //数据帧结构
        public class FrameInfo
        {
            public FrameSegmentInfo RootSegmentInfo { get; set; }
            public Frame TheFrame { get; set; }
            
        }

        //数据帧内存块结构
        public class FrameBlockInfo
        {
            public class SingleSegmentInfo
            {
                public int Offset { get; set; }
                public int LenNumber { get; set; } 
                public Exp LenExp { get; set; }
                public bool IsFixed { get; set; }
                public string ShortName { get; set; }
                public string FullName { get; set; } 
                public FrameBlockInfo BlockInfo { get; set; }
                public int Syid { get; set; }
            }
            public List<SingleSegmentInfo> SingleSegmentList { get; set; } = new List<SingleSegmentInfo>();
            public Exp NextBlockSize { get; set; }

        }

        //字段结构
        public class FrameSegmentInfo
        {
            //字段标识
            public string ID { get; set; } = "";

            //在内存块中的偏移量
            public int PosInBlock { get; set; } = -1;

            //字段长度
            public int BitLen { get; set; } = -1;

            //父字段
            public FrameSegmentInfo Parent { get; set; }

            //子字段
            public IList<FrameSegmentInfo> Children { get; private set; } = new List<FrameSegmentInfo>();

            //对应的字段
            public FrameSegmentBase Segment { get; set; }

            //字段所属的frame
            public Frame SegmentOwnerFrame { get; set; }

            //是否引用了字段所属frame
            public bool IsRefFrame { get; set; } 
            
            //代码位置
            public int Syid { get; set; }
        }
    }
}
