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
            //HACK 生成总的头文件

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
            return string.Format("FioXChannel * {0};", ch.Name);
        }

        protected override IList<string> GetChannelInitialFun(SubsysChannel ch)
        {
            var ret = new List<string>();
            ret.Add(string.Format("void InitialChannel{0}()", ch.Name));
            ret.Add("{");
            ret.Add(string.Format("\t{0} = new FioXChannel({1});", ch.Name, (int)ch.ChannelType));
            foreach (var op in ch.Options)
            {
                ret.Add(string.Format("\t{0}->SetOption({1}, {2});", ch.Name, op.Name, op.OptionValue));
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


        protected override IList<string> GetSendCode(JProperty seg, SubsysProperty pro)
        {
            var ret = new List<string>();

            return ret;
        }

        protected override IList<string> GetSendFunClose(IList<string> paras, SubsysAction ac)
        {
            var ret = new List<string>();

            return ret;
        }

        protected override IList<string> GetSendFunDeclear(IList<string> paras, SubsysAction ac)
        {
            var ret = new List<string>();

            return ret;
        }

        #endregion

        #region --接收数据--

        protected override IList<string> GetRecvCode(JProperty seg, SubsysProperty pro)
        {
            var ret = new List<string>();

            return ret;
        }

        protected override IList<string> GetRecvFunClose(IList<string> paras, SubsysAction ac)
        {
            var ret = new List<string>();

            return ret;
        }

        protected override IList<string> GetRecvFunDeclear(IList<string> paras, SubsysAction ac)
        {
            var ret = new List<string>();

            return ret;
        }

        protected override string GetRecvSwitchKey(string segFullName)
        {
            return "";
        }

        #endregion

    }
}
