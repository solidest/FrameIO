using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //代码生成器
    public class FrameIOSharpCodeGenerator
    {
        static private IOutText _tout;
        static private string _newpath = "";
        static private IOProject _pj;

        static public void GenerateSharpCodeFile(IOProject pj, IOutText tout)
        {
            try
            {
                _tout = tout;
                _pj = pj;
                _newpath = tout.GetMainOutPath() + "\\" + pj.Name;

                PrepareDir();

                GenerateFrameFile();
                
                var code = new StringBuilder(GetTemplate("TParameter"));
                ReplaceText(code, "projectname", _pj.Name);
                CreateFile("Parameter", code);

                GenerateEnumFile(pj.EnumdefList);
                GenerateSysFile(pj.SubsysList);
                tout.OutText("信息：代码文件输出完成", false);

            }
            catch (Exception e)
            {
                tout.OutText(e.ToString(), true);
            }
            finally
            {
                _pj = null;
                _newpath = null;
                _tout = null;
            }
        }

        #region --Frame--


        //创建Frame代码
        private static void GenerateFrameFile()
        {
            var cpframe = FrameCompiledBytes.Compile(_pj);

            //数据帧初始化
            var code = new StringBuilder(GetTemplate("TFrame"));
            ReplaceText(code, "project", _pj.Name);
            ReplaceText(code, "contentlist", ToStringList(cpframe.GetBytes()), 4);
            ReplaceText(code, "symbollist", ToStringList(GetSymbols(cpframe.Symbols)), 4);

            //数据帧settor && gettors
            var frm_code = new StringBuilder();
            foreach (var frm in _pj.FrameList)
            {
                frm_code.Append(GetFrameGettorCode(frm, cpframe.Symbols));
                frm_code.Append(GetFrameSettorCode(frm, cpframe.Symbols));
            }
            ReplaceText(code, "framegettorandsettorlist", frm_code.ToString());

            //生成文件
            CreateFile("frame", code);

        }

        private static byte[] GetSymbols(Dictionary<string, ushort> symbols)
        {
            var _sym = new Dictionary<ushort, string>();
            foreach (var sy in symbols)
                _sym.Add(sy.Value, sy.Key);

            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, _sym);
                ms.Close();
                return ms.ToArray();
            }
        }

        #region --Gettor--

        //生成数据帧Gettor代码
        private static string GetFrameGettorCode(Frame frm, Dictionary<string, ushort> symbols)
        {
            var code = new StringBuilder(GetTemplate("TSegmentGettor"));
            ReplaceText(code, "framename", frm.Name);
            ReplaceText(code, "frameid", symbols[frm.Name].ToString());

            var prolist = new List<string>();
            foreach(var seg in frm.Segments)
            {
                GetSegmentGettor(seg, frm.Name + "." + seg.Name, symbols, prolist, frm.Name);
            }

            ReplaceText(code, "propertygetlist", prolist, 2);
            return code.ToString();
        }

        //生成字段取值代码
        private static void GetSegmentGettor(FrameSegmentBase seg, string fullname, Dictionary<string, ushort> symbols, List<string> segcodelist, string framename)
        {
            var ty = seg.GetType();
            if (ty == typeof(FrameSegmentInteger))
                segcodelist.Add( GetSegmentGettor((FrameSegmentInteger)seg, fullname, symbols));
            else if(ty == typeof(FrameSegmentReal))
                segcodelist.Add(GetSegmentGettor((FrameSegmentReal)seg, fullname, symbols));
            else if(ty == typeof(FrameSegmentBlock))
            {
                var bseg = (FrameSegmentBlock)seg;
                switch (bseg.UsedType)
                {
                    case BlockSegType.RefFrame:
                        segcodelist.Add(GetSegmentGettor(bseg, fullname, bseg.RefFrameName, symbols));
                        break;
                    case BlockSegType.DefFrame:
                        GetSegmentGettor(bseg, symbols, segcodelist, framename);
                        break;
                    case BlockSegType.OneOf:
                        GetSegmentGettor(bseg, fullname, symbols, segcodelist);
                        break;
                    case BlockSegType.None:
                        break;
                    default:
                        break;
                }
            }
        }

        //内联数据帧字段
        private static void GetSegmentGettor(FrameSegmentBlock seg, Dictionary<string, ushort> symbols, List<string> segcodelist, string framename)
        {
            //private SegmentCGettor _SegmnetC;  public SegmentCGettor SegmnetC { get { if (_SegmnetC == null) _SegmnetC = new SegmentCGettor(this); return _SegmnetC; } }
            segcodelist.Add(string.Format("public {0}Gettor {0} {{ get {{ if (_{0} == null) _{0} = new {0}Gettor(_gettor); return _{0}; }} }} private {0}Gettor _{0};", seg.Name));
            var code = new StringBuilder(GetTemplate("TInnerClassGettor"));
            ReplaceText(code, "segmentname", seg.Name);
            var segs = new List<string>();
            foreach (var innerseg in seg.DefineSegments)
                GetSegmentGettor(innerseg, framename + "." + seg.Name + "." + innerseg.Name, symbols, segs, framename);
            ReplaceText(code, "innersegmentlist", segs, 1);
            var codelist = code.ToString().Split(Environment.NewLine.ToCharArray());
            foreach (var str in codelist)
                if (str != "") segcodelist.Add(str);
        }

        //oneof字段
        private static void GetSegmentGettor(FrameSegmentBlock seg, string fullname, Dictionary<string, ushort> symbols, List<string> segcodelist)
        {
            segcodelist.Add(string.Format("public {0}Gettor {1} {{ get {{ if (_{1} == null) _{1} = new {0}Gettor(_gettor); return _{1}; }} }}private {0}Gettor _{1};", seg.Name, seg.Name));
            var code = new StringBuilder(GetTemplate("TSwitchClassGettor"));
            ReplaceText(code, "segmentname", seg.Name);
            var itemlist = new List<string>();
            foreach (var map in seg.OneOfCaseList)
            {
                itemlist.Add(string.Format("public {0}Gettor {1} {{ get {{ if (_{1} == null) _{1} = new {0}Gettor(_gettor.GetSubFrame({2})); return _{1}; }} }} private {0}Gettor _{1};", map.FrameName, map.EnumItem, symbols[fullname + "." + map.EnumItem]));
            }
            ReplaceText(code, "caseitemlist", itemlist, 1);
            var codelist = code.ToString().Split(Environment.NewLine.ToCharArray());
            foreach (var str in codelist)
                if (str != "") segcodelist.Add(str);
        }

        //引用数据帧字段
        private static string GetSegmentGettor(FrameSegmentBlock seg, string fullname, string refframename, Dictionary<string, ushort> symbols)
        {
            //public TFrameGettor SegmentB { get { if (_SegmentB == null) _SegmentB = new TFrameGettor(_gettor.GetSubFrame(1)); return _SegmentB; } }
            return string.Format("public {0}Gettor {1} {{ get {{ if (_{1} == null) _{1} = new {0}Gettor(_gettor.GetSubFrame({2})); return _{1}; }}}} private {0}Gettor _{1};", refframename, seg.Name, symbols[fullname]);
        }

        //整数字段
        private static string GetSegmentGettor(FrameSegmentInteger seg, string fullname, Dictionary<string, ushort> symbols)
        {
            //public bool? SegmentA { get => _gettor.GetBool(1); }
            var ty = GetSegmentType(seg.BitCount, seg.Signed);
            if (seg.Repeated.IsIntOne())
                return string.Format("public {0}? {1} {{ get => _gettor.{2}({3}); }}", GetTypeName(ty), seg.Name, GetGetorName(ty), symbols[fullname] );
            else
                return string.Format("public {0}?[] {1} {{ get {{ if (_{1} == null) _{1} = _gettor.{2}Array({3}); return _{1}; }} }} private {0}?[] _{1};", GetTypeName(ty), seg.Name, GetGetorName(ty), symbols[fullname]);
        }

        //小数字段
        private static string GetSegmentGettor(FrameSegmentReal seg, string fullname, Dictionary<string, ushort> symbols)
        {
            //public bool? SegmentA { get => _gettor.GetBool(1); }
            var ty = seg.IsDouble ? syspropertytype.SYSPT_DOUBLE : syspropertytype.SYSPT_FLOAT;
            if (seg.Repeated.IsIntOne())
                return string.Format("public {0}? {1} {{ get => _gettor.{2}({3}); }}", GetTypeName(ty), seg.Name, GetGetorName(ty), symbols[fullname]);
            else
                return string.Format("public {0}?[] {1} {{ get {{ if (_{1} == null) _{1} = _gettor.{2}Array({3}); return _{1}; }} }} private {0}?[] _{1};", GetTypeName(ty), seg.Name, GetGetorName(ty), symbols[fullname]);
        }

        private static syspropertytype GetSegmentType(int bitcount, bool isSigned)
        {
            if (bitcount == 1)
                return syspropertytype.SYSPT_BOOL;
            else if (bitcount > 1 && bitcount <= 8)
                return isSigned ? syspropertytype.SYSPT_SBYTE : syspropertytype.SYSPT_BYTE;
            else if (bitcount > 8 && bitcount <= 16)
                return isSigned ? syspropertytype.SYSPT_SHORT : syspropertytype.SYSPT_USHORT;
            else if (bitcount > 16 && bitcount <= 32)
                return isSigned ? syspropertytype.SYSPT_INT : syspropertytype.SYSPT_UINT;
            else if (bitcount > 32 && bitcount <= 64)
                return isSigned ? syspropertytype.SYSPT_LONG : syspropertytype.SYSPT_ULONG;
            return 0;
        }

 
        #endregion

        #region --Settor--

        //生成数据帧Settor代码
        private static string GetFrameSettorCode(Frame frm, Dictionary<string, ushort> symbols)
        {
            var code = new StringBuilder(GetTemplate("TSegmentSettor"));
            ReplaceText(code, "framename", frm.Name);
            ReplaceText(code, "frameid", symbols[frm.Name].ToString());

            var prolist = new List<string>();
            foreach (var seg in frm.Segments)
            {
                GetSegmentSettor(seg, frm.Name + "." + seg.Name, symbols, prolist, frm.Name);
            }

            ReplaceText(code, "propertysetlist", prolist, 2);
            return code.ToString();
        }

        private static void GetSegmentSettor(FrameSegmentBase seg, string fullname, Dictionary<string, ushort> symbols, List<string> segcodelist, string framename)
        {
            var ty = seg.GetType();
            if (ty == typeof(FrameSegmentInteger))
                segcodelist.Add(GetSegmentSettor((FrameSegmentInteger)seg, fullname, symbols));
            else if (ty == typeof(FrameSegmentReal))
                segcodelist.Add(GetSegmentSettor((FrameSegmentReal)seg, fullname, symbols));
            else if (ty == typeof(FrameSegmentBlock))
            {
                var bseg = (FrameSegmentBlock)seg;
                switch (bseg.UsedType)
                {
                    case BlockSegType.RefFrame:
                        segcodelist.Add(GetSegmentSettor(bseg, fullname, bseg.RefFrameName, symbols));
                        break;
                    case BlockSegType.DefFrame:
                        GetSegmentSettor(bseg, symbols, segcodelist, framename);
                        break;
                    case BlockSegType.OneOf:
                        GetSegmentSettor(bseg, fullname, symbols, segcodelist);
                        break;
                    case BlockSegType.None:
                        break;
                    default:
                        break;
                }
            }
        }

        //switch字段
        private static void GetSegmentSettor(FrameSegmentBlock seg,string fullname, Dictionary<string, ushort> symbols, List<string> segcodelist)
        {
            segcodelist.Add(string.Format("public {0}Settor {1} {{ get {{ if (_{1} == null) _{1} = new {0}Settor(_settor); return _{1}; }} }}private {0}Settor _{1};", seg.Name, seg.Name));
            var code = new StringBuilder(GetTemplate("TSwitchClassSettor"));
            ReplaceText(code, "segmentname", seg.Name);
            var itemlist = new List<string>();
            foreach (var map in seg.OneOfCaseList)
            {
                itemlist.Add(string.Format("public {0}Settor {1} {{ get {{ if (_{1} == null) _{1} = new {0}Settor(_settor.GetSubFrame({2})); return _{1}; }} }} private {0}Settor _{1};", map.FrameName, map.EnumItem, symbols[fullname + "." + map.EnumItem]));
            }
            ReplaceText(code, "caseitemlist", itemlist, 1);
            var codelist = code.ToString().Split(Environment.NewLine.ToCharArray());
            foreach (var str in codelist)
                if (str != "") segcodelist.Add(str);
        }

        //内联数据帧字段
        private static void GetSegmentSettor(FrameSegmentBlock seg, Dictionary<string, ushort> symbols, List<string> segcodelist, string framename)
        {
            segcodelist.Add(string.Format("public {0}Settor {0} {{ get {{ if (_{0} == null) _{0} = new {0}Settor(_settor); return _{0}; }} }} private {0}Settor _{0};", seg.Name));
            var code = new StringBuilder(GetTemplate("TInnerClassSettor"));
            ReplaceText(code, "segmentname", seg.Name);
            var segs = new List<string>();
            foreach (var innerseg in seg.DefineSegments)
                GetSegmentSettor(innerseg, framename + "." + seg.Name + "." + innerseg.Name, symbols, segs, framename);
            ReplaceText(code, "innersegmentlist", segs, 1);
            var codelist = code.ToString().Split(Environment.NewLine.ToCharArray());
            foreach (var str in codelist)
                if(str!="") segcodelist.Add(str);
        }

        //引用数据帧字段
        private static string GetSegmentSettor(FrameSegmentBlock seg, string fullname, string refframename, Dictionary<string, ushort> symbols)
        {
            return string.Format("public {0}Settor {1} {{ get {{ if (_{1} == null) _{1} = new {0}Settor(_settor.GetSubFrame({2})); return _{1}; }}}} private {0}Settor _{1};", refframename, seg.Name, symbols[fullname]);
        }

        //整数字段
        private static string GetSegmentSettor(FrameSegmentInteger seg, string fullname, Dictionary<string, ushort> symbols)
        {
            //public bool? SegmentA { set => _settor.SetSegmentValue(2, value); }
            var ty = GetSegmentType(seg.BitCount, seg.Signed);
            if (seg.Repeated.IsIntOne())
                return string.Format("public {0}? {1} {{ set => _settor.SetSegmentValue({2}, value); }}", GetTypeName(ty), seg.Name, symbols[fullname]);
            else
                return string.Format("public {0}?[] {1} {{ set => _settor.SetSegmentValue({2}, value); }}", GetTypeName(ty), seg.Name, symbols[fullname]);
        }

        //小数字段
        private static string GetSegmentSettor(FrameSegmentReal seg, string fullname, Dictionary<string, ushort> symbols)
        {
            //public bool? SegmentA { get => _gettor.GetBool(1); }
            var ty = seg.IsDouble ? syspropertytype.SYSPT_DOUBLE : syspropertytype.SYSPT_FLOAT;
            if (seg.Repeated.IsIntOne())
                return string.Format("public {0}? {1} {{ set => _settor.SetSegmentValue({2}, value); }}", GetTypeName(ty), seg.Name, symbols[fullname]);
            else
                return string.Format("public {0}?[] {1} {{ set => _settor.SetSegmentValue({2}, value); }}", GetTypeName(ty), seg.Name, symbols[fullname]);
        }

        #endregion

        #endregion

        #region --Enum--

        //生成枚举文件
        static private void GenerateEnumFile(ICollection<Enumdef> emlist)
        {
            foreach(var emdef in emlist)
            {
                var code = GetSharpCode(emdef);
                CreateFile(emdef.Name, code);
            }
        }

        //生成枚举文件内容
        static private StringBuilder GetSharpCode(Enumdef em)
        {
            var code = new StringBuilder(GetTemplate("TEnum"));
            ReplaceText(code, "project", _pj.Name);
            ReplaceText(code, "enumname", em.Name);
            var il = new List<string>();
            foreach(var it in em.ItemsList)
            {
                if (it.ItemValue != "")
                    il.Add(string.Format("{0} = {1},", it.Name, it.ItemValue));
                else
                    il.Add(string.Format("{0},", it.Name));
            }
            if(il.Count>0)
            {
                il[il.Count - 1] = il.Last().TrimEnd(',');
            }
            ReplaceText(code, "enumlist", il, 2);
            return code;
        }

        #endregion

        #region --Subsys--


        //输出系统文件
        static private void GenerateSysFile(ICollection<Subsys> syslist)
        {
           foreach(var sys in syslist)
           {
                var code = GetSharpCode(sys);
                CreateFile(sys.Name, code);
           }
        }

        //生成分系统代码
        static private StringBuilder GetSharpCode(Subsys sys)
        {
            var code = new StringBuilder(GetTemplate("TSystem"));
            ReplaceText(code, "project", _pj.Name);
            ReplaceText(code, "system", sys.Name);

            //channel
            var decl = new List<string>();
            var initcode = new StringBuilder();
            foreach(var ch in sys.Channels)
            {
                decl.Add(string.Format("public IChannelBase {0};", ch.Name, sys.Name));
                initcode.Append(GetChannelInitial(ch) + Environment.NewLine);
            }
            ReplaceText(code, "channeldeclare", decl, 2);
            ReplaceText(code, "channelinitial", initcode.ToString());

            //property
            SetPropertyDeclare(sys, code);

            //action
            var actionsendcode = new StringBuilder();
            var actionrecvcode = new StringBuilder();
            foreach (var ac in sys.Actions)
            {
                switch (ac.IOType)
                {
                    case actioniotype.AIO_SEND:
                        actionsendcode.Append(GetSendActionCode(sys, ac));
                        break;
                    case actioniotype.AIO_RECV:
                        actionrecvcode.Append(GetRecvActionCode(sys, ac));
                        break;
                    //case actioniotype.AIO_RECVLOOP:
                        //HACK recvloop
                        break;
                    default:
                        break;
                }
            }
            ReplaceText(code, "sendactionlist", actionsendcode.ToString());
            ReplaceText(code, "recvactionlist", actionrecvcode.ToString());
            return code;
        }

        //通道初始化代码
        static private string GetChannelInitial(SubsysChannel ch)
        {
            var code = new StringBuilder(GetTemplate("TChannelInitial"));
            ReplaceText(code, "channelname", ch.Name);
            ReplaceText(code, "channeltype", GetChannelType(ch.ChannelType));
            var oplist = new List<string>();
            foreach (var op in ch.Options)
                oplist.Add(string.Format("if (!ops.Contains(\"{0}\")) ops.SetOption(\"{0}\", {1});",op.Name, op.OptionValue ));
            ReplaceText(code, "channeloptionlist", oplist, 3);
            return code.ToString();
        }


        //设置属性声明代码
        static private void SetPropertyDeclare(Subsys sys, StringBuilder code)
        {
            var decl = new List<string>();
            foreach(var pro in sys.Propertys)
            {
                if(pro.IsArray)
                {
                    decl.Add(string.Format("public ObservableCollection<Parameter<{0}?>> {1} {{ get; private set; }} = new ObservableCollection<Parameter<{0}?>>();", GetTypeName(pro.PropertyType), pro.Name));
                }
                else
                {
                    decl.Add(string.Format("public Parameter<{0}?> {1} {{ get; private set;}} = new Parameter<{0}?>();", GetTypeName(pro.PropertyType), pro.Name));
                }
            }
            ReplaceText(code, "propertydeclare", decl, 2);
        }


        //获取recvloopaction代码
        static private string GetRecvLoopActionCode(Subsys sys,SubsysAction ac)
        {
            //var code = new StringBuilder(GetTemplate("TRecvLoopAction"));
            //ReplaceText(code, "recvloopname", ac.Name);
            //ReplaceText(code, "framename", ac.FrameName);
            //ReplaceText(code, "channelname", ac.ChannelName);
            //var getlist = new List<string>();
            //foreach (var setor in ac.Maps)
            //{
            //    if (ProIsArray(sys, setor.SysPropertyName))
            //    {
            //        getlist.Add(string.Format("{0}.Clear();", setor.SysPropertyName));
            //        getlist.Add(string.Format("var __{0} = data.{1}Array(\"{2}\");", setor.SysPropertyName, GetGetorName(sys, setor.SysPropertyName), setor.FrameSegName));
            //        getlist.Add(string.Format("if (__{0} != null) foreach (var v in __{0}) {0}.Add(new Parameter<{1}?>(v));", setor.SysPropertyName, GetTypeName(GetProType(sys, setor.SysPropertyName))));
            //    }
            //    else
            //        getlist.Add(string.Format("{0}.Value = data.{1}(\"{2}\");", setor.SysPropertyName, GetGetorName(sys, setor.SysPropertyName), setor.FrameSegName));

            //}
            //ReplaceText(code, "getvaluelist", getlist, 4);
            //return code.ToString();
            return "";
        }

        //获取sendaction代码
        static private string GetSendActionCode(Subsys sys,SubsysAction ac)
        {
            var code = new StringBuilder(GetTemplate("TSendAction"));
            ReplaceText(code, "sendaction", ac.Name);
            ReplaceText(code, "framename", ac.FrameName);
            ReplaceText(code, "channelname", ac.ChannelName);
            var setlist = new List<string>();
            foreach(var gettor in ac.Maps)
            {
                if (gettor.FrameSegName == "")
                    setlist.Add(gettor.SysPropertyName.TrimStart('@').TrimEnd(Environment.NewLine.ToCharArray()));
                else if(ProIsArray(sys, gettor.SysPropertyName))
                    setlist.Add(string.Format("data.{0} = {1}.Select(p => p.Value).ToArray();", gettor.FrameSegName, gettor.SysPropertyName));
                else
                    setlist.Add(string.Format("data.{0} = {1}.Value;", gettor.FrameSegName, gettor.SysPropertyName));
            }
            ReplaceText(code, "setvaluelist", setlist, 4);
            return code.ToString();
        }

        //获取recvaction代码
        static private string GetRecvActionCode(Subsys sys,SubsysAction ac)
        {
            var code = new StringBuilder(GetTemplate("TRecvAction"));
            ReplaceText(code, "recvaction", ac.Name);
            ReplaceText(code, "framename", ac.FrameName);
            ReplaceText(code, "channelname", ac.ChannelName);
            var getlist = new List<string>();
            foreach(var setor in ac.Maps)
            {
                if (setor.FrameSegName == "")
                    getlist.Add(setor.SysPropertyName.TrimStart('@').TrimEnd(Environment.NewLine.ToCharArray()));
                else if (ProIsArray(sys, setor.SysPropertyName))
                {
                    getlist.Add(string.Format("{0}.Clear();",setor.SysPropertyName));
                    getlist.Add(string.Format("for (int i = 0; i < data.{0}.Length; i++) {1}.Add(new Parameter<{2}?>(data.{0}[i]));", setor.FrameSegName, setor.SysPropertyName, GetTypeName(GetProType(sys,setor.SysPropertyName))));
                }
                else
                    getlist.Add(string.Format("{0}.Value = data.{1}; ", setor.SysPropertyName,  setor.FrameSegName));
                
            }
            ReplaceText(code, "getvaluelist", getlist, 4);
            return code.ToString();
        }

        #endregion

        #region --Helper--

        //取通道类型
        private static string GetChannelType(syschanneltype chtype)
        {
            switch (chtype)
            {
                case syschanneltype.SCHT_COM:
                    return "ChannelTypeEnum.COM";
                case syschanneltype.SCHT_CAN:
                    return "ChannelTypeEnum.CAN";
                case syschanneltype.SCHT_TCPSERVER:
                    return "ChannelTypeEnum.TCPSERVER";
                case syschanneltype.SCHT_TCPCLIENT:
                    return "ChannelTypeEnum.TCPCLIENT";
                case syschanneltype.SCHT_UDP:
                    return "ChannelTypeEnum.UDP";
                case syschanneltype.SCHT_DIO:
                    return "ChannelTypeEnum.DIO";
            }
            return "";
        }

        //切割内存字符串
        private static IList<string> ToStringList(byte[] data)
        {
            var ret = new List<string>();
            var str = Convert.ToBase64String(CompressBytes(data));
            for (int i = 0; i < str.Length; i += 60)
            {
                if ((i + 60) >= str.Length)
                {
                    ret.Add("\"" + str.Substring(i) + "\"");
                    break;
                }
                else
                {
                    ret.Add("\"" + str.Substring(i, 60) + "\",");
                }
            }
            return ret;
        }

        //压缩内存
        public static byte[] CompressBytes(byte[] bytes)
        {
            using (MemoryStream compressStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(compressStream, CompressionMode.Compress))
                {
                    zipStream.Write(bytes, 0, bytes.Length);
                    zipStream.Close();
                    return compressStream.ToArray();
                }
            }
        }

        //属性是否为数组
        static private bool ProIsArray(Subsys sys, string proname)
        {
            foreach (var p in sys.Propertys)
            {
                if (p.Name == proname)
                    return p.IsArray;
            }
            return false;
        }

        static private string GetGetorName(Subsys sys, string proname)
        {
            foreach(var p in sys.Propertys)
            {
                if (p.Name == proname)
                    return GetGetorName(p.PropertyType);
            }
            return "Get_";
        }

        static private syspropertytype GetProType(Subsys sys, string proname)
        {
            foreach(var p in sys.Propertys)
            {
                if (p.Name == proname)
                    return p.PropertyType;
            }
            return 0;
        }

        //取值函数名称
        static private string GetGetorName(syspropertytype ty)
        {
            switch (ty)
            {
                case syspropertytype.SYSPT_BOOL:
                    return "GetBool";
                case syspropertytype.SYSPT_BYTE:
                    return "GetByte";
                case syspropertytype.SYSPT_SBYTE:
                    return "GetSByte";
                case syspropertytype.SYSPT_SHORT:
                    return "GetShort";
                case syspropertytype.SYSPT_USHORT:
                    return "GetUShort";
                case syspropertytype.SYSPT_INT:
                    return "GetInt";
                case syspropertytype.SYSPT_UINT:
                    return "GetUInt";
                case syspropertytype.SYSPT_LONG:
                    return "GetLong";
                case syspropertytype.SYSPT_ULONG:
                    return "GetULong";
                case syspropertytype.SYSPT_FLOAT:
                    return "GetFloat";
                case syspropertytype.SYSPT_DOUBLE:
                    return "GetDouble";
            }
            Debug.Assert(false);
            return "";
        }

        //类型名称
        static public string GetTypeName(syspropertytype ty)
        {
            switch(ty)
            {
                case syspropertytype.SYSPT_BOOL:
                    return "bool";
                case syspropertytype.SYSPT_BYTE:
                    return "byte";
                case syspropertytype.SYSPT_SBYTE:
                    return "sbyte";
                case syspropertytype.SYSPT_SHORT:
                    return "short";
                case syspropertytype.SYSPT_USHORT:
                    return "ushort";
                case syspropertytype.SYSPT_INT:
                    return "int";
                case syspropertytype.SYSPT_UINT:
                    return "uint";
                case syspropertytype.SYSPT_LONG:
                    return "long";
                case syspropertytype.SYSPT_ULONG:
                    return "ulong";
                case syspropertytype.SYSPT_FLOAT:
                    return "float";
                case syspropertytype.SYSPT_DOUBLE:
                    return "double";
            }
            Debug.Assert(false);
            return "";
        }

        //生成代码文件
        static private void CreateFile(string fname, StringBuilder content)
        {
            var fn = _newpath + "\\" + fname + ".cs";
            var match = Regex.Match(content.ToString(), "<%.+%>");
            while(match.Success)
            {
                content.Replace(match.Value, "");
                match = match.NextMatch();
            }
            content.Replace("\t", "    ");
            File.WriteAllText(fn, content.ToString());
            _tout.OutText(string.Format("信息：生成文件{0}", fn), false);
        }

        //取代码模板
        static private string GetTemplate(string tname)
        {
            return File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Template\\" + tname + ".cst");
        }

        //替换标识符
        static private void ReplaceText(StringBuilder code, string template_id, string new_id)
        {
            code.Replace("<%" + template_id + "%>", new_id);
        }

        static private void ReplaceText(StringBuilder code, string template_id, IList<string> new_list, int tab_count)
        {
            if (new_list.Count == 0) return;
            var pre = new string('\t', tab_count);
            var str = new StringBuilder(new_list[0]);
            
            for(int i=1; i<new_list.Count; i++)
            {
                str.Append(Environment.NewLine + pre + new_list[i]);
            }
            ReplaceText(code, template_id, str.ToString());
        }


        //准备输出目录
        static private void PrepareDir()
        {
            if(!Path.HasExtension(_newpath))
            {
                Directory.CreateDirectory(_newpath);
            }
            else
            {
                ClearDir(_newpath);
                Directory.CreateDirectory(_newpath);
            }
        }

        //删除目录下的cs和bin文件
        static private void ClearDir(string dirpath)
        {
            DirectoryInfo dir = new DirectoryInfo(dirpath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is FileInfo) 
                {
                    if (i.Extension == "cs" || i.Extension == "bin")
                        File.Delete(i.FullName);
                }
            }
        }

        #endregion
    }
}
