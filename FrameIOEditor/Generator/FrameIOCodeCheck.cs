﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //代码检查
    public class FrameIOCodeCheck
    {
        static private IOProject _pj = null;

        static public string LastErrorInfo { get; private set; }
        static public int LastErrorSyid { get; private set; }

        static public Dictionary<int, string> ErrorList { get; private set; } 

        static private Dictionary<Frame, Dictionary<string, Frame>> FrameSegmentList {  get;  set; }

        static private List<string> _proptypelist;
        //static private Frames2Json _jfrms;

        static private void Reset()
        {
            LastErrorInfo = "";
            LastErrorSyid = -1;
            _pj = null;
            ErrorList = new Dictionary<int, string>();
            FrameSegmentList = new Dictionary<Frame, Dictionary<string, Frame>>();
            //_jfrms = null;
        }


        //执行语法检查
        static public bool CheckProject(IOProject pj)
        {
            Reset();
            _pj = pj;
            //_jfrms = new Frames2Json(_pj);

            _proptypelist = _pj.GetPropertyTypeList();

            CheckNameRepeated();
            CheckFrames();
            CheckSubsys();
            CheckOneOf();
            return ErrorList.Count==0;
        }

        #region --检查子系统--


        #endregion

        #region --检查通道--

        //检查通道
        static private void CheckChannel(SubsysChannel ch)
        {
            //return;
            string[] str = GetChannelStrOptionName(ch.ChannelType);
            string[] nms = GetChannelNumberOptionName(ch.ChannelType);
            foreach (var op in ch.Options)
            {
                if (nms.Contains(op.Name))
                {
                    CheckIsInteger(op.OptionValue, op.Syid);
                }
                else if (str.Contains(op.Name))
                {
                    if (Helper.ValidateIsInt(op.OptionValue) || Helper.ValidateIsReal(op.OptionValue))
                        AddErrorInfo(op.Syid, "参数值设置错误");
                }
                else
                    AddErrorInfo(op.Syid, "参数名称错误");
            }
        }

        static public string[] GetChannelOptionName(syschanneltype chtype)
        {
            var ret1 = GetChannelNumberOptionName(chtype);
            var ret2 = GetChannelStrOptionName(chtype);
            return ret1.Concat(ret2).ToArray();

        }

        static private string[] GetChannelNumberOptionName(syschanneltype chtype)
        {
            switch (chtype)
            {
                case syschanneltype.SCHT_COM:
                    return new string[5] { "baudrate", "parity", "databits", "stopbits", "waittimeout" };
                case syschanneltype.SCHT_CAN:
                    return new string[9] { "devtype", "devind", "channelind", "baudrate", "acccode", "accmark", "mode", "filter", "waittimeout" };
                case syschanneltype.SCHT_TCPSERVER:
                    return new string[2] { "port", "waittimeout" };
                case syschanneltype.SCHT_TCPCLIENT:
                    return new string[2] { "port", "waittimeout" };
                case syschanneltype.SCHT_UDP:
                    return new string[3] { "localport", "remoteport", "waittimeout" };
                case syschanneltype.SCHT_DIO:
                    return new string[4] { "deviceno", "channelidx", "minvalue", "waittimeout" };
            }
            return new string[0];
        }

        static private string[] GetChannelStrOptionName(syschanneltype chtype)
        {
            switch (chtype)
            {
                case syschanneltype.SCHT_COM:
                    return new string[1] { "portname" };
                case syschanneltype.SCHT_CAN:
                    return new string[1] { "vendor" };
                case syschanneltype.SCHT_TCPSERVER:
                    return new string[2] { "serverip", "clientip" };
                case syschanneltype.SCHT_TCPCLIENT:
                    return new string[1] { "serverip" };
                case syschanneltype.SCHT_UDP:
                    return new string[2] { "localip", "remoteip" };
                case syschanneltype.SCHT_DIO:
                    return new string[0];
            }
            return new string[0];
        }



        //检查di do通道
        static private void CheckDiDoOption(IEnumerable<SubsysChannelOption> ops)
        {
            string[] nsm = GetChannelNumberOptionName(syschanneltype.SCHT_DIO);
            foreach (var op in ops)
            {
                if (nsm.Contains(op.Name))
                {
                    CheckIsInteger(op.OptionValue, op.Syid);
                }
                else
                    AddErrorInfo(op.Syid, "参数名称错误");
            }
        }


        //检查udp通道
        static private void CheckUdpOption(IEnumerable<SubsysChannelOption> ops)
        {
            string[] str = GetChannelStrOptionName(syschanneltype.SCHT_UDP);
            string[] nms = GetChannelNumberOptionName(syschanneltype.SCHT_UDP);
            foreach (var op in ops)
            {
                if (nms.Contains(op.Name))
                {
                    CheckIsInteger(op.OptionValue, op.Syid);
                }
                else if (str.Contains(op.Name))
                {
                    if (Helper.ValidateIsInt(op.OptionValue) || Helper.ValidateIsReal(op.OptionValue))
                        AddErrorInfo(op.Syid, "参数值设置错误");
                }
                else
                    AddErrorInfo(op.Syid, "参数名称错误");
            }
        }


        //检查tcpclient通道
        static private void CheckTcpclientOption(IEnumerable<SubsysChannelOption> ops)
        {
            string[] str = GetChannelStrOptionName(syschanneltype.SCHT_TCPCLIENT);
            string[] nms = GetChannelNumberOptionName(syschanneltype.SCHT_TCPCLIENT);
            foreach (var op in ops)
            {
                if (nms.Contains(op.Name))
                {
                    CheckIsInteger(op.OptionValue, op.Syid);
                }
                else if (str.Contains(op.Name))
                {
                    if (Helper.ValidateIsInt(op.OptionValue) || Helper.ValidateIsReal(op.OptionValue))
                        AddErrorInfo(op.Syid, "参数值设置错误");
                }
                else
                    AddErrorInfo(op.Syid, "参数名称错误");
            }
        }

        //检查tcpserver通道
        static private void CheckTcpserverOption(IEnumerable<SubsysChannelOption> ops)
        {
            string[] nms = GetChannelNumberOptionName(syschanneltype.SCHT_TCPSERVER);
            string[] str = GetChannelStrOptionName(syschanneltype.SCHT_TCPSERVER);
            foreach (var op in ops)
            {
                if (nms.Contains(op.Name))
                {
                    CheckIsInteger(op.OptionValue, op.Syid);
                }
                else if (str.Contains(op.Name))
                {
                    if (Helper.ValidateIsInt(op.OptionValue) || Helper.ValidateIsReal(op.OptionValue))
                        AddErrorInfo(op.Syid, "参数值设置错误");
                }
                else
                    AddErrorInfo(op.Syid, "参数名称错误");
            }
        }

        //检查COM通道
        static private void CheckComOption(IEnumerable<SubsysChannelOption> ops)
        {
            string[] nms = GetChannelNumberOptionName(syschanneltype.SCHT_COM);
            string[] str = GetChannelStrOptionName(syschanneltype.SCHT_COM);
            foreach (var op in ops)
            {
                if (str.Contains(op.Name))
                {
                    if (Helper.ValidateIsInt(op.OptionValue) || Helper.ValidateIsReal(op.OptionValue))
                        AddErrorInfo(op.Syid, "参数值设置错误");
                }
                else if (nms.Contains(op.Name))
                {
                    CheckIsInteger(op.OptionValue, op.Syid);
                }
                else
                    AddErrorInfo(op.Syid, "参数名称错误");
            }
        }

        //检查Can通道
        static private void CheckCanOption(IEnumerable<SubsysChannelOption> ops)
        {
            string[] nms = GetChannelNumberOptionName(syschanneltype.SCHT_CAN);
            string[] str = GetChannelStrOptionName(syschanneltype.SCHT_CAN);
            foreach (var op in ops)
            {
                if (op.Name == "vendor")
                {
                    if (op.OptionValue != "\"gzzy\"" && op.OptionValue != "\"yh\"")
                        AddErrorInfo(op.Syid, "CAN制造商类型取值必须为\"gzzy\"或\"yh\"");
                }
                else if (nms.Contains(op.Name))
                {
                    CheckIsInteger(op.OptionValue, op.Syid);
                }
                else
                    AddErrorInfo(op.Syid, "通道参数名称错误");
            }
        }

        static private void CheckIsInteger(string v, int syid)
        {
            if (!Helper.ValidateIsInt(v))
                AddErrorInfo(syid, "必须设置为整数值");
        }
        #endregion
    
        #region --检查分系统--

        static private void CheckSubsys()
        {
            foreach(var sys in _pj.SubsysList)
            {
                CheckSubsysName(sys);

                foreach (var pt in sys.Propertys)
                    CheckProperty(pt);

                foreach(var ch in sys.Channels)
                    CheckChannel(ch);

                foreach (var ac in sys.Actions)
                    CheckAction(ac, sys);
            }
        }


        //检查属性
        static private void CheckProperty(SubsysProperty pt)
        {
            if(!_proptypelist.Contains(pt.PropertyType))
                AddErrorInfo(pt.Syid, "分系统属性类型错误");
        }


        //检查动作
        static private void CheckAction(SubsysAction ac, Subsys sys)
        {
            if (sys.Channels.Where(p => p.Name == ac.ChannelName).Count() == 0)
                AddErrorInfo(ac.Syid, "引用的通道不正确");
            if (_pj.FrameList.Where(p => p.Name == ac.FrameName).Count() == 0)
                AddErrorInfo(ac.Syid, "引用的数据帧不正确");
            else
            {
                var fms = _pj.FrameList.Where(p => p.Name == ac.FrameName);
                var frm = fms.First();
                foreach(var map in ac.LiteMaps)
                {
                    if (!CheckProInMap(map.SysPropertyName, sys))
                    {
                        AddErrorInfo(map.Syid, "引用的分系统属性不正确");
                    }
                    if (!CheckSegInMap(map.FrameSegName, ac.FrameName))
                    {
                        AddErrorInfo(map.Syid, "引用的数据帧字段不正确");
                    }

                }
            }
        }

        //是否包含引用的属性
        static private bool CheckProInMap(string proname, Subsys sys)
        {
            var nms = proname.Split('.');
            if (nms.Length == 0) return false;

            var pros = sys.Propertys.Where(p => p.Name == nms[0]);
            if (pros.Count() == 0) return false;
            if (nms.Length == 1) return true;

            
            //子系统
            var inner = pros.First().PropertyType;
            var segname = proname.Substring(proname.IndexOf(".")+1);
            return CheckSegInMap(segname, inner);
        }


        //是否包含引用的字段
        static private bool CheckSegInMap(string segname, string frmname)
        {
            var frm = FindFrame(frmname);
            if (frm == null) return false;
            var segns = segname.Split('.');
            var segnms = frm.Segments.Select(p => p.Name);
            var refsegs = frm.Segments;
            var mapsegs = new Dictionary<string, string>();
            for (int i = 0; i < segns.Length; i++)
            {
                if (segnms.Where(p => p == segns[i]).Count() == 0) return false;
                if(refsegs!=null)
                {
                    var oseg = refsegs.Where(p => p.Name == segns[i]).First();
                    
                    if(oseg.GetType() == typeof(FrameSegmentBlock))
                    {
                        var bseg = ((FrameSegmentBlock)oseg);
                        switch (bseg.UsedType)
                        {
                            case BlockSegType.DefFrame:
                                segnms = bseg.DefineSegments.Select(p=>p.Name);
                                refsegs = bseg.DefineSegments;
                                break;
                            case BlockSegType.OneOf:
                                segnms = bseg.OneOfCaseList.Select(p => p.EnumItem);
                                refsegs = null;
                                mapsegs.Clear();
                                foreach (var item in bseg.OneOfCaseList)
                                {
                                    mapsegs.Add(item.EnumItem, item.FrameName);
                                }
                                break;
                            case BlockSegType.RefFrame:
                                segnms = FindFrame(bseg.RefFrameName).Segments.Select(p => p.Name);
                                refsegs = FindFrame(bseg.RefFrameName).Segments;
                                break;
                        }
                    }
                }
                else
                {
                    var findfrm = FindFrame(mapsegs[segns[i]]);
                    refsegs = findfrm.Segments;
                    segnms = findfrm.Segments.Select(p => p.Name);
                }

            }

            return true;

        }


        //检查分系统成员名称重复问题
        static private void CheckSubsysName(Subsys sys)
        {
            var nms = new List<string>();
            nms.AddRange(sys.Propertys.Select(p => p.Name));
            nms.AddRange(sys.Channels.Select(p => p.Name));
            nms.AddRange(sys.Actions.Select(p => p.Name));
            foreach(var p in sys.Propertys)
            {
                if (nms.Where(pp => pp == p.Name).Count() > 1)
                    AddErrorInfo(p.Syid, "成员名称重复");
            }
            foreach (var p in sys.Channels)
            {
                if (nms.Where(pp => pp == p.Name).Count() > 1)
                    AddErrorInfo(p.Syid, "成员名称重复");
            }
            foreach (var p in sys.Actions)
            {
                if (nms.Where(pp => pp == p.Name).Count() > 1)
                    AddErrorInfo(p.Syid, "成员名称重复");
            }
        }

        //检查一级对象是否重名
        static private void CheckNameRepeated()
        {
            var names = new List<Tuple<string, int>>();
            names.AddRange(_pj.EnumdefList.Select(p => new Tuple<string, int>(p.Name, p.Syid)));
            names.AddRange(_pj.InnerSubsysList.Select(p => new Tuple<string, int>(p.Name, p.Syid)));
            names.AddRange(_pj.SubsysList.Select(p => new Tuple<string, int>(p.Name, p.Syid)));
            foreach(var t in names)
            {
                if(names.Where(p=>p.Item1==t.Item1).Count()>1)
                {
                    AddErrorInfo(t.Item2, "名称重复");
                }
            }

        }


        #endregion

        #region --检查数据帧--

        static private void CheckFrames()
        {
            //数据帧没有字段
            foreach(var frm in _pj.FrameList)
            {
                //if (frm.Segments == null || frm.Segments.Count == 0)
                //    AddErrorInfo(frm.Syid, "数据帧字段不能为空");
                //else
                CheckFrameSegment(frm);
                
            }

            //检查循环引用
            foreach(var frms in FrameSegmentList)
            {
                foreach(var seg in frms.Value.Where(p=>p.Value!=null))
                {
                    var pfrm = new FrameRef() { Parent = null, TheFrm = frms.Key };
                    if (CircleFrameRef(pfrm, seg.Value))
                        AddErrorInfo(frms.Key.Syid, "数据帧循环引用");
                }
            }

        }

        //是否存在循环引用
        static private bool CircleFrameRef(FrameRef parent, Frame reffrm)
        {
            var p = parent;
            while(p!=null)
            {
                if (p.TheFrm == reffrm)
                    return true;
                p = p.Parent;
            }
            var newfref = new FrameRef() { Parent = parent, TheFrm = reffrm };

            foreach(var subfrm in FrameSegmentList[reffrm].Where(pp=>pp.Value!=null))
            {
                if (CircleFrameRef(newfref, subfrm.Value))
                    return true;
            }


            return false;
        }

        private class FrameRef
        {
            public FrameRef Parent;
            public Frame TheFrm;
        }

        //检查数据帧
        static private void CheckFrameSegment(Frame frm)
        {
            var segns = new Dictionary<string, Frame> ();
            foreach(var seg in frm.Segments)
                CheckSegment(seg, segns, frm);
            FrameSegmentList.Add(frm, segns);
        }

        //检查字段
        static private void CheckSegment(FrameSegmentBase seg, Dictionary<string, Frame> segns, Frame frm)
        {
            var ty = seg.GetType();
            if (ty == typeof(FrameSegmentInteger))
               CheckSegment((FrameSegmentInteger)seg, segns);
            else if (ty == typeof(FrameSegmentReal))
               CheckSegment((FrameSegmentReal)seg, segns);
            else if (ty == typeof(FrameSegmentBlock))
               CheckSegment((FrameSegmentBlock)seg, segns, frm);
        }

        //检查block字段
        static private void CheckSegment(FrameSegmentBlock bseg, Dictionary<string, Frame> segns, Frame frm)
        {
            switch (bseg.UsedType)
            {
                case BlockSegType.None:
                    AddErrorInfo(bseg.Syid, "block 字段必须设置type属性");
                    return;

                case BlockSegType.DefFrame:
                    {
                        var mysegs = new Dictionary<string, Frame>();
                        foreach (var sg in bseg.DefineSegments)
                        {
                            if (sg.GetType() == typeof(FrameSegmentBlock))
                                AddErrorInfo(sg.Syid, "block 字段无法嵌套使用");
                            else
                                CheckSegment(sg, mysegs, null);
                        }                        
                        segns.Add(bseg.Name, null);
                        foreach (var mys in mysegs)
                        {
                            segns.Add(bseg.Name + "." + mys.Key, null);
                        }

                        return;
                    }

                case BlockSegType.RefFrame:
                    {
                        var fr = FindFrame(bseg.RefFrameName);
                        if (fr == null) AddErrorInfo(bseg.Syid, string.Format("未定义的数据帧【{0}】引用", bseg.RefFrameName));
                        segns.Add(bseg.Name, fr);
                        return;
                    }

                case BlockSegType.OneOf:
                    if(!segns.Keys.Contains(bseg.OneOfBySegment)) AddErrorInfo(bseg.Syid, string.Format("错误的字段【{0}】引用", bseg.OneOfBySegment));
                    var em = GetToEnum(frm, bseg.OneOfBySegment);
                    if (em == null)
                    {
                        AddErrorInfo(bseg.Syid, string.Format("oneof 引用的字段【{0}】未正确设置toenum", bseg.OneOfBySegment));
                        return;
                    }
                    var ems = _pj.EnumdefList.Where(p => p.Name == em);
                    if(ems.Count() ==0)
                    {
                        AddErrorInfo(bseg.Syid, string.Format("oneof 引用的字段【{0}】未正确设置toenum", bseg.OneOfBySegment));
                        return;
                    }
                    segns.Add(bseg.Name, null);
                    var emd = ems.First();
                    foreach (var oi in bseg.OneOfCaseList)
                    {
                        if (bseg.OneOfCaseList.Where(p=>p.EnumItem==oi.EnumItem).Count()>1)
                        {
                            AddErrorInfo(bseg.Syid, "OneOf 分支出现重复定义");
                            return;
                        }
                        if (oi.EnumItem!="other" && emd.ItemsList.Where(p=>p.Name==oi.EnumItem).Count()==0)
                        {
                            AddErrorInfo(bseg.Syid, "OneOf 分支名称设置不正确");
                            return;
                        }
                        var frms = _pj.FrameList.Where(p => p.Name == oi.FrameName);
                        if (frms.Count() == 0)
                        {
                            AddErrorInfo(bseg.Syid, "OneOf 分支引用的数据帧不正确");
                            return;
                        }
                        segns.Add(bseg.Name + "." + oi.EnumItem, frms.First());
                    }
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }


        //检查整数字段
        static void CheckSegment(FrameSegmentInteger seg, Dictionary<string, Frame> segns)
        {
            if (seg.BitCount <= 0 || seg.BitCount > 64)
                AddErrorInfo(seg.Syid, "位长设置不正确");

            if (seg.Repeated != null && !seg.Repeated.CanEval(segns.Keys.ToList()))
                AddErrorInfo(seg.Syid, "repeated使用的表达式无法解析");

            if (seg.Value != null && !seg.Value.CanEval(segns.Keys.ToList()))
                AddErrorInfo(seg.Syid, "value使用的表达式无法解析");

            if (seg.ToEnum != null && seg.ToEnum != "" && _pj.EnumdefList.Where(p => p.Name == seg.ToEnum).Count() == 0)
                AddErrorInfo(seg.Syid, "未找到所引用的枚举定义");

            if (seg.CheckRangeBegin != null && seg.CheckRangeBegin.Length > 0 && !segns.Keys.Contains(seg.CheckRangeBegin))
                AddErrorInfo(seg.Syid, "所引用的校验开始字段不正确");

            if (seg.CheckRangeEnd != null && seg.CheckRangeEnd.Length > 0 && !segns.Keys.Contains(seg.CheckRangeEnd))
                AddErrorInfo(seg.Syid, "所引用的校验结尾字段不正确");

            segns.Add(seg.Name, null);

        }

        //检查浮点字段
        static private void CheckSegment(FrameSegmentReal seg, Dictionary<string, Frame> segns)
        {

            if (seg.Repeated != null && !seg.Repeated.CanEval(segns.Keys.ToList()))
                AddErrorInfo(seg.Syid, "repeated 使用的表达式无法解析");

            if (seg.Value != null && !seg.Value.CanEval(segns.Keys.ToList()))
                AddErrorInfo(seg.Syid, "value 使用的表达式无法解析");

            segns.Add(seg.Name, null);
        }

        #endregion

        #region --检查OneOf--

        static private void CheckOneOf()
        {
            foreach(var frm in _pj.FrameList)
            {
                var seg = frm.Segments.Where(p => typeof(FrameSegmentBlock) == p.GetType() && ((FrameSegmentBlock)p).UsedType == BlockSegType.OneOf);
                if (seg.Count() == 0) continue;
                foreach(var oseg in seg)
                {
                    bool find = false;
                    foreach(var fseg in frm.Segments)
                    {
                        if (fseg == oseg) break;
                        if(fseg.Name == ((FrameSegmentBlock)oseg).OneOfBySegment)
                        {
                            find = true;
                            CheckToEnum(fseg);                            
                            break;
                        }
                    }
                    if (!find)
                        AddErrorInfo(oseg.Syid, "设置的oneofby字段不正确");
                }
            }
        }

        static private bool CheckToEnum(FrameSegmentBase seg)
        {
            if (seg.GetType() != typeof(FrameSegmentInteger))
            {
                AddErrorInfo(seg.Syid, "不能作为oneof分支的判断字段");
                return false;
            }
            var iseg = (FrameSegmentInteger)seg;
            if(iseg.ToEnum == null || iseg.ToEnum.Length==0)
            {
                AddErrorInfo(seg.Syid, "oneof分支的判断字段必须设置toenum属性");
                return false;
            }
            if(!_pj.IsEnum(iseg.ToEnum))
            {
                AddErrorInfo(seg.Syid, "toenum属性设置错误");
                return false;   
            }
            return true;
        }
        #endregion

        #region --Helper--

        //查找数据帧
        static private Frame FindFrame(string name)
        {
            foreach (var f in _pj.FrameList)
            {
                if (f.Name == name) return f;
            }
            return null;
        }


        //字段是否设置了toenum属性
        static private string GetToEnum(Frame frm , string segname)
        {
            foreach(var seg in frm.Segments)
            {
                if(seg.Name == segname)
                {
                    if (seg.GetType() != typeof(FrameSegmentInteger))
                        return null;
                    var ve = ((FrameSegmentInteger)seg).ToEnum;
                    if (ve == null || ve == "")
                        return null;
                    else
                        return ve;
                }
            }

            return null;
        }

        //追加错误信息
        static private void AddErrorInfo(int syid, string info)
        {
            if(!ErrorList.Keys.Contains(syid)) ErrorList.Add(syid, info);
        }

        #endregion

    }
}
