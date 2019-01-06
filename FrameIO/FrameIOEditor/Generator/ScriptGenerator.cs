using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FrameIO.Main
{

    //代码生成器
    public abstract class ScriptGenerator
    {
        protected IOProject _pj;
        protected IOutText _out;
        protected string _path;
        private Frames2Json _jframes;
        
        public ScriptGenerator(IOProject pj,  IOutText tout)
        {
            _pj = pj;
            _out = tout;
            _jframes = new Frames2Json(_pj);
        }

        #region --abstract--

        protected abstract string Token { get; }
        protected abstract string FramesFileName { get; }
        protected abstract string DefaultExtension { get; }
        protected abstract void CreateShareFile();
        protected abstract StringBuilder GetFramesFileContent(IList<string> framesjson);
        protected abstract StringBuilder GetEnumFileContent(Enumdef em);
        protected abstract StringBuilder GetInnerSubsysFileContent(InnerSubsys innersys);
        protected abstract StringBuilder GetSubsysFileContent(Subsys subsys);


        #endregion

        public void GenerateScriptFile()
        {
            try
            {
                var pjnames = _pj.Name.Split('.');
                _path = _out.GetMainOutPath() + "\\" + pjnames[pjnames.Length-1] + "_" + Token;

                //准备目录
                PrepareDir();

                //生成专有的共享文件
                CreateShareFile();

                //生成数据帧文件
                CreateFile(FramesFileName, GetFramesFileContent(ToBase64List(_jframes.GetJsonString())));

                //生成枚举
                foreach (var emdef in _pj.EnumdefList)
                {
                    CreateFile(emdef.Name, GetEnumFileContent(emdef));
                }

                //生成子系统文件
                foreach(var isys in _pj.InnerSubsysList)
                {
                    CreateFile(isys.Name, GetInnerSubsysFileContent(isys));
                }

                //生成分系统文件
                foreach(var subsys in _pj.SubsysList)
                {
                    CreateFile(subsys.Name, GetSubsysFileContent(subsys));
                }

                //GenerateSysFile(pj.SubsysList);
                _out.OutText("信息：代码文件输出完成", false);

            }
            catch (Exception e)
            {
                _out.OutText(e.ToString(), true);
            }
        }

        #region --Helper--

        //取通道类型
        protected string ToChannelType(syschanneltype chtype)
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

        //转换字符串为Base64数组
        protected static IList<string> ToBase64List(string data)
        {
            var ret = new List<string>();
            var str = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));
            for (int i = 0; i < str.Length; i += 60)
            {
                if ((i + 60) >= str.Length)
                {
                    ret.Add(str.Substring(i));
                }
                else
                {
                    ret.Add(str.Substring(i, 60));
                }
            }
            return ret;
        }


        //属性是否为数组
        protected bool ProIsArray(Subsys sys, string proname)
        {
            return sys.Propertys.Where(p => (p.Name == proname && p.IsArray)).Count() > 0;
        }


        //取代码模板
        protected string GetTemplate(string tname)
        {
            return File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Template\\" + Token + "\\" + tname + ".tpl");
        }

        //替换标识符
        protected void ReplaceText(StringBuilder code, string template_id, string new_id)
        {
            code.Replace("<%" + template_id + "%>", new_id);
        }

        protected void ReplaceText(StringBuilder code, string template_id, IList<string> new_list, int tab_count)
        {
            if (new_list.Count == 0) return;
            var pre = new string('\t', tab_count);
            var str = new StringBuilder(new_list[0]);

            for (int i = 1; i < new_list.Count; i++)
            {
                str.Append(Environment.NewLine + pre + new_list[i]);
            }
            ReplaceText(code, template_id, str.ToString());
        }


        //生成代码文件
        private void CreateFile(string fname, StringBuilder content)
        {
            var fn = _path + "\\" + (fname.Split('.').Length > 1 ? fname : fname + "." + DefaultExtension);
            var match = Regex.Match(content.ToString(), "<%.+%>");
            while (match.Success)
            {
                content.Replace(match.Value, "");
                match = match.NextMatch();
            }
            content.Replace("\t", "    ");
            File.WriteAllText(fn, content.ToString());
            _out.OutText(string.Format("信息：生成文件{0}", fn), false);
        }

        //准备输出目录
        private void PrepareDir()
        {
            if (!Path.HasExtension(_path))
            {
                Directory.CreateDirectory(_path);
            }
            else
            {
                ClearDir(_path);
                Directory.CreateDirectory(_path);
            }
        }

        private string[] extlist = new string[3] { "cs", "cpp", "h" };

        //删除目录下的文件
        private void ClearDir(string dirpath)
        {
            DirectoryInfo dir = new DirectoryInfo(dirpath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is FileInfo)
                {
                    if (extlist.Contains(i.Extension))
                        File.Delete(i.FullName);
                }
            }
        }


        //static private string GetGetorName(Subsys sys, string proname)
        //{
        //    foreach (var p in sys.Propertys)
        //    {
        //        if (p.Name == proname)
        //            return GetGetorName(p.PropertyType);
        //    }
        //    return "Get_";
        //}

        //static private string GetProType(Subsys sys, string proname)
        //{
        //    if (proname.Contains("."))
        //    {
        //        var names = proname.Split('.');
        //        if (names.Count() != 2) return "";
        //        foreach (var p in sys.Propertys)
        //        {
        //            if (p.Name == names[0])
        //            {
        //                foreach (var ss in _pj.SubsysList)
        //                {
        //                    if (ss.Name == p.PropertyType)
        //                        return GetProType(ss, names[1]);
        //                }
        //            }
        //        }

        //    }

        //    foreach (var p in sys.Propertys)
        //    {
        //        if (p.Name == proname)
        //            return p.PropertyType;
        //    }
        //    return "";
        //}

        ////取值函数名称
        //static private string GetGetorName(string ty)
        //{
        //    switch (ty)
        //    {
        //        case "bool":
        //            return "GetBool";
        //        case "byte":
        //            return "GetByte";
        //        case "sbyte":
        //            return "GetSByte";
        //        case "short":
        //            return "GetShort";
        //        case "ushort":
        //            return "GetUShort";
        //        case "int":
        //            return "GetInt";
        //        case "uint":
        //            return "GetUInt";
        //        case "long":
        //            return "GetLong";
        //        case "ulong":
        //            return "GetULong";
        //        case "float":
        //            return "GetFloat";
        //        case "double":
        //            return "GetDouble";
        //    }
        //    return "";
        //}

        //类型名称
        //static public string GetTypeName(syspropertytype ty)
        //{
        //    switch(ty)
        //    {
        //        case syspropertytype.SYSPT_BOOL:
        //            return "bool";
        //        case syspropertytype.SYSPT_BYTE:
        //            return "byte";
        //        case syspropertytype.SYSPT_SBYTE:
        //            return "sbyte";
        //        case syspropertytype.SYSPT_SHORT:
        //            return "short";
        //        case syspropertytype.SYSPT_USHORT:
        //            return "ushort";
        //        case syspropertytype.SYSPT_INT:
        //            return "int";
        //        case syspropertytype.SYSPT_UINT:
        //            return "uint";
        //        case syspropertytype.SYSPT_LONG:
        //            return "long";
        //        case syspropertytype.SYSPT_ULONG:
        //            return "ulong";
        //        case syspropertytype.SYSPT_FLOAT:
        //            return "float";
        //        case syspropertytype.SYSPT_DOUBLE:
        //            return "double";
        //    }
        //    Debug.Assert(false);
        //    return "";
        //}


        #endregion

    }


}
