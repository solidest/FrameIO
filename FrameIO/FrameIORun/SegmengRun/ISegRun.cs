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
        
        #region --For Initial--

        //名称
        string Name { get; }

        //父级容器
        SegRunContainer Parent { get; }

        //下一个兄弟
        ISegRun Next { get; }

        //上一个兄弟
        ISegRun Previous { get; }

        //第一个子字段
        ISegRun First { get; }

        //最后一个子字段
        ISegRun Last { get; }

        //根数据帧
        SegRunFrame Root { get;  }


        #endregion

        #region --For Pack--

        //打包
        ISegRun Pack(IFrameWriteBuffer buff, JObject parent);


        //取位长度
        int GetBitLen(JObject parent);

        #endregion

        #region --For Unpack--

        //解包
        ISegRun Unpack(IFrameReadBuffer buff, JObject parent);


        //获取所需位长度
        bool GetNeedBitLen(ref int len, out ISegRun next, JObject parent);


        #endregion

        //记录错误信息
        void LogError(Interface.FrameIOErrorType type, string info);

    }

    //运行时字段的基类
    internal abstract class  SegRunBase : ISegRun
    {

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

        public string Name { get; set; }

        public SegRunContainer Parent { get; set; }

        public ISegRun Next { get; set; }

        public ISegRun Previous { get; set; }

        public ISegRun First { get; set; }

        public ISegRun Last { get; set; }

        public SegRunFrame Root { get; set; }


        protected abstract void InitialFromJson(JObject o);

        public abstract ISegRun Pack(IFrameWriteBuffer buff, JObject parent);
        public abstract ISegRun Unpack(IFrameReadBuffer buff, JObject parent);
        public abstract int GetBitLen(JObject parent);

        public abstract bool GetNeedBitLen(ref int len, out ISegRun next, JObject parent);


        public virtual void LogError(Interface.FrameIOErrorType type, string info)
        {
            var pos = new StringBuilder(Name);
            var p = Parent;
            while (p != null)
                pos.Insert(0, p.Name + ".");
            throw new Interface.FrameIOException(type, pos.ToString(), info);
        }



    }

}
