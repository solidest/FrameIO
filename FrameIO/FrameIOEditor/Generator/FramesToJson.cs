using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //将数据帧信息转换为Json
    public class Frames2Json
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
        internal const string TOENUM_TOKEN = "ToEnum";
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

        private IList<Frame> _frms;
        private IList<Enumdef> _ems;
        private JObject _jfrms;
        private IList<Subsys> _subsys;
        private Dictionary<string, JObject> _segs;

        public Frames2Json(IOProject pj)
        {
            _frms = pj.FrameList;
            _ems = pj.EnumdefList;
            _subsys = pj.SubsysList;
            _segs = new Dictionary<string, JObject>();

            _jfrms = new JObject();

            foreach(var frm in _frms)
            {
                if(IsUsedBySys(frm))
                {
                    _jfrms.Add(frm.Name, Frame2JObject(frm));
                }
            }
        }

        #region --Public Helper--

        //取字段的toenum属性
        internal string GetToEnum(string segFullName)
        {
            return _segs[segFullName]?[TOENUM_TOKEN]?.Value<string>();
        }

        //取给定节点的子字段组接口
        public IEnumerable<JProperty> GetChildren(JProperty seg, bool isFrame = false)
        {
            if (isFrame)
            {
                return seg.Value.Value<JObject>()[SEGMENTLIST_TOKEN].Value<JObject>().Properties();
            }
            Debug.Assert(seg.Name == SEGMENTLIST_TOKEN || seg.Name == ONEOFLIST_TOKEN);
            return ((JObject)seg.Value).Properties();
        }

        //取子字段组节点
        public JProperty GetChildrenNode(JProperty seg, out JProperty bySeg, out string intoCase)
        {

            var vseg = seg.Value.Value<JObject>();
            Debug.Assert(vseg != null);
            bySeg = null;
            intoCase = "";

            if(vseg.ContainsKey(SEGMENTTYPE_TOKEN))
            {
                var type = (SegmentTypeEnum)Enum.Parse(typeof(SegmentTypeEnum), vseg[SEGMENTTYPE_TOKEN].Value<string>());
                switch (type)
                {
                    case SegmentTypeEnum.SegInteger:
                    case SegmentTypeEnum.SegReal:
                    case SegmentTypeEnum.SegIntegerArray:
                    case SegmentTypeEnum.SegRealArray:
                        return null;

                    case SegmentTypeEnum.SegGroup:
                    case SegmentTypeEnum.SegGroupArray:
                        return vseg.Property(SEGMENTLIST_TOKEN);

                    case SegmentTypeEnum.SegOneOfGroup:
                    case SegmentTypeEnum.SegOneOfGroupArray:
                        var bySegName = vseg[ONEOFBYSEGMENT_TOKEN].Value<string>();
                        bySeg = ((JObject)seg.Parent).Property(bySegName);
                        return vseg.Property(ONEOFLIST_TOKEN);

                    case SegmentTypeEnum.SegOneOfItem:
                        intoCase = seg.Name;
                        return vseg.Property(SEGMENTLIST_TOKEN);

                }
            }
            else if (vseg.ContainsKey(SEGMENTLIST_TOKEN))  //数据帧
            {
                return vseg.Property(SEGMENTLIST_TOKEN);
            }

            throw new Exception("unknow");
        }


        //查找数据帧
        public JProperty FindJFrame(string name)
        {
           return _jfrms?.Properties().Where(p => p.Name == name).First();
        }

        //取字段的全名
        public string GetSegFullName(JObject seg, string name, bool withFrame)
        {
            if (_segs.Where(p => p.Value == seg).Count() == 0)
            {
                var i = 0;
            }
            var fn =  _segs.Where(p => p.Value == seg).First().Key;
            if (withFrame) return fn;
            return fn.Substring(fn.IndexOf('.') + 1);
            throw new Exception("unknow");
        }

        #endregion

        //获取全部数据帧的json字符串
        public string GetJsonString()
        {
            return _jfrms.ToString();
        }

        //转换数据帧为json对象
        private JObject Frame2JObject(Frame frm)
        {
            var o = new JObject();

            var fseg = GetHeaderSegment(frm.Segments);
            if (fseg != null && fseg.Match!=null && fseg.Match.Length>0 && fseg.BitCount>=8 && fseg.BitCount%8==0)
            {
                o.Add(HEADERMATCH_TOKEN, Helper.ToULong(fseg.Match));
                o.Add(HEADERMATCHLEN_TOKEN, fseg.BitCount / 8);
            }

            o.Add(SEGMENTLIST_TOKEN, SegmentList2JObject(frm.Segments, frm.Name));

            return o;
        }


        //转字段组为json
        private JObject SegmentList2JObject(IList<FrameSegmentBase> segs, string parentName)
        {
            var ret = new JObject();
            foreach (var seg in segs)
            {
                var fullname = parentName + "." + seg.Name;
                var o = Segment2JObject(seg, segs, fullname);
                ret.Add(seg.Name, o);
                _segs.Add(fullname, o);
            }
            return ret;
        }

        //转字段为json属性
        private JObject Segment2JObject(FrameSegmentBase seg, IList<FrameSegmentBase> segs, string parentName)
        {
            var t = seg.GetType();
            JObject o = null;
            if (t == typeof(FrameSegmentInteger))
                o = IntegerSegment2JObject((FrameSegmentInteger)seg, parentName);
            else if (t == typeof(FrameSegmentReal))
                o = RealSegment2JObject((FrameSegmentReal)seg, parentName);
            else if (t == typeof(FrameSegmentBlock))
                o = BlockSegment2JObject((FrameSegmentBlock)seg, segs, parentName);
            if(o == null) throw new Exception("unknow");
            return o;
        }


        #region --字段处理--


        //转整数字段为json对象
        private JObject IntegerSegment2JObject(FrameSegmentInteger seg, string parentName)
        {
            var isarray = !seg.Repeated.IsIntOne();
            var ret = GetNewSegJObject(isarray ? SegmentTypeEnum.SegIntegerArray : SegmentTypeEnum.SegInteger);
            if (isarray) ret.Add(ARRAYLEN_TOKEN, Exp2JToken(seg.Repeated));
            
            ret.Add(SIGNED_TOKEN, seg.Signed);
            ret.Add(BITCOUNT_TOKEN ,seg.BitCount);
            ret.Add(BYTEORDERT_TOKEN, seg.ByteOrder.ToString());
            ret.Add(ENCODED_TOKEN, seg.Encoded.ToString());
            if (seg.ToEnum != null && seg.ToEnum.Length > 0) ret.Add(TOENUM_TOKEN, seg.ToEnum);
            if(seg.Value!=null) ret.Add(VALUE_TOKEN, Exp2JToken(seg.Value));

            if(seg.ValidateMin!=null && seg.ValidateMin.Length>0)
                ret.Add(MINVALUE_TOKEN, Helper.ValidateIsInt(seg.ValidateMin) ? Helper.ToLong(seg.ValidateMin) : Convert.ToDouble(seg.ValidateMin));
            if (seg.ValidateMax != null && seg.ValidateMax.Length > 0)
                ret.Add(MAXVALUE_TOKEN, Helper.ValidateIsInt(seg.ValidateMax) ? Helper.ToLong(seg.ValidateMax) : Convert.ToDouble(seg.ValidateMax));
            if (seg.ValidateCheck != CheckType.None)
            {
                ret.Add(CHECKTYPE_TOKEN, seg.ValidateCheck.ToString());
                if (seg.CheckRangeBegin != null && seg.CheckRangeBegin.Length > 0) ret.Add(CHECKFROM_TOKEN, seg.CheckRangeBegin);
                if (seg.CheckRangeEnd != null && seg.CheckRangeEnd.Length > 0) ret.Add(CHECKTO_TOKEN, seg.CheckRangeEnd);
            }

            return ret;
        }

        //转小数字段为json对象
        private JObject RealSegment2JObject(FrameSegmentReal seg, string parentName)
        {
            var isarray = !seg.Repeated.IsIntOne();
            var ret = GetNewSegJObject(isarray ? SegmentTypeEnum.SegRealArray : SegmentTypeEnum.SegReal);
            if (isarray) ret.Add(ARRAYLEN_TOKEN, Exp2JToken(seg.Repeated));

            ret.Add(REALTYPE_TOKEN, seg.IsDouble? DOUBLE_TOKEN:FLOAT_TOKEN);
            ret.Add(BYTEORDERT_TOKEN, seg.ByteOrder.ToString());
            ret.Add(ENCODED_TOKEN, seg.Encoded.ToString());
            if (seg.Value != null) ret.Add(VALUE_TOKEN, Exp2JToken(seg.Value));

           
            if (seg.ValidateMin != null && seg.ValidateMin.Length > 0)
                ret.Add(MINVALUE_TOKEN, Convert.ToDouble(seg.ValidateMin));
            if (seg.ValidateMax != null && seg.ValidateMax.Length > 0)
                ret.Add(MAXVALUE_TOKEN, Convert.ToDouble(seg.ValidateMax));

            return ret;
        }

        //转block字段为json对象
        private JObject BlockSegment2JObject(FrameSegmentBlock seg, IList<FrameSegmentBase> segs, string parentName)
        {
            JObject ret = null;

            var isarray = !seg.Repeated.IsIntOne();

            switch (seg.UsedType)
            {
                case BlockSegType.RefFrame:
                    ret = GetNewSegJObject(isarray ? SegmentTypeEnum.SegGroupArray : SegmentTypeEnum.SegGroup);
                    ret.Add(SEGMENTLIST_TOKEN, SegmentList2JObject(FindFrame(seg.RefFrameName).Segments, parentName));
                    break;

                case BlockSegType.DefFrame:
                    ret = GetNewSegJObject(isarray ? SegmentTypeEnum.SegGroupArray : SegmentTypeEnum.SegGroup);
                    ret.Add(SEGMENTLIST_TOKEN, SegmentList2JObject(seg.DefineSegments, parentName));
                    break;

                case BlockSegType.OneOf:
                    ret = GetNewSegJObject(isarray ? SegmentTypeEnum.SegOneOfGroupArray : SegmentTypeEnum.SegOneOfGroup);
                    ret.Add(ONEOFBYSEGMENT_TOKEN, seg.OneOfBySegment);
                    var oo = new JObject();
                    OneOfItems2JObject(seg.OneOfCaseList, oo,
                        ((FrameSegmentInteger)segs.Where(p => p.Name == seg.OneOfBySegment).First()).ToEnum, isarray,
                        parentName);
                    ret.Add(ONEOFLIST_TOKEN, oo);
                    break;
                default:
                    throw new Exception("unknow");
            }

            if (isarray) ret.Add(ARRAYLEN_TOKEN, Exp2JToken(seg.Repeated));
            return ret;

        }


        //转OneOf列表为Json数组
        private void OneOfItems2JObject(IList<OneOfMap> items, JObject parent, string emName, bool isArray, string parentName)
        {
            foreach (var it in items)
            {
                var pname = parentName + "." + it.EnumItem;

                var o = GetNewSegJObject(SegmentTypeEnum.SegOneOfItem);
                if (it.EnumItem != "other")
                    o.Add(ONEOFBYVALUE_TOKEN, GetEnumItemValue(emName, it.EnumItem));
                else
                    o.Add(ONEOFBYVALUE_TOKEN, "other");
                o.Add(SEGMENTLIST_TOKEN, SegmentList2JObject(FindSegmentsOfFrame(it.FrameName), pname));
                parent.Add(it.EnumItem, o);
                _segs.Add(pname, o);
            }
        }


        #endregion


        #region --Helper--

        //查找数据帧
        private Frame FindFrame(string name)
        {
            return _frms.Where(p => p.Name == name).First();
        }

        //数据帧是否被分系统使用
        private bool IsUsedBySys(Frame frm)
        {
            //HACK IsUsedBySys
            //return true;
            return _subsys.Where(p => p.Actions.Where(a => a.FrameName == frm.Name).Count() > 0).Count() > 0;
        }


        //找到数据帧头
        private FrameSegmentInteger GetHeaderSegment(IList<FrameSegmentBase> segs)
        {
            if (segs == null || segs.Count == 0) return null;
            return GetHeaderSegment(segs.First());
        }

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
                        return GetHeaderSegment(FindSegmentsOfFrame(bseg.RefFrameName));

                    case BlockSegType.DefFrame:
                        return GetHeaderSegment(bseg.DefineSegments);

                }
            }

            return null;
        }

        //查找数据帧
        private IList<FrameSegmentBase> FindSegmentsOfFrame(string name)
        {
            return _frms.Where(p => p.Name == name).First().Segments;
        }

        //表达式转Json
        private JToken Exp2JToken(Exp ep)
        {
            switch (ep.Op)
            {
                case exptype.EXP_INT:
                    return new JValue(Helper.ToLong(ep.ConstStr));

                case exptype.EXP_REAL:
                    return new JValue(Convert.ToDouble(ep.ConstStr));

                case exptype.EXP_ID:
                    return new JValue(ep.ConstStr);

                case exptype.EXP_BYTESIZEOF:
                    {
                        var o = new JObject();
                        o.Add(EXPSIZEOF_TOKEN, ep.ConstStr);
                        return o;
                    }

                case exptype.EXP_ADD:
                    {
                        var o = new JObject();
                        o.Add(EXPADD_TOKEN, GetCaclArray(ep));
                        return o;
                    }

                case exptype.EXP_SUB:
                    {
                        var o = new JObject();
                        o.Add(EXPSUB_TOKEN, GetCaclArray(ep));
                        return o;
                    }

                case exptype.EXP_MUL:
                    {
                        var o = new JObject();
                        o.Add(EXPMUL_TOKEN, GetCaclArray(ep));
                        return o;
                    }

                case exptype.EXP_DIV:
                    {
                        var o = new JObject();
                        o.Add(EXPDIV_TOKEN, GetCaclArray(ep));
                        return o;
                    }
            }
            throw new Exception("unknow");
        }

        //取计算参数数组
        private JArray GetCaclArray(Exp ep)
        {
            var ret = new JArray();
            ret.Add(Exp2JToken(ep.LeftExp));
            ret.Add(Exp2JToken(ep.RightExp));
            return ret;
        }

        //创建新json字段并赋值类型属性
        private JObject GetNewSegJObject(SegmentTypeEnum t)
        {
            var ret = new JObject();
            ret.Add(SEGMENTTYPE_TOKEN, t.ToString());
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
                ret = (v == null || v == "") ? (ret + 1) : Helper.ToLong(v);
                i += 1;
                if (i == em.ItemsList.Count) break;
            } while (n != itname);
            return ret;
        }

        private enum SegmentTypeEnum
        {
            SegInteger,
            SegIntegerArray,
            SegReal,
            SegRealArray,
            SegGroup,
            SegGroupArray,
            SegOneOfGroup,
            SegOneOfGroupArray,
            SegOneOfItem
        }

        #endregion
    }
}
