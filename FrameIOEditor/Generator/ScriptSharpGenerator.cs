using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        #endregion

        #region --Frames--

        internal override void CreateFramsFile(IList<string> frames)
        {
            var codes = frames.Select(p => "\"" + p + "\",").ToList();
            OutFile("TFioNetRunner", "FioNetRunner",
                "framesconfig", codes, 
                TPROJECT, _pj.Name);
            OutFile("TParameter", "Parameter");
            OutFile("TFioNetObject", "FioNetObject");

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
            if (pro.IsBaseType() || pro.IsEnum(_pj))
            {
                if (pro.IsArray)
                    ret.Append("public ObservableCollection<Parameter<bool?>> name { get; private set; }");
                else
                    ret.Append("public Parameter<bool?> name { get; private set;}");
            }
            else
            {
                Debug.Assert(pro.IsInnerSubsys(_pj));
                if (pro.IsArray)
                    ret.Append("public ObservableCollection<bool> name { get; private set; }");
                else
                    ret.Append("public bool name { get; private set; }");
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
                    ret.Append("name = new ObservableCollection<Parameter<bool?>>();");
                    if ((pro.ArrayLen?.Length) > 0)
                        ret.AppendFormat(" for (int i = 0; i < {0}; i++) name.Add(new Parameter<bool?>());", pro.ArrayLen);
                }
                else
                {
                    ret.Append("name = new Parameter<bool?>();");
                }
            }
            else
            {
                if (pro.IsArray)
                {
                    ret.Append("name = new ObservableCollection<bool>();");
                    if (pro.ArrayLen?.Length > 0)
                        ret.AppendFormat(" for(int i=0; i<{0}; i++) name.Add(new bool());", pro.ArrayLen);
                }
                else
                    ret.Append("name = new bool();");
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


        protected override IList<string> GetSendFunDeclear(IList<string> paras, SubsysAction ac)
        {
            var ret = new List<string>();

            var parss = new StringBuilder();
            string pas = null;
            foreach (var item in paras)
            {
                parss.Append(_jframes.GetToEnum(item) + " " + item.Replace(".", "_") + ", ");
            }
            if (parss.Length > 2) pas = parss.ToString().Substring(0, parss.Length - 2);
            var dec =  string.Format("public void {0}({1})", ac.Name, pas ?? "");
            ret.Add(dec);
            ret.Add("{");
            ret.Add(string.Format("\tvar __v__ = FioNetRunner.NewFrameObject(\"{0}\");", ac.FrameName));
            return ret;
        }


        protected override IList<string> GetSendCode(JProperty seg, SubsysProperty pro)
        {
            var ret = new List<string>();
            var fullSegName = _jframes.GetSegFullName(seg.Value.Value<JObject>(), false);
            if(pro.IsBaseType())
            {
                ret.Add(string.Format("__v__.SetValue(\"{0}\", {1});", fullSegName, pro.Name));
            }
            else if (pro.IsEnum(_pj))
            {
                ret.Add(string.Format("__v__.SetValue(\"{0}\", (int){1});", fullSegName, pro.Name));
            }
            else if(pro.IsInnerSubsys(_pj))
            {
                if(!pro.IsArray)
                {
                    ret.Add("{");
                    ret.Add(string.Format("\tvar __vv__ = new FioNetObject();", fullSegName));
                    foreach(var inpro in _pj.InnerSubsysList.Where(p=>p.Name == pro.PropertyType).First().Propertys)
                        ret.Add(string.Format("\t__vv__.SetValue(\"{0}\", {1});", inpro.Name, pro.Name + "." + inpro.Name));
                    ret.Add(string.Format("\t__v__.SetValue(\"{0}\", __vv__);", fullSegName));
                    ret.Add("}");
                }
                else
                {
                    ret.Add("{");
                    ret.Add(string.Format("\tvar __vvs__ = new Collection<FioNetObject>();", fullSegName));
                    ret.Add(string.Format("\tfor (int i = 0; i < {0}.Count; i++)", pro.Name));
                    ret.Add("\t{");
                    ret.Add(string.Format("\t\tvar __vv__ = new FioNetObject();", fullSegName));
                    int index = 0;
                    foreach (var inpro in _pj.InnerSubsysList.Where(p => p.Name == pro.PropertyType).First().Propertys)
                        ret.Add(string.Format("\t\t__vv__.SetValue(\"{0}\", {1}[i].{2});", inpro.Name, pro.Name, inpro.Name));
                    ret.Add("\t\t__vvs__.Add(__vv__);");
                    ret.Add("\t}");
                    ret.Add(string.Format("\t__v__.SetValue(\"{0}\", __vvs__);", fullSegName));
                    ret.Add("}");
                }
            }
            return ret;
        }


        protected override IList<string> GetSendFunClose(IList<string> paras, SubsysAction ac)
        {
            //FioNetRunner.SendFrame(__v__, ch);
            var ret = new List<string>();
            ret.Add(string.Format("\tFioNetRunner.SendFrame(__v__, {0});", ac.ChannelName));
            ret.Add("}");
            return ret;
        }


        #endregion

        #region --RecvAction-

        protected override IList<string> GetRecvFunDeclear(IList<string> paras, SubsysAction ac)
        {
            var ret = new List<string>();
            ret.Add(string.Format("public void {0}()", ac.Name));
            ret.Add("{");
            ret.Add(string.Format("\tvar __v__ = FioNetRunner.RecvFrame(\"{0}\", {1});", ac.FrameName, ac.ChannelName));
            return ret;
        }

        protected override IList<string> GetRecvCode(JProperty seg, SubsysProperty pro)
        {
            var ret = new List<string>();
            var fullSegName = _jframes.GetSegFullName(seg.Value.Value<JObject>(), false);
            if (pro.IsBaseType())
            {
                ret.Add(string.Format("__v__.GetValue(\"{0}\", {1});", fullSegName, pro.Name));
            }
            else if (pro.IsEnum(_pj))
            {
                ret.Add(string.Format("{0} = ({1})__v__.GetValue(\"{2}\");", pro.Name, pro.PropertyType, fullSegName));
            }
            else if (pro.IsInnerSubsys(_pj))
            {
                if (!pro.IsArray)
                {
                    ret.Add("{");
                    ret.Add(string.Format("\tvar __vv__ = __v__.GetObject(\"{0}\");", fullSegName));
                    foreach (var inpro in _pj.InnerSubsysList.Where(p => p.Name == pro.PropertyType).First().Propertys)
                        ret.Add(string.Format("\t__vv__.GetValue(\"{0}\", {1});", inpro.Name, pro.Name + "." + inpro.Name));
                    ret.Add("}");
                }
                else
                {
                    ret.Add("{");
                    ret.Add(string.Format("\tvar __vvs__ = __v__.GetObjectArray(\"{0}\");", fullSegName));
                    ret.Add("\tint __vvi__ = 0;");
                    ret.Add(string.Format("\tforeach(var __vv__ in __vvs__)", pro.Name));
                    ret.Add("\t{");
                    ret.Add(string.Format("\t\tif({0}.Count == __vvi__) break;", pro.Name));
                    int index = 0;
                    foreach (var inpro in _pj.InnerSubsysList.Where(p => p.Name == pro.PropertyType).First().Propertys)
                        ret.Add(string.Format("\t\t__vv__.GetValue(\"{0}\", {1}[__vvi__].{2});", inpro.Name, pro.Name, inpro.Name));
                    ret.Add("\t\t__vvi__ += 1;");
                    ret.Add("\t}");
                    ret.Add("}");
                }
            }
            return ret;
        }


        protected override IList<string> GetRecvFunClose(IList<string> paras, SubsysAction ac)
        {
            var ret = new List<string>();
            ret.Add("}");
            return ret;
        }

        protected override string GetRecvSwitchKey(string segFullName)
        {
            return string.Format("({0})__v__.GetValue(\"{1}\")", _jframes.GetToEnum(segFullName), segFullName);
        }

        #endregion

    }
}
