using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FrameIO.Main
{
    public class CppScriptGenerator : ScriptGenerator
    {

        #region --Initial--

        public CppScriptGenerator(IOProject pj, IOutText tout) : base(pj, tout)
        {
        }

        protected override string Token => "cpp";

        protected override string DefaultExtension => "h";

        protected override string SystemTemplate => "TSubSys";

        protected override string ExceptionHandlerTemplate => "TExceptionHandler";


        #endregion

        #region --数据帧--

        internal override void CreateFramsFile(IList<string> frames)
        {
            if (frames.Count == 0) return;
            var codes = frames.Select(p => "\"" + p + "\"\\").ToList();
            var las = codes[codes.Count - 1];
            codes[codes.Count - 1] = las.Substring(0, las.Length - 1);
            OutFile("TFrmaes", "frmaes.h",
                "framesconfig", codes);

            AppendTextTo("enums.h", "#pragma once");
            OutFile("FioChannel", "FioChannel");
            OutFile("FioObjextX", "FioObjextX");
            OutFile("FioRunner", "FioRunner");
            OutFile("FrameIORunX", "FrameIORunX");

        }

        #endregion

        #region --枚举--

        //生成枚举文件代码内容
        protected override void CreateEnumFile(Enumdef em)
        {
            var code = em.ItemsList.Select(p => p.Name + (p.ItemValue.Length == 0 ? "," : (" = " + p.ItemValue + ","))).ToList();
            var emt = GetTemplateBuilder("TEnum", "enumlist", code,
                "enumname", em.Name);
            AppendTextTo("enums.h", emt.ToString());
        }


        #endregion

        #region --子系统--

        //子系统文件
        protected override StringBuilder GetInnerSubsysFileContent(InnerSubsys inner)
        {
            var pros = new List<string>();
            foreach (var pro in inner.Propertys)
            {
                pros.Add(GetPropertyDefCode(pro));
            }

            var inis = new List<string>();
            foreach (var pro in inner.Propertys)
            {
                inis.Add(GetPropertyIniCode(pro));
            }

            return GetTemplateBuilder("TStruct", 
                "propertydeclare", pros,
                "structname", inner.Name);
        }


        #endregion

        #region --属性--

        //属性定义
        protected override string GetPropertyDefCode(SubsysProperty pro)
        {
            var ret = new StringBuilder();
            if (pro.IsBaseType() || pro.IsEnum(_pj))
            {
                if (pro.IsArray)
                    if (pro.ArrayLen != null && pro.ArrayLen.Length > 0)
                        ret.Append("bool name[" + pro.ArrayLen + "];");
                    else
                        ret.Append("bool * name;");
                else
                    ret.Append("bool name;");
            }
            else
            {
                Debug.Assert(pro.IsInnerSubsys(_pj));
                if (pro.IsArray)
                    if (pro.ArrayLen != null && pro.ArrayLen.Length > 0)
                        ret.Append("bool name[" + pro.ArrayLen + "];");
                    else
                        ret.Append("bool * name;");
                else
                    ret.Append("bool name;");
            }

            ret.Replace("bool", pro.CppTypeName);
            ret.Replace("name", pro.Name);
            return ret.ToString();
        }


        #endregion

        #region --通道--


        protected override string GetChannelDeclare(SubsysChannel ch)
        {
            return string.Format("FioChannelX * {0};", ch.Name);
        }

        protected override IList<string> GetChannelInitialFun(SubsysChannel ch)
        {
            var ret = new List<string>();
            ret.Add(string.Format("void InitialChannel{0}()", ch.Name));
            ret.Add("{");
            ret.Add(string.Format("\t{0} = new FioChannelX({1});", ch.Name, (int)ch.ChannelType));
            foreach (var op in ch.Options)
            {
                ret.Add(string.Format("\t{0}->SetOption(\"{1}\", {2});", ch.Name, op.Name, op.OptionValue));
            }
            ret.Add(string.Format("\t{0}->InitialChannel();", ch.Name));
            ret.Add("}");
            return ret;
        }

        protected override IList<string> GetChannelRelease(SubsysChannel ch)
        {
            var ret = new List<string>();
            ret.Add(string.Format("void ReleaseChannel{0}()", ch.Name));
            ret.Add("{");
            ret.Add(string.Format("\tdelete {0};", ch.Name));
            ret.Add("}");
            return ret;
        }

        #endregion

        #region --发送数据--


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
            var dec = string.Format("void {0}({1})", ac.Name, pas ?? "");
            ret.Add(dec);
            ret.Add("{");
            ret.Add(string.Format("\tFioObjextX __v__(\"{0}\");", ac.FrameName));
            return ret;
        }

        protected override IList<string> GetSendCode(JProperty seg, SubsysProperty pro)
        {
            var ret = new List<string>();
            var fullSegName = _jframes.GetSegFullName(seg.Value.Value<JObject>(), false);
            if (pro.IsBaseType())
            {
                if(pro.IsArray)
                    ret.Add(string.Format("__v__.SetValue(\"{0}\", {1}, {2});", fullSegName, pro.Name, pro.ArrayLen));
                else
                    ret.Add(string.Format("__v__.SetValue(\"{0}\", {1});", fullSegName, pro.Name));

            }
            else if (pro.IsEnum(_pj))
            {
                if(pro.IsArray)
                    ret.Add(string.Format("__v__.SetValue(\"{0}\", (int){1}, {2});", fullSegName, pro.Name, pro.ArrayLen));
                else
                    ret.Add(string.Format("__v__.SetValue(\"{0}\", (int){1});", fullSegName, pro.Name));

            }
            else if (pro.IsInnerSubsys(_pj))
            {
                if (!pro.IsArray)
                {
                    ret.Add("{");
                    ret.Add("\tFioObjextX __vv__(\"\");");
                    foreach (var inpro in _pj.InnerSubsysList.Where(p => p.Name == pro.PropertyType).First().Propertys)
                    {
                        if(inpro.IsArray)
                            ret.Add(string.Format("\t__vv__.SetArray(\"{0}\", {1}, {2});", inpro.Name, pro.Name + "." + inpro.Name, inpro.ArrayLen));
                        else
                            ret.Add(string.Format("\t__vv__.SetValue(\"{0}\", {1});", inpro.Name, pro.Name + "." + inpro.Name));
                    }
                    ret.Add(string.Format("\t__v__.SetObjectX(\"{0}\", __vv__);", fullSegName));
                    ret.Add("}");
                }
                else
                {
                    ret.Add("{");
                    ret.Add(string.Format("\tauto __vvs__ = new FioObjextX*(\"\")[{0}];", pro.ArrayLen));
                    ret.Add(string.Format("\tfor (int i = 0; i < {0}; i++)", pro.ArrayLen));
                    ret.Add("\t{");
                    ret.Add("\t\tvar __vv__ = new FioObjextX(\"\");");
                    foreach (var inpro in _pj.InnerSubsysList.Where(p => p.Name == pro.PropertyType).First().Propertys)
                        ret.Add(string.Format("\t\t__vv__.SetValue(\"{0}\", {1}[i].{2});", inpro.Name, pro.Name, inpro.Name));
                    ret.Add("\t}");
                    ret.Add(string.Format("\tfor (int i = 0; i < {0}.Count; i++)", pro.ArrayLen));
                    ret.Add(string.Format("\t\t__v__.SetArray(\"{0}\", __vvs__, {1});", fullSegName, pro.ArrayLen));
                    ret.Add("\t\tdelete __vvs__[i];");
                    ret.Add("\t\tdelete [] __vvs__;");
                    ret.Add("}");
                }
            }
            return ret;
        }

        protected override IList<string> GetSendFunClose(IList<string> paras, SubsysAction ac)
        {
            var ret = new List<string>();
            ret.Add(string.Format("\tSendFrame(*{0}, __v__);", ac.ChannelName));
            ret.Add("}");
            return ret;
        }

        #endregion

        #region --接收数据--

        protected override IList<string> GetRecvFunDeclear(IList<string> paras, SubsysAction ac)
        {
            var ret = new List<string>();
            ret.Add(string.Format("void {0}()", ac.Name));
            ret.Add("{");
            ret.Add(string.Format("\tFioObjextX __v__(\"{0}\");", ac.FrameName));
            ret.Add(string.Format("\tRecvFrame(\"{0}\", *{1}, &__v__);", ac.FrameName, ac.ChannelName));
            return ret;
        }

        protected override IList<string> GetRecvCode(JProperty seg, SubsysProperty pro)
        {
            var ret = new List<string>();
            var fullSegName = _jframes.GetSegFullName(seg.Value.Value<JObject>(), false);
            if (pro.IsBaseType())
            {
                if (pro.IsArray)
                       ret.Add(string.Format("__v__.GetArray(\"{0}\", {1});", fullSegName, pro.Name));
                else
                    ret.Add(string.Format("{0} = __v__.{1}(\"{2}\");", pro.Name, GetGettorName(pro.PropertyType), fullSegName));

            }
            else if (pro.IsEnum(_pj))
            {
                ret.Add(string.Format("{0} = ({1})__v__.GetIntValue(\"{2}\");", pro.Name, pro.PropertyType, fullSegName));
            }
            else if (pro.IsInnerSubsys(_pj))
            {
                if (!pro.IsArray)
                {
                    ret.Add("{");
                    ret.Add(string.Format("\tFioObjextX __vv__(GetObjectXHandle(\"{0}\"));", fullSegName));
                    foreach (var inpro in _pj.InnerSubsysList.Where(p => p.Name == pro.PropertyType).First().Propertys)
                    {
                        if(inpro.IsArray)
                            ret.Add(string.Format("\t__vv__.GetArray(\"{0}\", {1});", inpro.Name, pro.Name + "." + inpro.Name));
                        else
                            ret.Add(string.Format("\t{0} = __vv__.{1}(\"{2}\");", pro.Name + "." + inpro.Name, GetGettorName(inpro.PropertyType), inpro.Name));

                    }

                    ret.Add("}");
                }
                else
                {
                    ret.Add("{");
                    ret.Add(string.Format("\tFioObjextXArray __vvs__ (__v__.GetObjectXArrayHandle(\"{0}\"));", fullSegName));
                    ret.Add(string.Format("\tfor (int i = 0; i < {0}; i++)", pro.ArrayLen));
                    ret.Add("\t{");
                    foreach (var inpro in _pj.InnerSubsysList.Where(p => p.Name == pro.PropertyType).First().Propertys)
                    {
                        if (inpro.IsArray)
                            ret.Add(string.Format("\t\t__vv__.GetArray(\"{0}\", {1}[__vvi__].{2}, {3});", inpro.Name, pro.Name, inpro.Name, inpro.ArrayLen));
                        else
                            ret.Add(string.Format("\t\t{0}[i].{1} = __vv__.{2}(\"{3}\", );",pro.Name, inpro.Name, GetGettorName(inpro.PropertyType), inpro.Name));

                    }
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
            return string.Format("({0})__v__.GetIntValue(\"{1}\")", _jframes.GetToEnum(segFullName), segFullName.Substring(segFullName.IndexOf('.') + 1));
        }

        private string GetGettorName(string typestr)
        {
            switch (typestr)
            {
                case "bool":
                    return "GetBoolValue";
                case "byte":
                    return "GetByteValue";
                case "sbyte":
                    return "GetSByteValue";
                case "short":
                    return "GetShortValue";
                case "ushort":
                    return "GetUShortValue";
                case "int":
                    return "GetIntValue";
                case "uint":
                    return "GetUIntValue";
                case "long":
                    return "GetLongValue";
                case "unsigned long":
                    return "GetULongValue";
                case "float":
                    return "GetFloatValue";
                case "double":
                    return "GetDoubleValue";
                default:
                    return typestr;
            }
        }

        #endregion

    }
}
