using FrameIO.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    public class SegRunPack : SegRun
    {
        public SegRunPack(SegBlockInfo segbi)
        {
            ValueType = segbi.SegType;
            IsArray = !segbi.Segment.Repeated.IsIntOne();
            RefSegBlock = segbi;

            _bitsize = segbi.BitSizeNumber;

            var pre = GetSegPreName(segbi.RefSegTree);
            BitSize = ExpRun.CreateExpRun(segbi.BitSize, pre);
            Repeated = ExpRun.CreateExpRun(segbi.Repeated, pre);

        }

        public bool IsSetValue { get ; private set; }   //是否设置了本字段的值

        private int _bitsize;                           //固定的字段位长


        //重置
        public void Reset()
        {
            IsSetValue = false;

            NumberValue = 0;
            TextValue = null;
            NumberArrayValue = null;
            TextArrayValue = null;
            BitStart = -1;
        }

        //写入本字段的值 返回提交的整字节数
        public void WriteValue(MemoryStream commit, ref int commit_bytelen, ref ulong cach, ref int cach_pos, IPackFrameRun ir)
        {
            BitStart = commit_bytelen * 8 + cach_pos;
            int commit_b = 0;

            if (!IsSetValue)
            {
                if (cach_pos % 8 != 0) throw new Exception("存在未设置值的非整字节字段");
                int cb = cach_pos / 8;
                if(cb>0)
                {
                    commit.Write(BitConverter.GetBytes(cach), 0, cb);
                    cach = 0;
                    cach_pos = 0;
                    commit_bytelen += cb;
                }
                SetAutoValue(commit.GetBuffer(), commit_bytelen*8, ir);
            }

            switch (ValueType)
            {
                case SegBlockType.Integer:
                    {
                        var seg = (FrameSegmentInteger)RefSegBlock.Segment;
                        if (IsArray)
                        {
                            for (int i = 0; i < NumberArrayValue.Length; i++)
                                commit_b += WriteValue(commit, ref cach, ref cach_pos, NumberArrayValue[i], _bitsize, seg.Encoded, seg.ByteOrder);
                        }
                        else
                            commit_b += WriteValue(commit, ref cach, ref cach_pos, NumberValue, _bitsize, seg.Encoded, seg.ByteOrder);
                        break;
                    }


                case SegBlockType.Real:
                    {
                        var seg = (FrameSegmentReal)RefSegBlock.Segment;
                        if (IsArray)
                        {
                            for (int i = 0; i < NumberArrayValue.Length; i++)
                                commit_b += WriteValue(commit, ref cach, ref cach_pos, NumberArrayValue[i], _bitsize, seg.Encoded, seg.ByteOrder);
                        }
                        else
                            commit_b += WriteValue(commit, ref cach, ref cach_pos, NumberValue, _bitsize, seg.Encoded, seg.ByteOrder);
                        break;
                    }

                case SegBlockType.Text:
                    //HACK 
                    break;
            }

            commit_bytelen += commit_b;
        }

        //写入一个指定长度的值，返回提交的整字节数
        private int WriteValue(MemoryStream commit, ref ulong cach, ref int cach_pos, ulong value,  int size, EncodedType encode, ByteOrderType byor)
        {
            //TODO 处理端序与编码

            int newpos = cach_pos + size;
            if(newpos<64)
            {
                cach |= (value << cach_pos);
                cach &= (~(ulong)0) >> (64- newpos);
                cach_pos += size;
                return 0;
            }
            else
            {
                ulong cv = cach | (value << cach_pos);
                commit.Write(BitConverter.GetBytes(cv), 0, 8);
                cach = value >> (64-cach_pos);
                cach_pos = newpos - 64;
                return 8;
            }
        }

        //自动计算字段值
        private void SetAutoValue(byte[] buff, int endBit, IPackFrameRun ir)
        {
            switch(ValueType)
            {
                case SegBlockType.Integer:
                    var seg1 = (FrameSegmentInteger)RefSegBlock.Segment;
                    if(seg1.VCheck != CheckType.None)
                    {
                        FillCheckSeg(buff, endBit, ir);
                    }
                    else
                    {
                        if (IsArray)
                        {
                            SetNumberArraySize((int)Repeated.GetRealValue(ir));
                        }
                        else
                            SetNumberValue((ulong)ExpRun.CreateExpRun(seg1.Value, GetSegPreName(RefSegBlock.RefSegTree)).GetRealValue(ir));
                    }
                    break;

                case SegBlockType.Real:
                    var seg2 = (FrameSegmentReal)RefSegBlock.Segment;
                    if (IsArray)
                    {
                        SetNumberArraySize((int)Repeated.GetRealValue(ir));
                    }
                    else
                    {
                        var dv = ExpRun.CreateExpRun(seg2.Value, GetSegPreName(RefSegBlock.RefSegTree)).GetRealValue(ir);
                        var vl = _bitsize == 32 ? BitConverter.ToUInt32(BitConverter.GetBytes((float)dv), 0) : BitConverter.ToUInt64(BitConverter.GetBytes(dv), 0);
                        SetNumberValue(vl);
                    }
                    break;
                case SegBlockType.Text:
                    if (IsArray)
                    {
                        SetTextArraySize((int)Repeated.GetRealValue(ir));
                    }
                    else
                        SetTextValue(null);
                    break;
            }
        }


        //填充校验值
        public void FillCheckSeg(byte[] buff, int endBit, IPackFrameRun ib)
        {
            int byteStart = 0;
            int byteEnd = endBit / 8;
            if (RefSegBlock.CheckBeginSegs == null || RefSegBlock.CheckBeginSegs.Count == 0)
                byteStart = 0;
            else
            {
                foreach (var n in RefSegBlock.CheckBeginSegs)
                {
                    var segr = ib.FindPackSegRun(n);
                    if (segr.IsSetValue)
                    {
                        byteStart = segr.BitStart / 8;
                        break;
                    }
                }
            }
            if (RefSegBlock.CheckEndSegs == null || RefSegBlock.CheckEndSegs.Count == 0)
                byteEnd = 0;
            else
            {
                foreach (var n in RefSegBlock.CheckEndSegs)
                {
                    var segr = ib.FindPackSegRun(n);
                    if (segr.IsSetValue)
                    {
                        byteEnd = (segr.BitStart + segr.GetBitLen(ib)) / 8;
                        break;
                    }
                }
            }

            //TODO 调用校验API
            ulong checkresult = 0;

            SetNumberValue(checkresult);

        }


        //取本字段的总位长度
        public int GetBitLen(IPackFrameRun ic)
        {
            if (RefSegBlock.IsFixed)
                return RefSegBlock.BitSizeNumber * RefSegBlock.RepeatedNumber;
            else
            {
                if (IsSetValue)
                {
                    switch(ValueType)
                    {
                        case SegBlockType.Integer:
                        case SegBlockType.Real:
                            return _bitsize * NumberArrayValue.Length;
                        case SegBlockType.Text:
                            var ret = 0;
                            foreach(var t in TextArrayValue)
                            {
                                ret += t.Length * 8;
                            }
                            return ret;
                    }
                    throw new Exception("意外读取空字段的长度");
                }
                else
                    throw new Exception("意外读取空字段的长度");
            }
        }

        //取字段的计算值
        public double GetEvalValue()
        {
            if (!IsSetValue) return 0;
            switch (ValueType)
            {
                case SegBlockType.Integer:
                    if (((FrameSegmentInteger)RefSegBlock.Segment).Signed)
                        return ConvertToLong(NumberValue, _bitsize);
                    else
                        return NumberValue;
                case SegBlockType.Real:
                    return _bitsize == 32 ? (BitConverter.ToSingle(BitConverter.GetBytes((uint)NumberValue), 0)) : (BitConverter.ToDouble(BitConverter.GetBytes(NumberValue), 0));
            }
            throw new Exception("错误的计算表达式");
        }

        //设置字段的值
        public void SetNumberValue(ulong b)
        {
            if (ValueType == SegBlockType.Text || IsArray) throw new Exception("设置字段值类型不匹配");
            IsSetValue = true;
            NumberValue = b;
        }

        //设置字段数组的值
        public void SetNumberArraySize(int size)
        {
            if(ValueType == SegBlockType.Text || !IsArray) throw new Exception("设置字段值类型不匹配");
            if(Repeated.IsConst())
            {
                var rep = (int)Repeated.GetConstValue();
                if(rep != size) throw new Exception("设置字段数组值长度不匹配");
            }
            IsSetValue = true;
            NumberArrayValue = new ulong[size];
        }

        //设置字段数组的值
        public void SetNumberArrayAt(int idx, ulong v)
        {
            NumberArrayValue[idx] = v;
        }

        //设置text的值
        public void SetTextValue(byte[] value)
        {
            TextValue = value;
            IsSetValue = true;
        }

        //设置text数组的值
        public void SetTextArraySize(int size)
        {
            TextArrayValue = new byte[size][];
            IsSetValue = true;
       }
    }
}
