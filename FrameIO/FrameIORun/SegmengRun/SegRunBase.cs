using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //运行时字段的基类
    internal abstract class SegRunBase
    {
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

        //名称
        internal abstract string Name { get; set; }

        //父级容器
        internal abstract SegRunContainer Parent { get; set; }

        //下一个兄弟
        internal abstract SegRunBase Next { get; set; }

        //上一个兄弟
        internal abstract SegRunBase Previous { get; set; }

        //第一个子字段
        internal abstract SegRunBase First { get; set; }

        //最后一个子字段
        internal abstract SegRunBase Last { get; set; }

        //根字段 数据帧
        internal abstract SegRunContainer Root { get;  set; }

        //从json填充属性
        internal protected abstract void FillFromJson(JObject o);


        //打包
        internal abstract SegRunBase Pack(FramePackBuffer buff, JToken value);
    }
}
