using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Main;

namespace FrameIO.Run
{
    //运行时字段信息
    public class SegRun
    {
        public SegRun(SegBlockInfo segbi)
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
        public SegBlockType ValueType { get; set; }
        public bool IsArray { get; set; }
        public ulong NumberValue { get; private set; }
        public byte[] TextValue { get; private set; }
        public ulong[] NumberArrayValue { get; private set; }
        public byte[][] TextArrayValue { get; private set; }
        public SegBlockInfo RefSegBlock { get; set; }
        public SegRun NextRunSeg { get; set; }
        public ExpRun BitSize { get; private set; }
        public ExpRun Repeated { get; private set; }
        public int BitStart { get => _bitstart; }

        private int _bitsize;
        private int _repeated;
        private int _bitstart;          //内存总的开始比特位置
        private bool _isSetSize;

        //取本字段的总长度
        public int GetBitLen(ISegRun ic)
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
        public void ReadValue(byte[] buff, ref int startBit, ISegRun ib)
        {
            _bitstart = startBit;

            int len = GetBitLen(ib);

            switch (ValueType)
            {
                case SegBlockType.Integer:
                    var seg = (FrameSegmentInteger)RefSegBlock.Segment;
                    var et1 = seg.Encoded;
                    if (IsArray) NumberArrayValue = new ulong[_repeated];
                    for (int i = 0; i < _repeated; i++)
                    {
                        var vl = GetUIntxFromByte(buff, (uint)startBit, _bitsize, et1); 
                        startBit += _bitsize;
                        if (IsArray)
                            NumberArrayValue[i] = vl;
                        else
                            NumberValue = vl;
                    }
                    if (seg.VCheck != CheckType.None) HandleCheckSeg(buff, startBit, ib);
                    break;

                case SegBlockType.Real:
                    var et2 = ((FrameSegmentInteger)RefSegBlock.Segment).Encoded;
                    if (IsArray) NumberArrayValue = new ulong[_repeated];
                    for (int i = 0; i < _repeated; i++)
                    {
                        var vl = GetUIntxFromByte(buff, (uint)startBit, _bitsize, et2);
                        startBit += _bitsize;
                        if (IsArray)
                            NumberArrayValue[i] = vl;
                        else
                            NumberValue = vl;
                    }
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
            ib.AddIdSeg(RefSegBlock.FullName, this);
            Debug.Assert(startBit == (_bitstart + GetBitLen(ib)));
        }

        //验证校验字段是否正确
        public void HandleCheckSeg(byte[] buff, int endBit, ISegRun ib)
        {
            int byteStart = 0;
            int byteEnd = endBit/8;
            if (RefSegBlock.CheckBeginSegs == null || RefSegBlock.CheckBeginSegs.Count == 0)
                byteStart = 0;
            else
            {
                foreach(var n in RefSegBlock.CheckBeginSegs)
                {
                    var segr = ib.FindSegRun(n);
                    if (segr!=null)
                    {
                        byteStart = segr.BitStart/8;
                    }
                }
            }
            if (RefSegBlock.CheckEndSegs == null || RefSegBlock.CheckEndSegs.Count == 0)
                byteStart = 0;
            else
            {
                foreach (var n in RefSegBlock.CheckEndSegs)
                {
                    var segr = ib.FindSegRun(n);
                    if (segr != null)
                    {
                        byteStart = (segr.BitStart + segr.GetBitLen(ib)) / 8;
                    }
                }
            }

            //TODO 调用校验API
            ulong checkresult = 1;

            if (checkresult != NumberValue) throw new Exception("校验位验证失败");

        }


        //取本字段的计算值
        public double GetEvalValue()
        {
            switch(ValueType)
            {
                case SegBlockType.Integer:
                    return NumberValue;
                case SegBlockType.Real:
                    return _bitsize==32?(BitConverter.ToSingle(BitConverter.GetBytes((uint)NumberValue), 0)):(BitConverter.ToDouble(BitConverter.GetBytes(NumberValue), 0));
            }
            Debug.Assert(false);
            return 0;
        }

        #region --Helper-

        //取任意位的字节
        static public ulong GetUInt64FromByte(byte[] buff, uint bitStart)
        {
            uint word_index = bitStart >> 6;
            uint word_offset = bitStart & 63;
            ulong result = BitConverter.ToUInt64(buff, (int)word_index * 8) >> (UInt16)word_offset;
            uint bits_taken = 64 - word_offset;
            if (word_offset > 0 && bitStart + bits_taken < (uint)(8 * buff.Length))
            {
                result |= BitConverter.ToUInt64(buff, (int)(word_index + 1) * 8) << (UInt16)(64 - word_offset);
            }
            return result;
        }

        //取任意位的指定长度字节
        static public ulong GetUIntxFromByte(byte[] buff, uint bitStart, int x, EncodedType et)
        {
            return GetUInt64FromByte(buff, bitStart) & ((x != 0) ? (~(ulong)0 >> (sizeof(ulong) * 8 - x)) : (ulong)0);
            //TODO 处理反码与补码
        }


        //取字段前缀名
        static private string GetSegPreName(SegTreeInfo segi)
        {
            segi = segi.Parent;
            if (segi == null) return "";
            return GetSegFullName(segi);
        }

        //取字段全名
        static private string GetSegFullName(SegTreeInfo segi)
        {
            var ret = segi.Name;
            while (segi.Parent != null)
            {
                ret = segi.Parent.Name + "." + ret;
                segi = segi.Parent;
            }
            return ret;
        }

        #endregion

    }


}
