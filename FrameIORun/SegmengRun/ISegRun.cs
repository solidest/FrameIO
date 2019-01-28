using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal interface ISegRun
    {

        //名称
        string Name { get; }
        int GetBitLen(JObject parent);
        SegRunContainer Parent { get; set; }
        SegRunBase First { get; set; }
        SegRunBase Last { get; set; }
    }



    //运行时字段的基类

    internal abstract class  SegRunBase : ISegRun
    {
        public abstract void Pack(IFrameWriteBuffer buff, JObject parent, JToken theValue);
        public abstract JToken GetDefaultValue();
        public abstract JToken GetAutoValue(IFrameWriteBuffer buff, JObject parent);
        public string Name { get; set; }

        #region --Const Token--
        internal const string FRAMELIST_TOKEN = "FrameList";
        internal const string SEGMENTLIST_TOKEN = "SegmentList";
        internal const string SEGMENTTYPE_TOKEN = "SegmentType";
        internal const string ONEOFLIST_TOKEN = "OneOfList";

        internal const string HEADERMATCH_TOKEN = "Match";
        internal const string HEADERMATCHLEN_TOKEN = "MatchBytesLen";
        internal const string ARRAYLEN_TOKEN = "ArrayLength";
        internal const string ONEOFBYSEGMENT_TOKEN = "DependOnSegment";
        internal const string ONEOFBYVALUE_TOKEN = "DependValue";

        internal const string EXPSIZEOF_TOKEN = "BytesSizeOf";
        internal const string EXPADD_TOKEN = "Calc_Add";
        internal const string EXPSUB_TOKEN = "Calc_Sub";
        internal const string EXPMUL_TOKEN = "Calc_Mul";
        internal const string EXPDIV_TOKEN = "Calc_Div";

        internal const string BITCOUNT_TOKEN = "BitCount";
        internal const string BYTEORDERT_TOKEN = "ByteOrder";
        internal const string ENCODED_TOKEN = "Encoded";
        internal const string VALUE_TOKEN = "Value";
        internal const string SIGNED_TOKEN = "Signed";
        internal const string MINVALUE_TOKEN = "MinValue";
        internal const string MAXVALUE_TOKEN = "MaxValue";
        internal const string CHECKTYPE_TOKEN = "CheckType";
        internal const string CHECKFROM_TOKEN = "CheckFrom";
        internal const string CHECKTO_TOKEN = "CheckTo";
        internal const string REALTYPE_TOKEN = "RealType";
        internal const string FLOAT_TOKEN = "Float";
        internal const string DOUBLE_TOKEN = "Double";

        #endregion

        public abstract bool IsArray { get; }


        public SegRunContainer Parent { get; set; }

        public SegRunBase Next { get; set; }

        public SegRunBase Previous { get; set; }

        public SegRunBase First { get; set; }

        public SegRunBase Last { get; set; }

        public SegRunBase Root { get; set; }


        public abstract int GetBitLen(JObject parent);

        //自上而下查找首个值字段 同时初始化所有parent
        public abstract bool LookUpFirstValueSeg(out SegRunValue firstSeg, out JContainer pc, out int repeated, JObject ctx, JToken theValue);


        public virtual void LogError(Interface.FrameIOErrorType type, string info)
        {
            var pos = new StringBuilder(Name);
            var p = Parent;
            while (p != null)
            {
                pos.Insert(0, p.Name + ".");
                p = p.Parent;
            }
            throw new Interface.FrameIOException(type, pos.ToString(), info);
        }

        static public JObject GetValueParent(JToken v)
        {
            if (v.Parent == null)
                return null;
            if (v.Parent.Type == JTokenType.Array)
                return (JObject)v.Parent.Parent.Parent;
            else if (v.Parent.Type == JTokenType.Property)
                return (JObject)v.Parent.Parent;
            else
                throw new Exception("unknow");
        }

    }

}
