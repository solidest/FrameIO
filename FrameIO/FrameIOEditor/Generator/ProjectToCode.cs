using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main.Generator
{
    public class ProjectToCode
    {

        public static string GetProjectCode(IOProject pj)
        {
            var code = new StringBuilder();

            AppendNotes(code, pj.Notes, 0);
            code.Append(string.Format("project {0} {{"+ Environment.NewLine, pj.Name));

            AppendSubsysCode(code, pj);
            AppendFrameCode(code, pj);
            AppendEnumdef(code, pj);

            code.Append("}}"+Environment.NewLine);
            return code.ToString();
        }

        //枚举
        private static void AppendEnumdef(StringBuilder code, IOProject pj)
        {

            foreach(var em in pj.EnumdefList)
            {
                AppendNotes(code, em.Notes, 1);
                code.Append(string.Format("\tenum {0} {{" + Environment.NewLine, em.Name));
                int len = code.Length;
                foreach(var emi in em.ItemsList)
                {
                    AppendNotes(code, emi.Notes, 2);

                    len = code.Length;
                    if (emi.ItemValue == null || emi.ItemValue == "")
                        code.Append(string.Format("\t\t{0}," + Environment.NewLine, emi.Name));
                    else
                        code.Append(string.Format("\t\t{0} = {1}," + Environment.NewLine, emi.Name, emi.ItemValue));
                }
                code.Replace(",", "", len, 1);
                code.Append("\t}}" + Environment.NewLine);
            }
        }

        //数据帧
        private static void AppendFrameCode(StringBuilder code, IOProject pj)
        {
            foreach(var frm in pj.FrameList)
            {
                AppendNotes(code, frm.Notes, 1);
                code.Append(string.Format("\tframe {0} {{" + Environment.NewLine, frm.Name));
                foreach(var seg in frm.Segments)
                {
                    AppendNotes(code, seg.Notes, 2);
                    code.Append("\t\t");
                    seg.AppendSegmentCode(code);
                }
                code.Append("\t}}" + Environment.NewLine);
            }
        }

        //分系统
        private static void AppendSubsysCode(StringBuilder code, IOProject pj)
        {
            foreach(var sys in pj.SubsysList)
            {
                AppendNotes(code, sys.Notes, 1);
                code.Append(string.Format("\tsystem {0} {{" + Environment.NewLine, sys.Name));

                //属性
                foreach(var pro in sys.Propertys)
                {
                    AppendNotes(code, pro.Notes, 2);
                    code.AppendFormat("\t\t{0} {1} {2};"+Environment.NewLine, FrameIOCodeGenerator.GetTypeName(pro.PropertyType), pro.Name, pro.IsArray ? "[]" : "");
                }

                //通道
                foreach (var ch in sys.Channels)
                {
                    AppendNotes(code, ch.Notes, 2);
                    code.Append(string.Format("\t\tchannel {0} : {1} {{" + Environment.NewLine, ch.Name, GetChannelTypeName(ch.ChannelType)));
                    foreach(var op in ch.Options)
                    {
                        AppendNotes(code, op.Notes, 3);
                        code.Append(string.Format("\t\t\t{0} = {1};" + Environment.NewLine, op.Name, op.OptionValue));
                    }
                    code.Append("\t\t}}" + Environment.NewLine);
                }

                //动作
                foreach (var ac in sys.Actions)
                {
                    AppendNotes(code, ac.Notes, 2);
                    code.AppendFormat("\t\taction {0} : {1} {2} on {3} {{" + Environment.NewLine, ac.Name, GetActionTypeName(ac.IOType), ac.FrameName, ac.ChannelName);

                    foreach (var map in ac.BeginCodes)
                        code.Append("\t\t\t" + map);

                    var nms = Helper.GetFrameSegmentsName(ac.FrameName, pj.FrameList);
                    if(nms.Count==0)
                    {
                        foreach (var mp in ac.LiteMaps)
                            code.AppendFormat("\t\t\t{0} : {1};" + Environment.NewLine, mp.FrameSegName, mp.SysPropertyName);
                    }
                    else
                    {

                    }

                    foreach (var map in ac.EndCodes)
                        code.Append("\t\t\t" + map);

                    code.Append("\t\t}}" + Environment.NewLine);
                }

                code.Append("\t}}" + Environment.NewLine);
            }
        }

        //添加注释
        private static void AppendNotes(StringBuilder code, string notes, int tabcount)
        {
            if (notes == null || notes.Length == 0) return;
            var pre = tabcount ==0? "": new string('\t', tabcount);
            var cs = notes.Split(Environment.NewLine.ToArray());
            foreach (var str in cs)
            {
                if (str.Trim() != "")
                {
                    code.Append(pre);
                    code.Append("//");
                    code.Append(str);
                    code.Append(Environment.NewLine);
                }
            }
        }

        //通道类型名称
        private static string GetChannelTypeName(syschanneltype ty)
        {
            switch (ty)
            {
                case syschanneltype.SCHT_COM:
                    return "com";
                case syschanneltype.SCHT_CAN:
                    return "can";
                case syschanneltype.SCHT_TCPSERVER:
                    return "tcpserver";
                case syschanneltype.SCHT_TCPCLIENT:
                    return "tcpclient";
                case syschanneltype.SCHT_UDP:
                    return "upd";
                case syschanneltype.SCHT_DIO:
                    return "dio";
            }
            return "";
        }

        //动作类型名称
        private static string GetActionTypeName(actioniotype ty)
        {
            switch (ty)
            {
                case actioniotype.AIO_SEND:
                    return "send";
                case actioniotype.AIO_RECV:
                    return "recv";
                case actioniotype.AIO_RECVLOOP:
                    return "recvloop";
            }
            return "";
        }
    }
}
