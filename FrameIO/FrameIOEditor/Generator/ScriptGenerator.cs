﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

        #region --Initial--

        protected const string TPROJECT = "project";

        //代码类型标识
        protected abstract string Token { get; }


        //默认扩展名
        protected abstract string DefaultExtension { get; }

        //创建专有的共享类库文件
        protected abstract void CreateSharedFile();


        public void GenerateScriptFile()
        {
            try
            {
                var pjnames = _pj.Name.Split('.');
                _path = _out.GetMainOutPath() + "\\" + pjnames[pjnames.Length - 1] + "_" + Token;

                //准备目录
                PrepareDir();

                //生成专有的共享文件
                CreateSharedFile();

                //生成数据帧文件
                CreateFramsFile();

                //生成枚举
                foreach (var emdef in _pj.EnumdefList) CreateEnumFile(emdef);

                //生成子系统文件
                foreach (var inner in _pj.InnerSubsysList) CreateInnerSubsys(inner);

                //生成分系统文件
                foreach (var subsys in _pj.SubsysList)
                {
                    OutFile(subsys.Name, GetSubsysFileContent(subsys));
                }

                _out.OutText("信息：代码文件输出完成", false);

            }
            catch (Exception e)
            {
                _out.OutText(e.ToString(), true);
            }
        }



        #endregion

        #region --数据帧--


        //数据帧文件内容转换
        protected abstract IList<string> ConvertFramesCode(IList<string> base64List);

        //数据帧文件内容转换
        private void CreateFramsFile()
        {
            var frms = _jframes.GetJsonString();
            var cont = CompressBytes(Encoding.Default.GetBytes(frms));
            var b64list = ToBase64List(cont);
            
            OutFile("TFrames", "Frames", "framesconfig", ConvertFramesCode(b64list), TPROJECT, _pj.Name);
        }

        #endregion

        #region --枚举--

        //生成枚举文件代码内容
        protected virtual void CreateEnumFile(Enumdef em)
        {
            var code = em.ItemsList.Select(p => p.Name + (p.ItemValue.Length == 0 ? "," : (" = " + p.ItemValue + ","))).ToList();
            OutFile("TEnum", em.Name, "enumlist", code,
                TPROJECT, _pj.Name,
                "enumname", em.Name);
        }

        #endregion

        #region --子系统--

        //子系统代码文件内容
        protected abstract StringBuilder GetInnerSubsysFileContent(InnerSubsys inner);

        //找出全部需要被创建的子系统
        protected void CreateInnerSubsys(InnerSubsys inner)
        {
            OutFile(inner.Name, GetInnerSubsysFileContent(inner));
        }

        #endregion

        #region --分系统--

        //分系统相关
        protected abstract string SystemTemplate { get; }
        protected abstract string GetPropertyDefCode(SubsysProperty pro);
        protected abstract string GetPropertyIniCode(SubsysProperty pro);
        protected abstract string ExceptionHandlerTemplate { get; }

        //创建分系统类文件
        private StringBuilder GetSubsysFileContent(Subsys subsys)
        {
            var prodec = new List<string>();
            foreach (var item in subsys.Propertys)
            {
                prodec.Add(GetPropertyDefCode(item));
            }

            var proini = new List<string>();
            foreach (var item in subsys.Propertys)
            {
                proini.Add(GetPropertyIniCode(item));
            }

            return GetTemplateBuilder(SystemTemplate, "propertydeclare", prodec,
                "project", _pj.Name,
                "system", subsys.Name,
                "propertyinitial", List2String(proini, 3),
                "channeldeclare", GetChannelsDeclare(subsys, 2),
                "channelinitial", GetChannelsInitial(subsys, 2),
                "exceptionhandler", GetExceptionhandler(2),
                "sendactionlist", GetSendActions(subsys.Actions.Where(p=>p.IOType== actioniotype.AIO_SEND), 2),
                "recvactionlist", GetRecvActions(subsys.Actions.Where(p => p.IOType == actioniotype.AIO_RECV), 2)
                );
        }

        //异常处理函数
        private String GetExceptionhandler(int tabCount)
        {
            if (ExceptionHandlerTemplate == null) return "";
            var exh = GetTemplate(ExceptionHandlerTemplate);
            var exhs = exh.Split(Environment.NewLine.ToCharArray()).Where(p=>p!="").ToList();
            return List2String(exhs, tabCount);
        }

        #endregion

        #region --通道--

        protected abstract string GetChannelDeclare(SubsysChannel ch);
        protected abstract IList<string> GetChannelInitialFun(SubsysChannel ch);

        //通道声明
        private string GetChannelsDeclare(Subsys subsys, int tabCount)
        {
            var chs = new List<string>();
            foreach(var ch in subsys.Channels)
            {
                chs.Add(GetChannelDeclare(ch));
            }
            return List2String(chs, tabCount);
        }

        //通道初始化函数
        private string GetChannelsInitial(Subsys subsys, int tabCount)
        {
            var chs = new List<string>();
            foreach (var ch in subsys.Channels)
            {
                var fun = GetChannelInitialFun(ch);
                if (chs.Count > 0 && fun.Count > 0) chs.Add(Environment.NewLine);
                chs.AddRange(fun);
            }
            return List2String(chs, tabCount);
        }


        #endregion


        #region --发送数据--

        protected abstract IList<string> GetSendFun(SubsysAction ac);

        private string GetSendActions(IEnumerable<SubsysAction> acs, int tabCount)
        {
            if (acs == null) return "";
            var chs = new List<string>();
            foreach (var ac in acs)
            {
                var fun = GetSendFun(ac);
                if (chs.Count > 0 && fun.Count > 0) chs.Add(Environment.NewLine);
                chs.AddRange(fun);
            }
            return List2String(chs, tabCount);
        }


        #endregion

        #region --接收数据--

        protected abstract IList<string> GetRecvFun(SubsysAction ac);

        private string GetRecvActions(IEnumerable<SubsysAction> acs, int tabCount)
        {
            if (acs == null) return "";
            var chs = new List<string>();
            foreach (var ac in acs)
            {
                var fun = GetRecvFun(ac);
                if (chs.Count > 0 && fun.Count > 0) chs.Add(Environment.NewLine);
                chs.AddRange(fun);
            }
            return List2String(chs, tabCount);
        }

        #endregion

        #region --Helper for file--

        //插入前置空白
        private IList<string> InsertTabs(IList<string> ls, int tabCount)
        {
            var pre = new string('\t', tabCount);
            return ls.Select(p => pre + p).ToList();
        }

        //输出文件
        protected void OutFile(string templageName, string fileName, string token, IList<string> codelist, params string[] others)
        {
            OutFile(fileName, GetTemplateBuilder(templageName, token, codelist, others));
        }


        //输出文件
        private void OutFile(string fname, StringBuilder content)
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

        //取代码模板字符缓冲 并填充内容
        protected StringBuilder GetTemplateBuilder(string templageName, string token, IList<string> codelist, params string[] others)
        {
            var code = GetTemplateBuilder(templageName);
            ReplaceText(code, token, codelist);

            for (int i = 0; i < others.Length; i += 2)
            {
                ReplaceText(code, others[i], others[i + 1]);
            }
            return code;
        }

        //拼接代码
        protected string List2String(IList<string> codelist, int tabCount)
        {
            var pre = new string('\t', tabCount);
            var str = new StringBuilder();

            for (int i = 0; i < codelist.Count; i++)
            {
                str.Append((i == 0 ? "" : pre) + codelist[i] + (i == codelist.Count - 1 ? "" : Environment.NewLine));
            }
            return str.ToString();

        }

        //取代码模板字符缓冲
        protected StringBuilder GetTemplateBuilder(string tname)
        {
            return new StringBuilder(GetTemplate(tname));
        }

        //取代码模板
        protected string GetTemplate(string tname)
        {
            return File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Template_" + Token + "\\" + tname + ".tpl");
        }

        //替换标识符字符串
        protected void ReplaceText(StringBuilder code, string template_id, string new_id)
        {
            code.Replace("<%" + template_id + "%>", new_id);
        }


        //替换标识未字符串列表
        protected void ReplaceText(StringBuilder code, string template_id, IList<string> new_list)
        {
            if (new_list.Count == 0) return;
            var pre = new string(' ', GetEmptyBefore(code, template_id));
            var str = new StringBuilder();

            for (int i = 0; i < new_list.Count; i++)
            {
                str.Append((i==0?"":pre) + new_list[i] + (i == new_list.Count-1 ? "" : Environment.NewLine));
            }
            var script = str.ToString();
            ReplaceText(code, template_id, script.EndsWith(",")? script.Substring(0, script.Length-1) : script);
        }

        //去掉最末位的字符
        protected IList<string> RemoveLastChart(IList<string> list)
        {
            list[list.Count - 1] = list[list.Count - 1].Substring(0, list.Count - 1);
            return list;
        }


        //查找标识前面的空格数量
        protected int GetEmptyBefore(string script, string template_id)
        {
            var match = Regex.Match(script, "<%" + template_id + "%>");
            while (match.Success)
            {
                int pos = match.Index;
                int ret = 0;
                while(--pos>0)
                {
                    if (script[pos] == '\t')
                        ret += 4;
                    else if (script[pos] == ' ')
                        ret += 1;
                    else
                        return ret;
                }
            }
            throw new Exception("unknow");
        }
        protected int GetEmptyBefore(StringBuilder code, string template_id)
        {
            return GetEmptyBefore(code.ToString(), template_id);       

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

        private string[] extlist = new string[4] { "cs", "cpp", "h", "hpp" };

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

        #endregion

        #region --Helper for other--

        //转换字符串为Base64数组
        protected static IList<string> ToBase64List(byte[] data)
        {
            var ret = new List<string>();
            var str = Convert.ToBase64String(data);
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

        #endregion

        #region --Helper for script--

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



        //属性是否为数组
        protected bool ProIsArray(Subsys sys, string proname)
        {
            return sys.Propertys.Where(p => (p.Name == proname && p.IsArray)).Count() > 0;
        }

        

        #endregion

    }


}
