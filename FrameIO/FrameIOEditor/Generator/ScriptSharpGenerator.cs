using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FrameIO.Main
{
    //c# 代码生成驱动
    public class SharpScriptGenerator : ScriptGenerator
    {
        #region --Initial--

        public SharpScriptGenerator(IOProject pj, IOutText tout) : base(pj, tout)
        {

        }

        protected override string Token => "cs";
        protected override string DefaultExtension { get => "cs";}

        protected override string SystemTemplate => "TSubsys";

        protected override string ExceptionHandlerTemplate => "TExceptionHandler";

        protected override void CreateSharedFile()
        {
            //HACK
        }

        #endregion

        #region --Frames--

        protected override IList<string> ConvertFramesCode(IList<string> base64List)
        {
            return base64List.Select(p => "\"" + p + "\",").ToList();
        }

        #endregion

        #region --InnerSubsys--


        //创建子系统类文件
        protected override StringBuilder GetInnerSubsysFileContent(InnerSubsys inner)
        {
            var pros = new List<string>();
            foreach(var pro in inner.Propertys)
            {
                pros.Add(GetPropertyDefCode(pro));
            }

            var inis = new List<string>();
            foreach (var pro in inner.Propertys)
            {
                inis.Add(GetPropertyIniCode(pro));
            }

            return GetTemplateBuilder("TInnerSubsys", "propertydeclare", pros,
                "project", _pj.Name,
                "innersys", inner.Name,
                "propertyinitial", List2String(inis, 3));
        }


        #endregion

        #region --Property--

        //取属性定义代码
        protected override string GetPropertyDefCode(SubsysProperty pro)
        {
            var ret = new StringBuilder();
            if (pro.IsBaseType() || _pj.IsEnum(pro.PropertyType))
            {
                if (pro.IsArray)
                    ret.Append(@"public ObservableCollection<Parameter<bool?>> name { get; private set; }");
                else
                    ret.Append(@"public Parameter<bool?> name { get; private set;}");
            }
            else
            {
                if (pro.IsArray)
                    ret.Append(@"public ObservableCollection<bool> name { get; private set; }");
                else
                    ret.Append(@"public bool Name { get; private set; }");
            }

            ret.Replace("bool", pro.PropertyType);
            ret.Replace("name", pro.Name);
            return ret.ToString();
        }

        //取属性初始化代码
        protected override string GetPropertyIniCode(SubsysProperty pro)
        {
            var ret = new StringBuilder();
            if (pro.IsBaseType() || _pj.IsEnum(pro.PropertyType))
            {
                if (pro.IsArray)
                {
                    ret.Append(@"name = new ObservableCollection<Parameter<bool?>>();");
                    if ((pro.ArrayLen?.Length) > 0)
                        ret.AppendFormat(@" for (int i = 0; i < {0}; i++) name.Add(new Parameter<bool?>());", pro.ArrayLen);
                }
                else
                {
                    ret.Append(@"name = new Parameter<bool?>();");
                }
            }
            else
            {
                if (pro.IsArray)
                {
                    ret.Append(@"name = new ObservableCollection<bool>();");
                    if (pro.ArrayLen?.Length > 0)
                        ret.AppendFormat(@" for(int i=0; i<{0}; i++) name.Add(new bool());", pro.ArrayLen);
                }
                else
                    ret.Append(@"name = new bool();");
            }

            ret.Replace("bool", pro.PropertyType);
            ret.Replace("name", pro.Name);
            return ret.ToString();
        }

        #endregion

        #region --Channel--

        //通道声明
        protected override string GetChannelDeclare(SubsysChannel ch)
        {
            return string.Format("public FioChannel {0};", ch.Name);
        }


        //通道初始化
        protected override IList<string> GetChannelInitialFun(SubsysChannel ch)
        {
            var ret = new List<string>();
            ret.Add(string.Format("public void InitialChannel{0}(ChannelOption ops)", ch.Name));
            ret.Add("{");
            ret.Add("\tif (ops == null) ops = new ChannelOption();");
            foreach (var op in ch.Options)
            {
                ret.Add(string.Format("\tif (!ops.Contains(\"{0}\")) ops.SetOption(\"{0}\", {1});", op.Name, op.OptionValue));
            }
            ret.Add(string.Format("\tops.SetOption(\"{0}\", {1});", "$channeltype", (int)ch.ChannelType));
            ret.Add(string.Format("\t{0} = FioNetRunner.GetChannel(ops);", ch.Name));
            ret.Add("}");
            return ret;
        }



        #endregion

        #region --SendAction--


        protected override string GetSendFunDeclear(IList<string> paras, SubsysAction ac)
        {
            var parss = new StringBuilder();
            string pas = null;
            foreach (var item in paras)
            {
                parss.Append(_jframes.GetToEnum(item) + " " + item.Replace(".", "_") + ", ");
            }
            if (parss.Length > 2) pas = parss.ToString().Substring(0, parss.Length - 2);
            return string.Format("public void {0}({1})", ac.Name, pas ?? "");
        }

        protected override string GetRecvFunDeclear(IList<string> paras, SubsysAction ac)
        {
            return string.Format("public void {0}({1})", ac.Name, "");
        }

        protected override IList<string> GetSendCode(JProperty seg, SubsysActionMap map)
        {
            //HACK
            var ret = new List<string>();
            ret.Add(string.Format("{0} = {1};", map.SysPropertyName, map.FrameSegName));
            return ret;
        }

        protected override IList<string> GetRecvCode(JProperty seg, SubsysActionMap map)
        {
            //HACK
            var ret = new List<string>();
            ret.Add(string.Format("{0} = {1};", map.FrameSegName, map.SysPropertyName));
            return ret;
        }

        #endregion

    }
}
