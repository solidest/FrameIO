using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //数据帧配置文件编译
    public class FrameCompiledFile
    {

        #region --Const--

        const byte LEN_USHORT = 16;
        const byte LEN_BYTE = 8;
        const byte LEN_CODE = 6;

        const byte CO_SEGINTEGER = 1;
        const byte CO_SEGREAL = 2;
        const byte CO_SEGTEXT = 3;
        const byte CO_SEGINTEGER_ARRAY = 4;
        const byte CO_SEGREAL_ARRAY = 5;
        const byte CO_SEGTEXT_ARRAY = 6;
        const byte CO_SEGBLOCK_IN = 7;
        const byte CO_SEGBLOCK_OUT = 8;
        const byte CO_SEGONEOF_IN = 9;
        const byte CO_SEGONEOF_OUT = 10;
        const byte CO_SEGONEOFITEM = 11;
        const byte CO_FRAME_BEGIN = 12;
        const byte CO_FRAME_END = 13;
        const byte CO_REF_FRAME = 14;
        
        const byte CO_VALIDATOR_MAX = 1;
        const byte CO_VALIDATOR_MIN = 2;
        const byte CO_VALIDATOR_EQUAL = 3;
        const byte CO_VALIDATOR_CHECK = 4;

        const byte POS_VALIDATOR_VALUE = 32;
        const byte POS_VALIDATOR_NEXT = 48;
        const byte POS_REF_FRAME = 48;


        const byte ENCO_PRIMITIVE = 1;
        const byte ENCO_INVERSION = 2;
        const byte ENCO_COMPLEMENT = 3;

        const byte EXP_NUMBER = 1;
        const byte EXP_ADD = 2;
        const byte EXP_SUB = 3;
        const byte EXP_MUL = 4;
        const byte EXP_DIV = 5;
        const byte EXP_REF_SEGMENT = 6;
        const byte EXP_FUN_BYTESIZEOF = 7;

        #endregion

        private List<ulong> _segmentlist = new List<ulong>();   //字段
        private List<ulong> _constlist = new List<ulong>();     //常量
        private List<ulong> _expression = new List<ulong>();    //表达式
        private List<ulong> _validatorlist = new List<ulong>(); //验证规则
        private Dictionary<string, ushort> _symbols = new Dictionary<string, ushort>();     //符号表

        static IOProject _pj;

        #region --Public--

        //编译配置文件
        public static FrameCompiledFile Compile(IOProject pj)
        {
            //const byte pos_ref_frame = 48;  //引用数据帧位

            _pj = pj;
            var ret = new FrameCompiledFile();
            var need_update = new Dictionary<ushort, string>(); //需要更新数据帧引用的位置

            for (int i=0; i<pj.FrameList.Count; i++)
            {
                ret.CompileFrame(pj.FrameList[i], need_update);
            }

            //追加结束字段
            //ret.AddSegment(CO_REF_END_ALL, "$");

            //更新数据帧引用
            foreach(var d in need_update)
            {
                ret.UpdateSegmentToken(d.Key, ret._symbols[d.Value], POS_REF_FRAME);
            }

            _pj = null;

            return ret;
        }

        //取字段索引
        public ushort this[string index]
        {
            get
            {
                return _symbols[index];
            }
        }

        //转换为数组
        public byte[] GetBytes()
        {
            //const byte pos_segment = 0;
            const byte pos_const = 16;
            const byte pos_expression = 32;
            const byte pos_validator = 48;
            if (_segmentlist.Count >= ushort.MaxValue || _constlist.Count >= ushort.MaxValue || _expression.Count >= ushort.MaxValue || _validatorlist.Count >= ushort.MaxValue)
                throw new Exception("outrange");
            ulong token = (ushort)_segmentlist.Count;
            SetTokenValue(ref token, (ushort)_constlist.Count, pos_const, LEN_USHORT);
            SetTokenValue(ref token, (ushort)_expression.Count, pos_expression, LEN_USHORT);
            SetTokenValue(ref token, (ushort)_validatorlist.Count, pos_validator, LEN_USHORT);

            using (var mst = new MemoryStream())
            {
                mst.Write(BitConverter.GetBytes(token), 0, 8);
                WriteMemory(mst, _constlist);
                WriteMemory(mst, _expression);
                WriteMemory(mst, _validatorlist);
                WriteMemory(mst, _segmentlist);
                mst.Close();
                return mst.ToArray();
            }
        }

        //写入内存
        private void WriteMemory(MemoryStream mst, List<ulong> data)
        {
            for (int i = 0; i < data.Count; i++)
                mst.Write(BitConverter.GetBytes(data[i]), 0, 8);
        }

        #endregion
        
        #region --编译数据帧--

        //编译一个数据帧
        private void CompileFrame(Frame fm, Dictionary<ushort, string> need_udpate)
        {
            //cosnt byte not_used = 6;
            const byte pos_refBegin = 16;
            const byte pos_refEnd = 32;
            //const byte pos_endall = 48;

            ulong token_begin = CO_FRAME_BEGIN;
            ulong token_end = CO_FRAME_END;

            ushort refbegin = AddSegment(token_begin, fm.Name);
            for (int i = 0; i < fm.Segments.Count; i++)
            {
                CompileSegment(fm.Segments[i], fm.Name, fm.Segments, need_udpate);
            }
            SetTokenValue(ref token_end, refbegin, pos_refBegin, LEN_USHORT);
            ushort refend = AddSegment(token_end, fm.Name + "$");
            UpdateSegmentToken(refbegin, refend, pos_refEnd);
            UpdateSegmentToken(refend, refend, pos_refEnd);
            UpdateSegmentToken(refbegin, refbegin, pos_refBegin);
            //need_udpate.Add(refend, "$");
            //need_udpate.Add(refbegin, "$");
        }

        #endregion       

        #region --编译字段--

        //编译字段
        private ushort CompileSegment(FrameSegmentBase seg, string pre, IList<FrameSegmentBase> brotherlist, Dictionary<ushort, string> need_udpate_refframe)
        {
            var ty = seg.GetType();
            if (ty == typeof(FrameSegmentInteger))
                return CompileSegment((FrameSegmentInteger)seg, pre, brotherlist);
            else if (ty == typeof(FrameSegmentReal))
                return CompileSegment((FrameSegmentReal)seg, pre);
            else if (ty == typeof(FrameSegmentText))
                return CompileSegment((FrameSegmentText)seg, pre);
            else if (ty == typeof(FrameSegmentBlock))
            {
                var bseg = (FrameSegmentBlock)seg;
                switch (bseg.UsedType)
                {
                    case BlockSegType.RefFrame:
                        return CompileSegment(bseg.RefFrameName, pre,seg.Name, need_udpate_refframe);
                    case BlockSegType.DefFrame:
                        return CompileSegment(bseg.DefineSegments, bseg, pre, need_udpate_refframe);
                    case BlockSegType.OneOf:
                        return CompileSegment(bseg.OneOfFromSegment, (ushort)bseg.OneOfCaseList.Count, bseg, pre, brotherlist, need_udpate_refframe);
                }
            }
            throw new Exception("unknow");
        }

        //编译引用数据帧字段
        private ushort CompileSegment(string framename, string parent_pre, string segname, Dictionary<ushort, string> need_udpate)
        {
            ulong token = CO_REF_FRAME;
            //const byte pos_myself = 16;
            var idx = AddSegment(token, parent_pre + "." + segname);
            //UpdateSegmentToken(idx, idx, pos_myself);
            need_udpate.Add(idx, framename);
            return idx;
        }

        //编译OneOf字段组
        private ushort CompileSegment(string bysegname, ushort caseitemcount, FrameSegmentBlock parent, string parent_pre, IList<FrameSegmentBase> brotherlist, Dictionary<ushort, string> need_udpate_refframe)
        {
            //const biyte pos_fiotype = 0;
            //const byte pos_not_used = 6;
            const byte pos_by_segment = 16;
            const byte pos_first_caseitem = 32;
            const byte pos_last_caseitem = 48;
            const byte pos_ref_outoneof = 32;

            var need_update_end = new List<ushort>();
            var pre = parent_pre + "." + parent.Name;
            ulong token_begin_oneof = CO_SEGONEOF_IN;
            var reftoken_begin_oneof = AddSegment(token_begin_oneof, pre);

            var byenum = FindToEnum(bysegname, brotherlist);
            var byseg = LookUpSegment(bysegname, parent_pre);
            var ref_intovalue = LookUpExp(GetEnumItemValue(byenum, parent.OneOfCaseList[0].EnumItem));
            var ref_firstcase = CompileSegment(LookUpExp(ref_intovalue), parent.OneOfCaseList[0].FrameName, pre + "." + parent.OneOfCaseList[0].EnumItem, parent.OneOfCaseList[0].IsDefault, need_update_end, need_udpate_refframe);
            var ref_lastcase = ref_firstcase;
            for (int i = 1; i < parent.OneOfCaseList.Count; i++)
            {
                ref_lastcase = CompileSegment(LookUpExp(ref_intovalue), parent.OneOfCaseList[i].FrameName, pre + "." + parent.OneOfCaseList[i].EnumItem, parent.OneOfCaseList[i].IsDefault, need_update_end, need_udpate_refframe);
            }

            ulong token_end_oneof = CO_SEGONEOF_OUT;
            var ref_end_oneof = AddSegment(token_end_oneof, pre + "$");

            SetTokenValue(ref token_begin_oneof, byseg, pos_by_segment, LEN_USHORT);
            SetTokenValue(ref token_begin_oneof, ref_firstcase, pos_first_caseitem, LEN_USHORT);
            SetTokenValue(ref token_begin_oneof, ref_lastcase, pos_last_caseitem, LEN_USHORT);
            UpdateSegmentToken(reftoken_begin_oneof, token_begin_oneof);

            for (int i = 0; i < need_update_end.Count - 1; i++)
            {
                UpdateSegmentToken(need_update_end[i], ref_end_oneof, pos_ref_outoneof);
            }

            return reftoken_begin_oneof;
        }

        //编译OneOf分支字段
        private ushort CompileSegment(ushort intovalue, string framename, string parent_pre, bool isDefault, IList<ushort> need_update_end, Dictionary<ushort, string> need_udpate_refframe)
        {
            const byte pos_isDefault = 6;
            //const biyte pos_fiotype = 0;
            //const byte pos_bysegment = 8;
            const byte pos_into_value = 16;
            //const byte pos_ref_outoneof = 32;
            //const byte POS_REF_FRAME = 48;

            ulong token = CO_SEGONEOFITEM;
            if (isDefault)
                SetTokenValue(ref token, 1, pos_isDefault, 1);
            else
                SetTokenValue(ref token, intovalue, pos_into_value, LEN_USHORT);

            var ret = AddSegment(token, parent_pre);
            need_update_end.Add(ret);
            need_udpate_refframe.Add(ret, framename);
            return ret;
        }


        //编译内联定义字段
        private ushort CompileSegment(IList<FrameSegmentBase> children, FrameSegmentBlock parent, string parent_pre, Dictionary<ushort, string> need_udpate)
        {
            //const char pos_fiotype = 0;
            const byte pos_first_childseg = 6;
            const byte pos_last_childseg = 22;
            const byte pos_in_seg = 38;
            const byte pos_out_seg = 38;

            ulong token1 = CO_SEGBLOCK_IN;
            var pre = parent_pre + "." + parent.Name;
            var pid_in = AddSegment(token1, pre);
            var first_seg_id = CompileSegment(children[0], pre, children, need_udpate);
            ushort last_seg_id = first_seg_id;
            for (int i = 1; i < children.Count; i++)
            {
                last_seg_id = CompileSegment(children[i], pre, children, need_udpate);
            }

            ulong token2 = CO_SEGBLOCK_OUT;
            token2 |= (ulong)first_seg_id << pos_first_childseg;
            token2 |= (ulong)last_seg_id << pos_last_childseg;
            token2 |= (ulong)pid_in << pos_in_seg;
            var pid_out = AddSegment(token2, pre + "$");

            token1 |= (ulong)first_seg_id << pos_first_childseg;
            token1 |= (ulong)last_seg_id << pos_last_childseg;
            token1 |= (ulong)pid_out << pos_out_seg;
            UpdateSegmentToken(pid_in, token1);
            return pid_in;
        }

        //编译整数字段
        private ushort CompileSegment(FrameSegmentInteger seg, string pre, IList<FrameSegmentBase> brotherlist)
        {
            //byte FioType;
            //byte Encoded;
            //byte ByteOrder;
            //bool IsSigned;
            //byte BitCount;
            //ushort RefRepeated;
            //ushort RefValue;
            //ushort RefValidator;
            const byte pos_encoded = 6;
            const byte pos_byteorder = 8;
            const byte pos_issigned = 9;
            const byte pos_bitcount = 10;
            const byte pos_repeated = 16;
            const byte pos_value = 32;
            const byte pos_validate = 48;

            const byte len_bitcount = 6;

            bool isarray = !seg.Repeated.IsIntOne();
            ulong token = isarray ? CO_SEGINTEGER_ARRAY : CO_SEGINTEGER;

            byte encoded = 0;
            switch (seg.Encoded)
            {
                case EncodedType.Primitive:
                    encoded = ENCO_PRIMITIVE;
                    break;
                case EncodedType.Complement:
                    encoded = ENCO_COMPLEMENT;
                    break;
                case EncodedType.Inversion:
                    encoded = ENCO_INVERSION;
                    break;
            }
            SetTokenValue(ref token, encoded, pos_encoded, 2);
            SetTokenValue(ref token, (byte)(seg.ByteOrder == ByteOrderType.Big ? 1 : 0), pos_byteorder, 1);
            SetTokenValue(ref token, (byte)(seg.Signed ? 1 : 0), pos_issigned, 1);
            SetTokenValue(ref token, (byte)(seg.BitCount), pos_bitcount, len_bitcount);
            if (isarray) SetTokenValue(ref token, LookUpExp(seg.Repeated, pre), pos_repeated, LEN_USHORT);
            SetTokenValue(ref token, LookUpExp(seg.Value, pre), pos_value, LEN_USHORT);
            if (seg.VCheck != CheckType.None)
            {
                if (seg.VCheckRangeBegin == null || seg.VCheckRangeBegin.Length == 0)
                    seg.VCheckRangeBegin = brotherlist[0].Name;
                if (seg.VCheckRangeEnd == null || seg.VCheckRangeEnd.Length == 0)
                    seg.VCheckRangeEnd = FindPreiousSegment(seg, brotherlist);
            }
            var refvalid = LookUpValidator(seg.VMax, seg.VMin, seg.VCheck, seg.VCheckRangeBegin, seg.VCheckRangeEnd, pre);
            SetTokenValue(ref token, refvalid, pos_validate, LEN_USHORT);

            return AddSegment(token, pre + "." + seg.Name);
        }

        //编译浮点数字段
        private ushort CompileSegment(FrameSegmentReal seg, string pre)
        {
            //byte FioType;
            //byte Encoded;
            //byte ByteOrder;
            //bool IsDouble;
            //ushort RefRepeated;
            //ushort RefValue;
            //ushort RefValidator;

            const byte pos_encoded = 6;
            const byte pos_byteorder = 7;
            const byte pos_isdouble = 9;
            //const byte not_used = 10;
            const byte pos_repeated = 16;
            const byte pos_value = 32;
            const byte pos_validate = 48;

            var isarray = seg.Repeated.IsIntOne();
            ulong token = isarray ? CO_SEGREAL_ARRAY : CO_SEGREAL;
            byte encoded = 0;
            switch (seg.Encoded)
            {
                case EncodedType.Primitive:
                    encoded = ENCO_PRIMITIVE;
                    break;
                case EncodedType.Complement:
                    encoded = ENCO_COMPLEMENT;
                    break;
                case EncodedType.Inversion:
                    encoded = ENCO_INVERSION;
                    break;
            }
            SetTokenValue(ref token, encoded, pos_encoded, 2);
            SetTokenValue(ref token, (byte)(seg.ByteOrder == ByteOrderType.Big ? 1 : 0), pos_byteorder, 1);
            SetTokenValue(ref token, (byte)(seg.IsDouble ? 1 : 0), pos_isdouble, 1);
            if (isarray) SetTokenValue(ref token, LookUpExp(seg.Repeated, pre), pos_repeated, LEN_USHORT);
            SetTokenValue(ref token, LookUpExp(seg.Value, pre), pos_value, LEN_USHORT);
            SetTokenValue(ref token, LookUpValidator(seg.VMax, seg.VMin, pre), pos_validate, LEN_USHORT);

            return AddSegment(token, pre + "." + seg.Name);
        }

        //编译文本字段
        private ushort CompileSegment(FrameSegmentText seg, string pre)
        {
            //byte FioType;
            //byte Encoded;
            //byte AlignedLen;
            //byte Tail;
            //byte ByteSize;
            //ushort RefRepeated;
            const byte pos_repeated = 32;
            const byte pos_bytesize = 48;
            var isarray = seg.Repeated.IsIntOne();

            ulong token = isarray ? CO_SEGREAL_ARRAY : CO_SEGREAL;
            if (isarray) SetTokenValue(ref token, LookUpExp(seg.Repeated, pre), pos_repeated, LEN_USHORT);
            SetTokenValue(ref token, LookUpExp(seg.ByteSize, pre), pos_bytesize, LEN_USHORT);
            //HACK TEXTSEGMENT
            return AddSegment(token, pre + "." + seg.Name);

        }


        #endregion

        #region --验证规则--


        //验证规则
        private ushort AddValidator(ulong token, ushort refprevious)
        {
            _validatorlist.Add(token);
            var refnew = (ushort)(_validatorlist.Count - 1);
            if (refprevious != 0)
            {
                var pre_token = _validatorlist[refprevious];
                SetTokenValue(ref pre_token, refnew, POS_VALIDATOR_NEXT, LEN_USHORT);
                _validatorlist[refprevious] = pre_token;
            }
            return refnew;
        }


        private ushort LookUpValidator(byte validortype, string value, ushort refprevious)
        {
            if (value == null || value.Length == 0) return 0;
            ulong token = validortype;
            SetTokenValue(ref token, LookUpExp(value), POS_VALIDATOR_VALUE, LEN_USHORT);
            return AddValidator(token, refprevious);
        }

        private ushort LookUpValidator(CheckType check, string checkbegin, string checkend, string pre, ushort refprevious)
        {
            if (check == CheckType.None) return 0;
            const byte pos_checktype = 6;
            const byte pos_checkbegin = 32;
            const byte pos_checkend = 48;
            ulong token = CO_VALIDATOR_CHECK;
            SetTokenValue(ref token, (byte)check, pos_checktype, LEN_BYTE);
            SetTokenValue(ref token, (byte)LookUpSegment(checkbegin, pre), pos_checkbegin, LEN_USHORT);
            SetTokenValue(ref token, (byte)LookUpSegment(checkend, pre), pos_checkend, LEN_USHORT);
            return AddValidator(token, refprevious);
        }

        private ushort LookUpValidator(string vmax, string vmin, string pre)
        {
            return LookUpValidator(vmax, vmin, CheckType.None, null, null, pre);
        }
        private ushort LookUpValidator(string vmax, string vmin, CheckType check, string checkbegin, string checkend, string pre)
        {
            var firstvalid = LookUpValidator(CO_VALIDATOR_MAX, vmax, 0);
            var lastvalid = firstvalid;
            if (firstvalid == 0)
                firstvalid = LookUpValidator(CO_VALIDATOR_MIN, vmin, 0);
            else
                lastvalid = LookUpValidator(CO_VALIDATOR_MIN, vmin, lastvalid);
            if (firstvalid == 0)
                firstvalid = LookUpValidator(check, checkbegin, checkend, pre, 0);
            else
                lastvalid = LookUpValidator(check, checkbegin, checkend, pre, lastvalid);
            return firstvalid;
        }

        #endregion

        #region --计算式--

        //计算式
        private ushort LookUpSegment(string name, string pre)
        {
            if (name == "this")
            {
                return _symbols[pre];
            }
            return _symbols[pre + "." + name];
        }

        private ushort LookUpExp(ulong v)
        {
            return LookUpExp((double)v);
        }

        private ushort LookUpExp(Exp ep, string pre)
        {
            if (ep == null || ep.IsIntZero()) return 0;
            ulong token = 0;
            const byte pos_left = 32;
            const byte pos_right = 48;
            switch (ep.Op)
            {
                case exptype.EXP_ADD:
                    token = EXP_ADD;
                    token |= ((ulong)(LookUpExp(ep.LeftExp, pre)) << pos_left);
                    token |= ((ulong)(LookUpExp(ep.RightExp, pre)) << pos_right);
                    break;
                case exptype.EXP_DIV:
                    token = EXP_DIV;
                    token |= ((ulong)(LookUpExp(ep.LeftExp, pre)) << pos_left);
                    token |= ((ulong)(LookUpExp(ep.RightExp, pre)) << pos_right);
                    break;
                case exptype.EXP_MUL:
                    token = EXP_MUL;
                    token |= ((ulong)(LookUpExp(ep.LeftExp, pre)) << pos_left);
                    token |= ((ulong)(LookUpExp(ep.RightExp, pre)) << pos_right);
                    break;
                case exptype.EXP_SUB:
                    token = EXP_SUB;
                    token |= ((ulong)(LookUpExp(ep.LeftExp, pre)) << pos_left);
                    token |= ((ulong)(LookUpExp(ep.RightExp, pre)) << pos_right);
                    break;
                case exptype.EXP_REAL:
                case exptype.EXP_INT:
                    token = EXP_NUMBER;
                    token |= ((ulong)(LookUpExp(ep.ConstStr)) << pos_left);
                    break;
                case exptype.EXP_ID:
                    token = EXP_REF_SEGMENT;
                    token |= ((ulong)(LookUpSegment(ep.ConstStr, pre)) << pos_left);
                    break;
                case exptype.EXP_BYTESIZEOF:
                    token = EXP_FUN_BYTESIZEOF;
                    token |= ((ulong)LookUpSegment(ep.ConstStr, pre) << pos_left);
                    break;
                default:
                    throw new Exception("Unknow");
            }
            _expression.Add(token);
            return (ushort)(_expression.Count - 1);
        }

        private ushort LookUpExp(double d)
        {
            var v = BitConverter.ToUInt64(BitConverter.GetBytes(d), 0);
            for (ushort i = 0; i < _constlist.Count; i++)
            {
                if (_constlist[i] == v) return i;
            }
            _constlist.Add(v);
            return (ushort)(_constlist.Count - 1);

        }

        private ushort LookUpExp(long v)
        {
            return LookUpExp((double)v);
        }

        private ushort LookUpExp(string constv)
        {
            if (constv == null || constv.Length == 0) return 0;

            if (Helper.ValidateIsReal(constv))
                return LookUpExp(Convert.ToDouble(constv));
            else if (Helper.ValidateIsInt(constv))
            {
                if (constv.StartsWith("-"))
                    return LookUpExp(Convert.ToInt64(constv));
                else
                    return LookUpExp(Convert.ToUInt64(constv));
            }
            throw new Exception("unknow");
        }


        #endregion

        #region --Helper--

        //设置ulong某位置的值
        private void SetTokenValue(ref ulong token, ushort value, byte pos_start, byte len)
        {
            token |= ((((ulong)value) << (64 - len)) >> (64 - len - pos_start));
        }

        //添加字段到符号表
        private ushort AddSegment(ulong token, string segfullname)
        {
            _segmentlist.Add(token);
            ushort id = (ushort)(_segmentlist.Count - 1);
            _symbols.Add(segfullname, id);
            return id;
        }

        //更新字段token
        private void UpdateSegmentToken(ushort idx, ulong newtoken)
        {
            _segmentlist[idx] = newtoken;
        }

        private void UpdateSegmentToken(ushort idx, ushort value, byte pos)
        {
            _segmentlist[idx] |= ((ulong)value << pos);
        }

        //取枚举项的数值
        private long GetEnumItemValue(string enumname, string itname)
        {
            foreach (var em in _pj.EnumdefList)
            {
                if (em.Name == enumname)
                    return GetEnumItemValue(em, itname);
            }
            throw new Exception("unknow");
        }

            private static long GetEnumItemValue(Enumdef em, string itname)
            {
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

            //查找字段
            private static string FindPreiousSegment(FrameSegmentBase who, IList<FrameSegmentBase> brother)
            {
                var ret = "";
                for (int i = 0; i < brother.Count - 1; i++)
                {
                    if (brother[i] == who)
                        return ret;
                    else
                        ret = brother[i].Name;
                }
                throw new Exception("unknow");
            }

            //查找引用的数据帧
            private static Frame FindFrame(string fname)
            {
                foreach (var fn in _pj.FrameList)
                    if (fn.Name == fname) return fn;
                throw new Exception("unknow");
            }

            //查找toenum属性
            private static string FindToEnum(string segname, IList<FrameSegmentBase> brotherlist)
            {
                foreach (var seg in brotherlist)
                {
                    if (seg.Name == segname)
                        return ((FrameSegmentInteger)seg).VToEnum;
                }
                throw new Exception("nukonw");
            }


        #endregion


        }
    
}
