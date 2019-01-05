using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //将数据帧信息转换为Json
    public class Frames2Json
    {
        private IList<Frame> _frms;
        private IList<Enumdef> _ems;
        private JArray _jfrms;
        private const string TYPE_TOKEN = "$type";
        private const string NAME_TOKEN = "Name";
        private const string HEADERMATCH_TOKEN = "Match";
        private const string HEADERMATCHLEN_TOKEN = "MatchBytesLen";
        private const string SEGMENTLIST_TOKEN = "SegmentList";
        private const string EXPTYPE_TOKEN = "ExpressionType";
        private const string EXPVALUE_TOKEN = "ExpressionValue";
        private const string EXPLEFT_TOKEN = "ExpLeft";
        private const string EXPRIGHT_TOKEN = "ExpRight";
        private const string ARRAYLEN_TOKEN = "ArrayLength";
        private const string ONEOFBYSEGMENT_TOKEN = "DependOnSegment";
        private const string ONEOFBYVALUE_TOKEN = "DependValue";
        private const string ONEOFITEMLIST_TOKEN = "OneOfItemList";

        private const string BITCOUNT_TOKEN = "BitCount";
        private const string BYTEORDERT_TOKEN = "ByteOrder";
        private const string ENCODED_TOKEN = "Encoded";
        private const string VALUE_TOKEN = "Value";
        private const string SIGNED_TOKEN = "Signed";
        private const string MINVALUE_TOKEN = "MinValue";
        private const string MAXVALUE_TOKEN = "MaxValue";
        private const string CHECKTYPE_TOKEN = "CheckType";
        private const string CHECKFROM_TOKEN = "CheckFrom";
        private const string CHECKTO_TOKEN = "CheckTo";
        private const string REALTYPE_TOKEN = "RealType";
        private const string FLOAT_TOKEN = "float";
        private const string DOUBLE_TOKEN = "double";


        public Frames2Json(IList<Frame> frms, IList<Enumdef> ems)
        {
            _frms = frms;
            _ems = ems;
            _jfrms = new JArray();
            foreach(var frm in frms)
            {
                _jfrms.Add(Frame2JObject(frm));
            }
        }

        //获取全部数据帧的json字符串
        public string GetJsonString()
        {
            return _jfrms.ToString();
        }

        //转换数据帧为json对象
        private JObject Frame2JObject(Frame frm)
        {
            var ret = GetNewJObject(FrametIOType.Frame, frm.Name);

            var fseg = GetHeaderSegment(frm.Segments.First());
            if (fseg != null && fseg.Match!=null && fseg.Match.Length>0 && fseg.BitCount>=8 && fseg.BitCount%8==0)
            {
                ret.Add(HEADERMATCH_TOKEN, Convert.ToInt64(fseg.Match));
                ret.Add(HEADERMATCHLEN_TOKEN, fseg.BitCount / 8);
            }

            ret.Add(SEGMENTLIST_TOKEN, Segments2JArray(frm.Segments));

            return ret;
        }

        #region --字段处理--

        //转字段为json对象
        private JObject Segment2JObject(FrameSegmentBase seg, IList<FrameSegmentBase> segs)
        {
            var t = seg.GetType();
            if (t == typeof(FrameSegmentInteger))
                return Segment2JObject((FrameSegmentInteger)seg);
            else if (t == typeof(FrameSegmentReal))
                return Segment2JObject((FrameSegmentReal)seg);
            else if (t == typeof(FrameSegmentBlock))
                return Segment2JObject((FrameSegmentBlock)seg, segs);

            throw new Exception("unknow");
        }

        //转整数字段为json对象
        private JObject Segment2JObject(FrameSegmentInteger seg)
        {
            var isarray = !seg.Repeated.IsIntOne();
            var ret = GetNewJObject(isarray ? FrametIOType.SegIntegerArray : FrametIOType.SegInteger, seg.Name);
            if (isarray) ret.Add(ARRAYLEN_TOKEN, Exp2JObject(seg.Repeated));
            
            ret.Add(SIGNED_TOKEN, seg.Signed);
            ret.Add(BITCOUNT_TOKEN ,seg.BitCount);
            ret.Add(BYTEORDERT_TOKEN, seg.ByteOrder.ToString());
            ret.Add(ENCODED_TOKEN, seg.Encoded.ToString());
            if(seg.Value!=null) ret.Add(VALUE_TOKEN, Exp2JObject(seg.Value));

            if(seg.ValidateMin!=null && seg.ValidateMin.Length>0)
                ret.Add(MINVALUE_TOKEN, Helper.ValidateIsInt(seg.ValidateMin) ? Convert.ToInt64(seg.ValidateMin) : Convert.ToDouble(seg.ValidateMin));
            if (seg.ValidateMax != null && seg.ValidateMax.Length > 0)
                ret.Add(MAXVALUE_TOKEN, Helper.ValidateIsInt(seg.ValidateMax) ? Convert.ToInt64(seg.ValidateMax) : Convert.ToDouble(seg.ValidateMax));
            if (seg.ValidateCheck != CheckType.None)
            {
                ret.Add(CHECKTYPE_TOKEN, seg.ValidateCheck.ToString());
                if (seg.CheckRangeBegin != null && seg.CheckRangeBegin.Length > 0) ret.Add(CHECKFROM_TOKEN, seg.CheckRangeBegin);
                if (seg.CheckRangeEnd != null && seg.CheckRangeEnd.Length > 0) ret.Add(CHECKTO_TOKEN, seg.CheckRangeEnd);
            }

            return ret;
        }

        //转小数字段为json对象
        private JObject Segment2JObject(FrameSegmentReal seg)
        {
            var isarray = !seg.Repeated.IsIntOne();
            var ret = GetNewJObject(isarray ? FrametIOType.SegRealArray : FrametIOType.SegReal, seg.Name);
            if (isarray) ret.Add(ARRAYLEN_TOKEN, Exp2JObject(seg.Repeated));

            ret.Add(REALTYPE_TOKEN, seg.IsDouble? DOUBLE_TOKEN:FLOAT_TOKEN);
            ret.Add(BYTEORDERT_TOKEN, seg.ByteOrder.ToString());
            ret.Add(ENCODED_TOKEN, seg.Encoded.ToString());
            if (seg.Value != null) ret.Add(VALUE_TOKEN, Exp2JObject(seg.Value));

           
            if (seg.ValidateMin != null && seg.ValidateMin.Length > 0)
                ret.Add(MINVALUE_TOKEN, Convert.ToDouble(seg.ValidateMin));
            if (seg.ValidateMax != null && seg.ValidateMax.Length > 0)
                ret.Add(MAXVALUE_TOKEN, Convert.ToDouble(seg.ValidateMax));

            return ret;
        }

        //转block字段为json对象
        private JObject Segment2JObject(FrameSegmentBlock seg, IList<FrameSegmentBase> segs)
        {
            JObject ret = null;
            var isarray = !seg.Repeated.IsIntOne();

            switch (seg.UsedType)
            {
                case BlockSegType.RefFrame:
                    ret = GetNewJObject(isarray ? FrametIOType.SegBlockArray : FrametIOType.SegBlock, seg.Name);
                    ret.Add(SEGMENTLIST_TOKEN, Segments2JArray(seg.DefineSegments));
                    break;
                case BlockSegType.DefFrame:
                    ret = GetNewJObject(isarray ? FrametIOType.SegBlockArray : FrametIOType.SegBlock, seg.Name);
                    ret.Add(SEGMENTLIST_TOKEN, Segments2JArray(FindFrame(seg.RefFrameName)));
                    break;
                case BlockSegType.OneOf:
                    ret = GetNewJObject(isarray ? FrametIOType.SegOneOfGroupArray : FrametIOType.SegOneOfGroup, seg.Name);
                    ret.Add(ONEOFBYSEGMENT_TOKEN, seg.OneOfBySegment);
                    ret.Add(ONEOFITEMLIST_TOKEN, OneOfItems2JArray(seg.OneOfCaseList, ((FrameSegmentInteger)segs.Where(p => p.Name == seg.OneOfBySegment).First()).ToEnum));
                    break;
                default:
                    throw new Exception("unknow");
            }

            if (isarray) ret.Add(ARRAYLEN_TOKEN, Exp2JObject(seg.Repeated));
            return ret;

        }

        //转字段组为json数组
        private JArray Segments2JArray(IList<FrameSegmentBase> segs)
        {
            var ret = new JArray();
            foreach (var seg in segs)
            {
                ret.Add(Segment2JObject(seg, segs));
            }
            return ret;
        }

        //转OneOf列表为Json数组
        private JArray OneOfItems2JArray(IList<OneOfMap> items, string emName)
        {
            var ret = new JArray();
            foreach (var it in items)
            {
                ret.Add(OneOfItem2JObject(it, emName));
            }
            return ret;
        }

        //转OneOf项为Json对象
        private JObject OneOfItem2JObject(OneOfMap item, string emName)
        {
            var ret = GetNewJObject(FrametIOType.SegOneOfItem, item.EnumItem);
            ret.Add(ONEOFBYVALUE_TOKEN, GetEnumItemValue(emName, item.EnumItem));
            ret.Add(SEGMENTLIST_TOKEN, Segments2JArray(FindFrame(item.FrameName)));
            return ret;

        }

        #endregion


        #region --Helper--

        //找到数据帧头

        private FrameSegmentInteger GetHeaderSegment(FrameSegmentBase seg)
        {
            if (seg == null) return null;
            var t = seg.GetType();
            if (t == typeof(FrameSegmentInteger))
                return (FrameSegmentInteger)seg;

            if(t == typeof(FrameSegmentBlock))
            {
                var bseg = (FrameSegmentBlock)seg;
                switch (bseg.UsedType)
                {
                    case BlockSegType.RefFrame:
                        return GetHeaderSegment(FindFrame(bseg.RefFrameName).First());

                    case BlockSegType.DefFrame:
                        return GetHeaderSegment(bseg.DefineSegments.First());

                }
            }

            return null;
        }

        //查找数据帧
        private IList<FrameSegmentBase> FindFrame(string name)
        {
            return _frms.Where(p => p.Name == name).First().Segments;
        }

        //表达式转Json
        private JObject Exp2JObject(Exp ep)
        {
            var ret = GetNewJObject(FrametIOType.Expression, null);
            ret.Add(EXPTYPE_TOKEN, ep.Op.ToString());
            switch (ep.Op)
            {
                case exptype.EXP_INT:
                    ret.Add(EXPVALUE_TOKEN, Convert.ToInt64(ep.ConstStr));
                    break;

                case exptype.EXP_REAL:
                    ret.Add(EXPVALUE_TOKEN, Convert.ToDouble(ep.ConstStr));
                    break;

                case exptype.EXP_ID:
                case exptype.EXP_BYTESIZEOF:
                    ret.Add(EXPVALUE_TOKEN, ep.ConstStr);
                    break;

                case exptype.EXP_ADD:
                case exptype.EXP_SUB:
                case exptype.EXP_MUL:
                case exptype.EXP_DIV:
                    ret.Add(EXPLEFT_TOKEN, Exp2JObject(ep.LeftExp));
                    ret.Add(EXPRIGHT_TOKEN, Exp2JObject(ep.RightExp));
                    break;
                default:
                    break;
            }
            return ret;
        }

        //创建新json对象
        private JObject GetNewJObject(FrametIOType t, string name)
        {
            var ret = new JObject();
            ret.Add(TYPE_TOKEN, t.ToString());
            if (name != null && name.Length > 0) ret.Add(NAME_TOKEN, name);
            return ret;
        }

        //取枚举项的数值
        private long GetEnumItemValue(string enumname, string itname)
        {
            var em = _ems.Where(p => p.Name == enumname).First();
            int i = 0;
            long ret = -1;
            var n = "";
            do
            {
                n = em.ItemsList[i].Name;
                var v = em.ItemsList[i].ItemValue;
                ret = (v == null || v == "") ? (ret + 1) : Convert.ToInt64(v);
                i += 1;
                if (i == em.ItemsList.Count) break;
            } while (n != itname);
            return ret;
        }

        enum FrametIOType
        {
            Frame,
            SegInteger,
            SegIntegerArray,
            SegReal,
            SegRealArray,
            SegBlock,
            SegBlockArray,
            SegOneOfGroup,
            SegOneOfGroupArray,
            SegOneOfItem,
            Expression
        }

        #endregion
    }
}
