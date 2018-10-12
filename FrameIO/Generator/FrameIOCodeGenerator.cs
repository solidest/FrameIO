using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //代码生成器
    public class FrameIOCodeGenerator
    {
        static private IOutText _tout;
        static private string _newpath = "";
        static private IOProject _pj;

        static public void GenerateCodeFile(IOProject pj, ProjectInfo pji, IOutText tout)
        {
            try
            {
                _tout = tout;
                _pj = pj;
                _newpath = tout.GetMainOutPath() + "\\" + pj.Name;

                PrepareDir();

                var fn = _newpath + "\\FrameIO.bin";
                CodeFile.SaveFrameBinFile(fn, pji);
                tout.OutText(string.Format("信息：生成文件{0}",fn) , true);

                GenerateEnumFile(pj.EnumdefList);
                GenerateSysFile(pj.SubsysList);
                tout.OutText("信息：代码文件输出完成", false);

            }
            catch (Exception e)
            {
                tout.OutText(e.ToString(), true);
            }
        }

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
            var init = new List<string>();
            foreach(var ch in sys.Channels)
            {
                decl.Add(string.Format("private IChannelBase {0};", ch.Name));
                init.Add(string.Format("{0} = FrameIOFactory.GetChannel(\"{1}\", \"{0}\");", ch.Name, sys.Name));
                init.Add(string.Format("{0}.Open();", ch.Name));
            }
            ReplaceText(code, "channeldeclare", decl, 2);
            ReplaceText(code, "channelinit", init, 4);

            SetPropertyDeclare(sys, code);

            foreach(var ac in sys.Actions)
            {
                SetActionCode(sys, ac, code);
            }

            return code;
        }


        //设置属性声明代码
        static private void SetPropertyDeclare(Subsys sys, StringBuilder code)
        {
            var decl = new List<string>();
            foreach(var pro in sys.Propertys)
            {
                if(pro.IsArray)
                {
                    decl.Add(string.Format("public ObservableCollection<Parameter<{0}?>> {1} {{ get; set; }} = new ObservableCollection<Parameter<{0}?>>();", GetPropertyTypeName(pro.PropertyType), pro.Name));
                }
                else
                {
                    decl.Add(string.Format("public Parameter<{0}?> {1} {{ get; set;}} = new Parameter<{0}?>();", GetPropertyTypeName(pro.PropertyType), pro.Name));
                }
            }
            ReplaceText(code, "propertydeclare", decl, 2);
        }

        //设置动作代码
        static private void SetActionCode(Subsys sys, SubsysAction ac, StringBuilder code)
        {
            switch(ac.IOType)
            {
                case actioniotype.AIO_SEND:
                    ReplaceText(code, "sendactionlist", GetSendActionCode(sys, ac));
                    break;

                case actioniotype.AIO_RECV:
                    ReplaceText(code, "recvactionlist", GetRecvActionCode(sys, ac));
                    break;

                case actioniotype.AIO_RECVLOOP:
                    ReplaceText(code, "recvloopactionlist", GetRecvLoopActionCode(sys, ac));
                    break;
            }

        }

        //获取recvloopaction代码
        static private string GetRecvLoopActionCode(Subsys sys,SubsysAction ac)
        {
            var code = new StringBuilder(GetTemplate("TRecvLoopAction"));
            ReplaceText(code, "recvloopname", ac.Name);
            ReplaceText(code, "framename", ac.FrameName);
            ReplaceText(code, "channelname", ac.ChannelName);
            var getlist = new List<string>();
            foreach (var setor in ac.Maps)
            {
                if (ProIsArray(sys, setor.SysPropertyName))
                {
                    getlist.Add(string.Format("{0}.Clear();", setor.SysPropertyName));
                    getlist.Add(string.Format("var __{0} = data.GetByteArray(\"{1}\");", setor.SysPropertyName, setor.FrameSegName));
                    getlist.Add(string.Format("if (__{0} != null) foreach (var v in __{0}) {0}.Add(new Parameter<{1}?>(v));", setor.SysPropertyName, GetPropertyTypeName(GetProType(sys, setor.SysPropertyName))));
                }
                else
                    getlist.Add(string.Format("{0}.Value = data.{1}(\"{2}\");", setor.SysPropertyName, GetGetorName(sys, setor.SysPropertyName), setor.FrameSegName));

            }
            ReplaceText(code, "getvaluelist", getlist, 4);
            return code.ToString();
        }

        //获取sendaction代码
        static private string GetSendActionCode(Subsys sys,SubsysAction ac)
        {
            var code = new StringBuilder(GetTemplate("TSendAction"));
            ReplaceText(code, "sendaction", ac.Name);
            ReplaceText(code, "framename", ac.FrameName);
            ReplaceText(code, "channelname", ac.ChannelName);
            var setlist = new List<string>();
            foreach(var setor in ac.Maps)
            {
                if(ProIsArray(sys, setor.SysPropertyName))
                {
                    setlist.Add(string.Format("pack.SetSegmentValue(\"{0}\", {1}.Select(p => p.Value).ToArray());", setor.FrameSegName, setor.SysPropertyName));
                }
                else
                    setlist.Add(string.Format("pack.SetSegmentValue(\"{0}\", {1}.Value);", setor.FrameSegName, setor.SysPropertyName));
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
                if(ProIsArray(sys, setor.SysPropertyName))
                {
                    getlist.Add(string.Format("{0}.Clear();",setor.SysPropertyName));
                    getlist.Add(string.Format("var __{0} = data.GetByteArray(\"{1}\");", setor.SysPropertyName, setor.FrameSegName));
                    getlist.Add(string.Format("if (__{0} != null) foreach (var v in __{0}) {0}.Add(new Parameter<{1}?>(v));",setor.SysPropertyName, GetPropertyTypeName(GetProType(sys,setor.SysPropertyName))));
                }
                else
                    getlist.Add(string.Format("{0}.Value = data.{1}(\"{2}\");", setor.SysPropertyName,GetGetorName(sys,setor.SysPropertyName), setor.FrameSegName));
                
            }
            ReplaceText(code, "getvaluelist", getlist, 4);
            return code.ToString();
        }

        #endregion


        #region --Helper--

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
                    return "BetByte";
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
        static private string GetPropertyTypeName(syspropertytype ty)
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
