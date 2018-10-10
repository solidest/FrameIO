using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Main;

namespace FrameIO.Run
{
    //解包时字段
    public class SegRunUnpack : SegRun
    {
        public SegRunUnpack(SegBlockInfo segbi)
        {
            ValueType = segbi.SegType;
            IsArray = !segbi.Segment.Repeated.IsIntOne();
            RefSegBlock = segbi;
            if (segbi.IsFixed)
            {
                _bitsize = RefSegBlock.BitSizeNumber;
                _repeated = RefSegBlock.RepeatedNumber;
                IsArray = (_repeated == 1);
                _isSetSize = true;
            }
            else
            {
                _isSetSize = false;
                BitSize = ExpRun.CreateExpRun(segbi.BitSize, GetSegPreName(segbi.RefSegTree));
                Repeated = ExpRun.CreateExpRun(segbi.Repeated, GetSegPreName(segbi.RefSegTree));
            }
        }

        public SegRunUnpack NextRunSeg { get; set; }


        private int _bitsize;
        private int _repeated;
        private bool _isSetSize;

        public void Reset()
        {
            _isSetSize = false;
            if (RefSegBlock.IsFixed)
            {
                _bitsize = RefSegBlock.BitSizeNumber;
                _repeated = RefSegBlock.RepeatedNumber;
                _isSetSize = true;
            }
            else
            {
                _isSetSize = false;
            }
            NumberValue = 0;
            TextValue = null;
            NumberArrayValue = null;
            TextArrayValue = null;
            BitStart = -1;
        }

        //取本字段的总长度
        public int GetBitLen(IUnpackFrameRun ic)
        {
            if (!_isSetSize)
            {
                _bitsize = (int)BitSize.GetRealValue(ic);
                _repeated = (int)Repeated.GetRealValue(ic);
                _isSetSize = true;
            }
            return _bitsize * _repeated;
        }

        //从内存中读取本字段的值
        public void ReadValue(byte[] buff, ref int startBit, IUnpackFrameRun ib)
        {
            BitStart = startBit;

            int len = GetBitLen(ib);

            switch (ValueType)
            {
                case SegBlockType.Integer:
                    var seg1 = (FrameSegmentInteger)RefSegBlock.Segment;
                    var et1 = seg1.Encoded;
                    var or1 = seg1.ByteOrder;
                    if (IsArray) NumberArrayValue = new ulong[_repeated];
                    for (int i = 0; i < _repeated; i++)
                    {
                        var vl = GetUIntxFromByte(buff, (uint)startBit, _bitsize, et1, or1); 
                        startBit += _bitsize;
                        if (IsArray)
                            NumberArrayValue[i] = vl;
                        else
                            NumberValue = vl;
                    }
                    if (seg1.VCheck != CheckType.None) ValidateCheckSeg(buff, startBit, ib);
                    if (seg1.VMax != null || seg1.VMax != "") CheckMax(seg1.VMax);
                    if (seg1.VMin != null || seg1.VMin != "") CheckMin(seg1.VMin);
                    if (seg1.VToEnum != null || seg1.VToEnum != "") CheckToEnum(seg1.VToEnum);
                    break;

                case SegBlockType.Real:
                    var seg2 = (FrameSegmentInteger)RefSegBlock.Segment;
                    var et2 = seg2.Encoded;
                    var or2 = seg2.ByteOrder;
                    if (IsArray) NumberArrayValue = new ulong[_repeated];
                    for (int i = 0; i < _repeated; i++)
                    {
                        var vl = GetUIntxFromByte(buff, (uint)startBit, _bitsize, et2, or2);
                        startBit += _bitsize;
                        if (IsArray)
                            NumberArrayValue[i] = vl;
                        else
                            NumberValue = vl;
                    }
                    if (seg2.VMax != null || seg2.VMax != "") CheckMax(seg2.VMax);
                    if (seg2.VMin != null || seg2.VMin != "") CheckMin(seg2.VMin);
                    break;

                case SegBlockType.Text:
                    if (startBit % 8 != 0) throw new Exception("数据解析时出现非整字节");
                    if (IsArray) TextArrayValue = new byte[_repeated][];
                    int istart = startBit / 8;
                    for(int i=0; i<_repeated; i++)
                    {
                        var vbs = new byte[_bitsize / 8];
                        for(int ii=0; ii<_bitsize/8; ii++)
                        {
                            vbs[ii] = buff[istart];
                            istart += 1;
                        }
                        if (IsArray)
                            TextArrayValue[i] = vbs;
                        else
                            TextValue = vbs;
                    }
                    startBit += istart * 8;
                    break;
            }
            ib.AddUnpackSeg(RefSegBlock.FullName, this);
            Debug.Assert(startBit == (BitStart + GetBitLen(ib)));
        }

        //TODO 验证最大、最小、校验位、枚举等

        //检验最大值
        private void CheckMax(string max)
        {

        }

        //检验最小值
        private void CheckMin(string min)
        {

        }

        //检验toenum
        private void CheckToEnum(string toenum)
        {

        }

        //验证校验字段是否正确
        public void ValidateCheckSeg(byte[] buff, int endBit, IUnpackFrameRun ib)
        {
            int byteStart = 0;
            int byteEnd = endBit/8;
            if (RefSegBlock.CheckBeginSegs == null || RefSegBlock.CheckBeginSegs.Count == 0)
                byteStart = 0;
            else
            {
                foreach(var n in RefSegBlock.CheckBeginSegs)
                {
                    var segr = ib.FindUnpackSegRun(n);
                    if (segr!=null)
                    {
                        byteStart = segr.BitStart/8;
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
                    var segr = ib.FindUnpackSegRun(n);
                    if (segr != null)
                    {
                        byteEnd = (segr.BitStart + segr.GetBitLen(ib)) / 8;
                        break;
                    }
                }
            }

            //调用校验API

            ulong checkresult = 1;

            if (checkresult != NumberValue) throw new Exception("校验位验证失败");

        }


        //取本字段用于公式计算时的值
        public double GetEvalValue()
        {
            switch(ValueType)
            {
                case SegBlockType.Integer:
                    if (((FrameSegmentInteger)RefSegBlock.Segment).Signed)
                        return ConvertToLong(NumberValue, _bitsize);
                    else
                        return NumberValue;
                case SegBlockType.Real:
                    return _bitsize==32?(BitConverter.ToSingle(BitConverter.GetBytes((uint)NumberValue), 0)):(BitConverter.ToDouble(BitConverter.GetBytes(NumberValue), 0));
            }
            throw new Exception("错误的计算表达式");
        }


    }


}
