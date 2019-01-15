using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
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

            code.Append("}"+Environment.NewLine);
            return code.ToString();
        }

        //枚举
        private static void AppendEnumdef(StringBuilder code, IOProject pj)
        {

            foreach(var em in pj.EnumdefList)
            {
                if (em.Name == null || em.Name.Length == 0) continue;
                AppendNotes(code, em.Notes, 1);
                code.Append(string.Format("\tenum {0} {{" + Environment.NewLine, em.Name));
                int i = -1;
                foreach(var emi in em.ItemsList)
                {
                    i++;
                    if (emi.Name == null || emi.Name.Length == 0) continue;

                    AppendNotes(code, emi.Notes, 2);

                    if (emi.ItemValue == null || emi.ItemValue == "")
                        code.Append(string.Format("\t\t{0}{1}" + Environment.NewLine, emi.Name, i == em.ItemsList.Count - 1 ? "" : ","));
                    else
                    {
                        code.Append(string.Format("\t\t{0} = {1}{2}" + Environment.NewLine, emi.Name, emi.ItemValue, i==em.ItemsList.Count-1?"":","));
                    }
                }
                code.Append("\t}" + Environment.NewLine + Environment.NewLine);
            }
        }

        //数据帧
        private static void AppendFrameCode(StringBuilder code, IOProject pj)
        {
            foreach(var frm in pj.FrameList)
            {
                if (frm.Name == null || frm.Name.Length == 0) continue;

                AppendNotes(code, frm.Notes, 1);
                if(frm.SubSysName!=null && frm.SubSysName.Length>0)
                {
                    code.Append(string.Format("\t[subsys: {0}]" + Environment.NewLine, frm.SubSysName));
                }
                code.Append(string.Format("\tframe {0} {{" + Environment.NewLine, frm.Name));
                foreach(var seg in frm.Segments)
                {
                    if (seg.Name == null || seg.Name.Length == 0) continue;
                    AppendNotes(code, seg.Notes, 2);
                    code.Append("\t\t");
                    seg.AppendSegmentCode(code);
                    code.Append(Environment.NewLine);
                }
                code.Append("\t}" + Environment.NewLine + Environment.NewLine);
            }
        }

        //分系统
        private static void AppendSubsysCode(StringBuilder code, IOProject pj)
        {
            foreach(var sys in pj.SubsysList)
            {
                if (sys.Name == null || sys.Name.Length == 0) continue;

                AppendNotes(code, sys.Notes, 1);
                code.Append(string.Format("\tsystem {0} {{" + Environment.NewLine, sys.Name));

                //属性
                foreach(var pro in sys.Propertys)
                {
                    if (pro.Name == null || pro.Name.Length == 0) continue;
                    AppendNotes(code, pro.Notes, 2);
                    code.AppendFormat("\t\t{0}{1} {2};"+Environment.NewLine, pro.PropertyType, pro.IsArray? ("[" + pro.ArrayLen + "]") : "", pro.Name);
                }

                code.Append(Environment.NewLine);

                //通道
                foreach (var ch in sys.Channels)
                {
                    if (ch.Name == null || ch.Name.Length == 0) continue;

                    AppendNotes(code, ch.Notes, 2);
                    code.Append(string.Format("\t\tchannel {0} : {1} {{" + Environment.NewLine, ch.Name, GetChannelTypeName(ch.ChannelType)));
                    foreach(var op in ch.Options)
                    {
                        if (op.Name == null || op.Name.Length == 0) continue;
                        if (op.OptionValue == null || op.OptionValue.Length == 0) continue;
                        var v = op.OptionValue;
                        if (!Helper.ValidateIsInt(v) && !Helper.ValidateIsReal(v))
                        {
                            v = v.Trim('"');
                            v = string.Format("\"{0}\"", v);
                        }
                        AppendNotes(code, op.Notes, 3);
                        code.Append(string.Format("\t\t\t{0} = {1};" + Environment.NewLine, op.Name, v));
                    }
                    code.Append("\t\t}" + Environment.NewLine);
                }
                code.Append(Environment.NewLine);

                //动作
                foreach (var ac in sys.Actions)
                {
                    if (ac.Name == null || ac.Name.Length == 0) continue;

                    AppendNotes(code, ac.Notes, 2);
                    code.AppendFormat("\t\taction {0} : {1} {2} on {3} {{" + Environment.NewLine, ac.Name, GetActionTypeName(ac.IOType), ac.FrameName.Length>0? ac.FrameName:"_", ac.ChannelName.Length>0 ? ac.ChannelName:"_");

                    //开始位置的用户代码
                    foreach (var map in ac.BeginCodes)
                        code.Append("\t\t\t" + map);


                    //字段映射
                    foreach (var mp in ac.LiteMaps)
                    {
                        if (mp.FrameSegName == null || mp.FrameSegName.Length == 0) continue;
                        if (mp.SysPropertyName == null || mp.SysPropertyName.Length == 0) continue;
                        AppendNotes(code, mp.Notes, 3);
                        code.AppendFormat("\t\t\t{0} : {1};" + Environment.NewLine, mp.FrameSegName, mp.SysPropertyName);
                    }

                    //用户结尾代码
                    foreach (var map in ac.EndCodes)
                        code.Append("\t\t\t" + map);

                    code.Append("\t\t}" + Environment.NewLine);

                }

                code.Append("\t}" + Environment.NewLine + Environment.NewLine);
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
                    return "udp";
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
                //case actioniotype.AIO_RECVLOOP:
                    //return "recvloop";
            }
            return "";
        }
    }
}
