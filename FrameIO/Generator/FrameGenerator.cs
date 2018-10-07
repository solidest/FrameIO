﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        //生成代码信息
        static public List<FrameInfo> Generate(IOProject pj)
        {
            Reset();
            _pj = pj;

            var ret = new List<FrameInfo>();
            foreach(var fr in pj.FrameList)
            {

                var rootseginfo = new FrameSegmentInfo();
                if (!CreateSegTree(fr, rootseginfo)) return null;

                var rootblockinfo = CreateUnpackBlockInfo(rootseginfo, fr);
                if (rootblockinfo == null) return null;

                var fri = new FrameInfo()
                {
                    RootSegmentInfo = rootseginfo,
                    RootSegBlockGroupInfo = rootblockinfo,
                    TheFrame = fr
                };
                ret.Add(fri);
            }
            return ret;

        }


        #region --Tree--

        //生成字段树
        static private bool CreateSegTree(Frame rootFrame, FrameSegmentInfo rootSegInfo)
        {
            foreach (var seg in rootFrame.Segments)
            {
                if (!AppendSegToTree(seg, rootFrame, rootSegInfo, false, seg.Syid)) return false;
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

            if (typeof(FrameSegmentBlock) == seg.GetType())
            {
                var bseg = (FrameSegmentBlock)seg;
                switch (bseg.UsedType)
                {
                    case BlockSegType.None:
                        LastErrorSyid = bseg.Syid;
                        LastErrorInfo = "block字段必须设置type属性";
                        return false;

                    case BlockSegType.DefFrame:
                        foreach (var sg in bseg.DefineSegments)
                        {
                            if (!AppendSegToTree(sg, null, fin, false, sg.Syid)) return false;
                        }
                        break;

                    case BlockSegType.RefFrame:
                        var fr = FindFrame(bseg.RefFrameName);
                        if (fr == null)
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
                        foreach (var oi in bseg.OneOfCaseList)
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
                            foreach (var sg in ofr.Segments)
                            {
                                if (!AppendSegToTree(sg, ofr, afin, true, syid)) return false;
                            }
                        }

                        break;
                    default:
                        Debug.Assert(false);
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
                    LastErrorInfo = string.Format("循环引用了数据帧【{0}】", reffr.Name);
                    return true;
                }
                fri = fri.Parent;
            }
            return false;
        }

        //查找数据帧
        static private Frame FindFrame(string name)
        {
            foreach (var f in _pj.FrameList)
            {
                if (f.Name == name) return f;
            }
            return null;
        }

        #endregion

        #region --Unpack--

        //创建解包代码
        static private SegBlockInfoGroup CreateUnpackBlockInfo(FrameSegmentInfo rootseg, Frame theframe)
        {
            var rootg = new SegBlockInfoGroup();
            var nextg = rootg;
            foreach(var segi in rootseg.Children)
            {
                nextg = AppendSegmentInfo(segi, nextg);
                if (nextg == null) return null; 
            }

            if(LastErrorInfo =="" )
            {
                if(rootg.SegBlockList.Count==0)
                {
                    LastErrorInfo = "未找到可解析的字段;";
                    LastErrorSyid = theframe.Syid;
                    return null;
                }

                if(!rootg.SegBlockList[0].IsFixed)
                {
                    LastErrorInfo = "必须指定数据帧首字段长度";
                    LastErrorSyid = theframe.Syid;
                    return null;
                }
            }
            return rootg;
        }

        //添加字段到列表组
        static SegBlockInfoGroup AppendSegmentInfo(FrameSegmentInfo segi, SegBlockInfoGroup sseggroup)
        {
            var ty = segi.Segment.GetType();
            var sseg = new SegBlockInfo(sseggroup.SegBlockList.Count)
            {
                FullName = GetSegFullName(segi),
                ShortName = segi.ID,
                Syid = segi.Syid,
                Parent = sseggroup
            };

            //整数字段
            if(ty == typeof(FrameSegmentInteger))
            {
                var seg = (FrameSegmentInteger)segi.Segment;
                sseg.IsFixed = seg.Repeated.IsConst();
                if (sseg.IsFixed)
                {
                    sseg.LenNumber = seg.BitCount * (int)seg.Repeated.GetConstValue();
                    sseggroup.SegBlockList.Add(sseg);
                    return sseggroup;
                }
                else
                {
                    var expbitcount = new Exp() { Op = exptype.EXP_INT, ConstStr = seg.BitCount.ToString() };
                    sseg.LenExp = new Exp() { Op = exptype.EXP_MUL, LeftExp = expbitcount, RightExp = seg.Repeated };
                    if (!CanExp(sseg.LenExp, sseggroup.SegBlockList, sseggroup.CanUseThis))
                    {
                        LastErrorInfo = "使用了无法解析的表达式";
                        LastErrorSyid = seg.Syid;
                        return null;
                    }
                    sseggroup.SegBlockList.Add(sseg);
                    return sseggroup;
                }
            }

            //浮点字段
            if (ty == typeof(FrameSegmentReal))
            {
                var seg = (FrameSegmentReal)segi.Segment;
                sseg.IsFixed = seg.Repeated.IsConst();
                if (sseg.IsFixed)
                {
                    sseg.LenNumber = seg.IsDouble?64:32 * (int)seg.Repeated.GetConstValue();
                    sseggroup.SegBlockList.Add(sseg);
                    return sseggroup;
                }
                else
                {
                    var expbitcount = new Exp() { Op = exptype.EXP_INT, ConstStr = (seg.IsDouble ? 64 : 32).ToString() };
                    sseg.LenExp = new Exp() { Op = exptype.EXP_MUL, LeftExp = expbitcount, RightExp = seg.Repeated };
                    if (!CanExp(sseg.LenExp, sseggroup.SegBlockList, sseggroup.CanUseThis))
                    {
                        LastErrorInfo = "使用了无法解析的表达式";
                        LastErrorSyid = seg.Syid;
                        return null;
                    }
                    sseggroup.SegBlockList.Add(sseg);
                    return sseggroup;
                }
            }

            //文本字段
            if (ty == typeof(FrameSegmentText))
            {
                var seg = (FrameSegmentText)segi.Segment;
                sseg.IsFixed = seg.Repeated.IsConst()&& seg.ByteSize.IsConst();
                if (sseg.IsFixed)
                {
                    sseg.LenNumber = (int)(seg.ByteSize.GetConstValue() * 8 *seg.Repeated.GetConstValue());
                    sseggroup.SegBlockList.Add(sseg);
                    return sseggroup;
                }
                else
                {
                    var expbyteszie = new Exp() { Op = exptype.EXP_INT, ConstStr ="8" };
                    var expbitcount = new Exp() { Op = exptype.EXP_MUL, LeftExp = seg.ByteSize, RightExp = expbyteszie };
                    sseg.LenExp = new Exp() { Op = exptype.EXP_MUL, LeftExp = expbitcount, RightExp = seg.Repeated };
                    if (!CanExp(sseg.LenExp, sseggroup.SegBlockList, sseggroup.CanUseThis))
                    {
                        LastErrorInfo = "使用了无法解析的表达式";
                        LastErrorSyid = seg.Syid;
                        return null;
                    }
                    sseggroup.SegBlockList.Add(sseg);
                    return sseggroup;
                }
            }

            //block字段
            if (ty == typeof(FrameSegmentBlock))
            {

                SegBlockInfoGroup newgroup = null;
                if (sseggroup.SegBlockList.Count == 0)
                {
                    newgroup = sseggroup;
                }
                else
                {
                    newgroup = new SegBlockInfoGroup();
                    sseggroup.Next = newgroup;
                }

                var seg = (FrameSegmentBlock)segi.Segment;
                newgroup.Parent = sseggroup.Parent;
                newgroup.Repeated = seg.Repeated;
                newgroup.ByteSize = seg.ByteSize;


                switch (seg.UsedType)
                {
                    case BlockSegType.RefFrame:
                    case BlockSegType.DefFrame:
                        {
                            foreach (var segii in segi.Children)
                            {
                                newgroup = AppendSegmentInfo(segii, newgroup);
                                if (newgroup == null) return null;
                            }
                            return newgroup;
                        }

                    case BlockSegType.OneOf:
                        {
                            newgroup.IsOneOfGroup = true;
                            var grouplist = new Dictionary<string, SegBlockInfoGroup>();
                            foreach(var segii in segi.Children)
                            {
                                var rootng = new SegBlockInfoGroup() { Parent = newgroup, ByteSize = newgroup.ByteSize };
                                var ng = rootng;
                                foreach(var segiii in segii.Children)
                                {
                                    ng = AppendSegmentInfo(segiii, ng);
                                    if (ng == null) return null;
                                }
                                grouplist.Add(segii.ID, rootng);
                            }
                            newgroup.OneOfGroupList = grouplist;
                            return newgroup;
                        }
                }
         }

            return null;
        }

        //判断表达式是否可计算
        static private bool CanExp(Exp exp, IList<SegBlockInfo> seglist, bool canThis)
        {
            //TODO 
            return true;
        }


        //取字段全名
        static private string GetSegFullName(FrameSegmentInfo segi)
        {
            var ret = segi.ID;
            while(segi.Parent!=null)
            {
                ret = segi.Parent.ID + "." + ret;
                segi = segi.Parent;
            }
            return ret;
        }


        #endregion

        #region --class--

        //数据帧结构
        public class FrameInfo
        {
            public FrameSegmentInfo RootSegmentInfo { get; set; }
            public SegBlockInfoGroup RootSegBlockGroupInfo { get; set; }
            public Frame TheFrame { get; set; }
            
        }


        //字段结构群组
        public class SegBlockInfoGroup
        {
            public List<SegBlockInfo> SegBlockList { get; set; } = new List<SegBlockInfo>();

            public Exp Repeated { get; set; } = new Exp { Op = exptype.EXP_INT, ConstStr = "1" };
            public Exp ByteSize { get; set; } = new Exp { Op = exptype.EXP_INT, ConstStr = "0" };
            public bool CanUseThis
            {
                get
                {
                    if (ByteSize.IsConst() && ByteSize.GetConstValue() == 0) return false;
                    return true;
                }
            }
            public bool IsOneOfGroup { get; set; } = false;
            public Dictionary<string, SegBlockInfoGroup> OneOfGroupList { get; set; }
            public SegBlockInfoGroup Next { get; set; }
            public SegBlockInfoGroup Parent { get; set; }
        }

        //字段结构
        public class SegBlockInfo
        {
            public SegBlockInfo(int idx)
            {
                Idx = idx;
            }
            public int Idx { get; private set; }
            public int LenNumber { get; set; }
            public Exp LenExp { get; set; }
            public bool IsFixed { get; set; } = false;
            public string ShortName { get; set; }
            public string FullName { get; set; }
            public int Syid { get; set; }
            public SegBlockInfoGroup Parent {get; set;}
        }

        //字段信息
        public class FrameSegmentInfo
        {
            //字段标识
            public string ID { get; set; } = "";

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
    #endregion
}
