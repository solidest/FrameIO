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
    public class FrameCompileFile
    {
        public static Dictionary<string, CompiledFrame>  Compile(IList<Frame> fms, IOProject pj)
        {
            var ret = new Dictionary<string, CompiledFrame>();
            for (int i=0; i<fms.Count; i++)
            {
                var cp = new FrameCompiler();
                var cped = new CompiledFrame();
                cped.Content = cp.Compile(pj, fms[i], fms);
                cped.SymbolDic = cp.OutSymbols;
                ret.Add(fms[i].Name, cped);
            }
            return ret;
        }
    }

    public class FrameCompiler
    {
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
        const byte CO_SEGONEOFITEM_IN = 11;
        const byte CO_SEGONEOFITEM_OUT = 12;

        const byte CO_VALIDATOR_MAX = 1;
        const byte CO_VALIDATOR_MIN = 2;
        const byte CO_VALIDATOR_EQUAL= 3;
        const byte CO_VALIDATOR_CHECK = 4;

        const byte POS_VALIDATOR_VALUE = 32;
        const byte POS_VALIDATOR_NEXT = 48;

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


        private List<ulong> _segmentlist = new List<ulong>();
        private List<ulong> _constlist = new List<ulong>();
        private List<ulong> _expression = new List<ulong>();
        private List<ulong> _validatorlist = new List<ulong>();

        public Dictionary<string, ushort> OutSymbols { get; set; } = new Dictionary<string, ushort>();

        public FrameCompiler()
        {
            _segmentlist.Add(0);
            _constlist.Add(0);
            _validatorlist.Add(0);
            _expression.Add(0);
            OutSymbols.Add("", 0);
        }

        //编译数据帧
        private ICollection<Frame> _fms = null;
        private IOProject _pj = null;
        public byte[] Compile(IOProject pj, Frame fm, ICollection<Frame> fms)
        {
            _fms = fms;
            _pj = pj;
            for (int i = 0; i < fm.Segments.Count; i++)
            {
                CompileSegment(fm.Segments[i], "", fm.Segments);
            }
            _fms = null;
            _pj = null;

            //const byte pos_segment = 0;
            const byte pos_const = 16;
            const byte pos_expression = 32;
            const byte pos_validator = 48;
            if (_segmentlist.Count >= ushort.MaxValue || _constlist.Count >= ushort.MaxValue || _expression.Count >= ushort.MaxValue || _validatorlist.Count >= ushort.MaxValue)
                throw new Exception("outrange");
            ulong token= (ushort)_segmentlist.Count;
            SetTokenValue(ref token, (ushort)_constlist.Count, pos_const, LEN_USHORT);
            SetTokenValue(ref token, (ushort)_expression.Count, pos_expression, LEN_USHORT);
            SetTokenValue(ref token, (ushort)_validatorlist.Count, pos_validator, LEN_USHORT);

            ;
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

        //编译字段
        private ushort CompileSegment(FrameSegmentBase seg, string pre, IList<FrameSegmentBase> brotherlist)
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
                        return CompileSegment(bseg.RefFrameName, bseg, pre);
                    case BlockSegType.DefFrame:
                        return CompileSegment(bseg.DefineSegments, bseg, pre);
                    case BlockSegType.OneOf:
                        return CompileSegment(bseg.OneOfFromSegment, (ushort)bseg.OneOfCaseList.Count, bseg, pre,brotherlist);
                }
            }
            throw new Exception("unknow");
        }

        //编译OneOf字段
        private ushort CompileSegment(string bysegname, ushort caseitemcount, FrameSegmentBlock parent, string parent_pre, IList<FrameSegmentBase> brotherlist)
        {
            //const biyte pos_fiotype = 0;
            //const byte pos_not_used = 6;
            const byte pos_by_segment = 16;
            const byte pos_first_caseitem = 32;
            const byte pos_last_caseitem = 32;
            const byte pos_oneof_inseg = 48;
            const byte pos_oneof_outseg = 48;

            var need_update_end = new List<ushort>();
            var pre = parent_pre + "." + parent.Name;
            ulong token1 = CO_SEGONEOF_IN;
            var reftoken1 = AddSegment(token1, pre);

            var byenum = FindToEnum(bysegname, brotherlist);
            var byseg = LookUpSegment(bysegname, parent_pre);
            var intovalue = LookUpExp(GetEnumItemValue(byenum, parent.OneOfCaseList[0].EnumItem));
            var firstitem = CompileSegment(LookUpExp(intovalue), byseg, parent.OneOfCaseList[0].FrameName, pre + "." +  parent.OneOfCaseList[0].EnumItem, true, false, need_update_end);
            var lastitem = firstitem;
            for(int i=1; i<parent.OneOfCaseList.Count; i++)
            {
                lastitem = CompileSegment(LookUpExp(intovalue), byseg, parent.OneOfCaseList[i].FrameName, pre + "." + parent.OneOfCaseList[i].EnumItem,false, i==parent.OneOfCaseList.Count-1, need_update_end);
            }
            ulong token2 = CO_SEGONEOF_OUT;
            var reftoken2 = AddSegment(token2, pre + "$");

            SetTokenValue(ref token2, byseg, pos_by_segment, LEN_USHORT);
            SetTokenValue(ref token2, lastitem, pos_last_caseitem, LEN_USHORT);
            SetTokenValue(ref token2, reftoken1, pos_oneof_inseg, LEN_USHORT);
            UpdateSegmentToken(reftoken2, token2);

            SetTokenValue(ref token1, byseg, pos_by_segment, LEN_USHORT);
            SetTokenValue(ref token1, firstitem, pos_first_caseitem, LEN_USHORT);
            SetTokenValue(ref token1, reftoken2, pos_oneof_outseg, LEN_USHORT);
            UpdateSegmentToken(reftoken1, token1);

            const byte pos2_parent_outseg = 48;
            for (int i=0; i<need_update_end.Count-1; i++)
            {
                UpdateSegmentToken(need_update_end[i], reftoken2, pos2_parent_outseg);
            }

            return reftoken1;
        }

        //编译OneOf分支
        private ushort CompileSegment(ushort intovalue, ushort bysegment, string framename, string parent_pre, bool isLast, bool isFirst, IList<ushort> need_update_end)
        {
            const byte pos_isLast = 6;
            const byte pos_isFirst = 7;
            //const biyte pos_fiotype = 0;
            //const byte pos_not_used = 8;

            //into segment
            const byte pos1_my_outseg = 16;
            const byte pos1_into_value = 32;
            const byte pos1_by_segv = 48;


            //out
            const byte pos2_my_intoseg = 16;
            //const byte pos2_notused = 32;
            //const byte pos2_parent_outseg = 48;

            ulong token1 = CO_SEGONEOFITEM_IN;
            var reftoken1 = AddSegment(token1, parent_pre);
            var fm = FindFrame(framename);
            var reffirst = CompileSegment(fm.Segments[0], parent_pre, fm.Segments);
            var reflast = reffirst;
            for (int i = 1; i < fm.Segments.Count; i++)
                reflast = CompileSegment(fm.Segments[i], parent_pre, fm.Segments);


            ulong token2 = CO_SEGONEOFITEM_OUT;
            if (isFirst) SetTokenValue(ref token2, 1, pos_isFirst, 1);
            if (isLast) SetTokenValue(ref token2, 1, pos_isLast, 1);
            SetTokenValue(ref token2, reftoken1, pos2_my_intoseg, LEN_USHORT);
            var reftoken2 = AddSegment(token2, parent_pre + "$");
            need_update_end.Add(reftoken2);

            if (isFirst) SetTokenValue(ref token1, 1, pos_isFirst, 1);
            if (isLast) SetTokenValue(ref token1, 1, pos_isLast, 1);
            SetTokenValue(ref token1, reftoken2, pos1_my_outseg, LEN_USHORT);
            SetTokenValue(ref token1, intovalue, pos1_into_value, LEN_USHORT);
            SetTokenValue(ref token1, bysegment, pos1_by_segv, LEN_USHORT);
            UpdateSegmentToken(reftoken1, token1);

            return reftoken1;
        }

        //编译引用数据帧字段
        private ushort CompileSegment(string framename, FrameSegmentBlock parent, string parent_pre)
        {
            var f = FindFrame(framename);
            return CompileSegment(f.Segments, parent, parent_pre);
        }

        //编译内联定义字段
        private ushort CompileSegment(IList<FrameSegmentBase> children, FrameSegmentBlock parent, string parent_pre)
        {
            //const char pos_fiotype = 0;
            const byte pos_first_childseg = 6;
            const byte pos_last_childseg = 22;
            const byte pos_in_seg =  38;
            const byte pos_out_seg = 38;

            ulong token1 = CO_SEGBLOCK_IN;
            var pre = parent_pre + "." + parent.Name;
            var pid_in = AddSegment(token1, pre);
            var first_seg_id = CompileSegment(children[0], pre, children);
            ushort last_seg_id = first_seg_id;
            for (int i=1; i<children.Count; i++)
            {
                last_seg_id = CompileSegment(children[i], pre, children);
            }

            ulong token2 = CO_SEGBLOCK_OUT;
            token2 |= (ulong)first_seg_id << pos_first_childseg;
            token2 |= (ulong)last_seg_id << pos_last_childseg;
            token2 |= (ulong)pid_in << pos_in_seg;
            var pid_out = AddSegment(token2, pre+"$");

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
            SetTokenValue(ref token, (byte)(seg.ByteOrder == ByteOrderType.Big?1:0), pos_byteorder, 1);
            SetTokenValue(ref token, (byte)(seg.Signed ? 1:0), pos_issigned, 1);
            SetTokenValue(ref token, (byte)(seg.BitCount), pos_bitcount, len_bitcount);
            if(isarray) SetTokenValue(ref token, LookUpExp(seg.Repeated, pre), pos_repeated, LEN_USHORT);
            SetTokenValue(ref token, LookUpExp(seg.Value, pre), pos_value, LEN_USHORT);
            if(seg.VCheck!= CheckType.None)
            {
                if (seg.VCheckRangeBegin == null || seg.VCheckRangeBegin.Length == 0)
                    seg.VCheckRangeBegin = brotherlist[0].Name;
                if (seg.VCheckRangeEnd == null || seg.VCheckRangeEnd.Length == 0)
                    seg.VCheckRangeEnd = FindPreiousSegment(seg, brotherlist);
            }
            var refvalid =LookUpValidator(seg.VMax, seg.VMin, seg.VCheck, seg.VCheckRangeBegin, seg.VCheckRangeEnd, pre);
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
            ulong token = isarray ?  CO_SEGREAL_ARRAY : CO_SEGREAL;
            byte encoded = 0;
            switch (seg.Encoded)
            {
                case EncodedType.Primitive:
                    encoded  = ENCO_PRIMITIVE;
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
            if(isarray) SetTokenValue(ref token, LookUpExp(seg.Repeated, pre), pos_repeated, LEN_USHORT);
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
            if(isarray) SetTokenValue(ref token, LookUpExp(seg.Repeated, pre), pos_repeated, LEN_USHORT);
            SetTokenValue(ref token, LookUpExp(seg.ByteSize, pre), pos_bytesize, LEN_USHORT);
            //HACK TEXTSEGMENT
            return AddSegment(token, pre + "." + seg.Name);

        }

        //验证规则
        private ushort AddValidator(ulong token, ushort refprevious)
        {
            _validatorlist.Add(token);
            var refnew = (ushort)(_validatorlist.Count - 1);
            if(refprevious!=0)
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
            if(firstvalid ==0)
                firstvalid = LookUpValidator(CO_VALIDATOR_MIN, vmin, 0);
            else
                lastvalid = LookUpValidator(CO_VALIDATOR_MIN, vmin, lastvalid);
            if (firstvalid == 0)
                firstvalid = LookUpValidator(check, checkbegin, checkend, pre, 0);
            else
                lastvalid = LookUpValidator(check, checkbegin, checkend, pre, lastvalid);
            return firstvalid;
        }

        //计算式
        private ushort LookUpSegment(string name, string pre)
        {
            if(name=="this")
            {
                return OutSymbols[pre];
            }
            return OutSymbols[pre + "." +name];
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
            switch(ep.Op)
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

            if(Helper.ValidateIsReal(constv))
                return LookUpExp(Convert.ToDouble(constv));
            else if(Helper.ValidateIsInt(constv))
            {
                if (constv.StartsWith("-"))
                    return LookUpExp(Convert.ToInt64(constv));
                else
                    return LookUpExp(Convert.ToUInt64(constv));
            }
            throw new Exception("unknow");
        }

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
            OutSymbols.Add(segfullname, id);
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

        private long GetEnumItemValue(Enumdef em, string itname)
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
        private string FindPreiousSegment(FrameSegmentBase who, IList<FrameSegmentBase> brother)
        {
            var ret = "";
            for(int i=0; i<brother.Count-1; i++)
            {
                if (brother[i] == who)
                    return ret;
                else
                    ret = brother[i].Name;
            }
            throw new Exception("unknow");
        }

        //查找引用的数据帧
        private Frame FindFrame(string fname)
        {
            foreach (var fn in _fms)
                if (fn.Name == fname) return fn;
            throw new Exception("unknow");
        }

        //查找toenum属性
        private string FindToEnum(string segname, IList<FrameSegmentBase> brotherlist)
        {
            foreach(var seg in brotherlist)
            {
                if (seg.Name == segname)
                    return ((FrameSegmentInteger)seg).VToEnum;
            }
            throw new Exception("nukonw");
        }

    }

    public class CompiledFrame
    {
        public Dictionary<string, ushort> SymbolDic { get; set; }
        public byte[] Content { get; set; }
    }
}
