using System;
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

        static private void Reset()
        {
            LastErrorInfo = "";
            LastErrorSyid = -1;
            _pj = null;
            ErrorList = new Dictionary<int, string>();
            FrameSegmentList = new Dictionary<Frame, Dictionary<string, Frame>>();
        }

        //"枚举名称重复";
        //"枚举组成项名称重复";
        //"分系统名称重复";
        //"通道名称重复";
        //"通道参数重复设置";
        //"操作名称重复";
        //"字段值重复设置";
        //"属性名称重复";
        //"数据帧名称重复";
        //"字段名称重复";
        //"字段属性重复设置";
        //"字段属性与字段类型不匹配";
        //"OneOf 选择项名称重复";
        //"未定义的enum引用";

        //执行语法检查
        static public bool CheckProject(IOProject pj)
        {
            Reset();
            _pj = pj;
            _proptypelist = _pj.GetPropertyTypeList("");

            CheckEnumSysName();
            CheckFrames();
            CheckSubsys();

            return ErrorList.Count==0;
        }


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
                    return new string[1] { "portname"};
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
                    if (Helper.ValidateIsInt(op.OptionValue)||Helper.ValidateIsReal(op.OptionValue))
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
                foreach(var map in ac.Maps)
                {
                    if (map.FrameSegName != "" && !IsContainSegment(map.FrameSegName, ac.FrameName)) 
                        AddErrorInfo(map.Syid, "引用的数据帧字段不正确");
                    //HACK if (!map.SysPropertyName.StartsWith("@") && sys.Propertys.Where(p => p.Name == map.SysPropertyName).Count() == 0)
                    //    AddErrorInfo(map.Syid, "引用的分系统属性不正确");
                }
            }
        }

        //引用的字段是否为数值字段
        static private bool IsContainSegment(string segname, string frmname)
        {
            var frms = _pj.FrameList.Where(p => p.Name == frmname);
            var frm = frms.First();
            if (frm == null) return false;
            var segs = FrameSegmentList[frm];
            if (segs.Keys.Contains(segname)) return true;

            var nms = segname.Split('.');
            int imy = -1;
            Frame findfrm = null;
            for(int i=0; i<nms.Length; i++)
            {
                var n = nms[0];
                for (int ii = 1; ii <= i; ii++)
                    n = n + "." + nms[ii];
                if (segs.Keys.Contains(n))
                {
                    imy = i;
                    findfrm = segs[n];
                }
                else
                    break;
            }
            if (imy == -1 || findfrm == null) return false;
            var refn = nms[imy + 1];
            for (int ii = imy + 2; ii < nms.Length; ii++)
                refn = refn + "." + nms[ii];
            return IsContainSegment(refn, findfrm.Name);
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

        //检查枚举与分系统是否重名
        static private void CheckEnumSysName()
        {
            foreach(var em in _pj.EnumdefList)
            {
                foreach (var sys in _pj.SubsysList)
                    if (em.Name == sys.Name)
                        AddErrorInfo(sys.Syid, "分系统定义与枚举定义重名");
            }
        }


        #endregion

        #region --检查数据帧--

        static private void CheckFrames()
        {
            //数据帧没有字段
            foreach(var frm in _pj.FrameList)
            {
                if (frm.Segments == null || frm.Segments.Count == 0)
                    AddErrorInfo(frm.Syid, "数据帧字段不能为空");
                else
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
                            segns.Add(bseg.Name + "." + mys.Value, null);
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
