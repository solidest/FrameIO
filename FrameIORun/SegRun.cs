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
        public SegRun(SegBlockInfo segi)
        {
            ValueType = segi.SegType;
            IsArray = !segi.Segment.Repeated.IsIntOne();
            RefSegBlock = segi;
        }
        public SegBlockType ValueType { get; set; }
        public bool IsArray { get; set; }
        public ulong IntValue { get; set; }
        public string TextValue { get; set; }
        public double RealValue { get; set; }
        public ulong[] IntArrayValue { get; set; }
        public string[] TextArrayValue { get; set; }
        public double[] RealArrayValue { get; set; }
        public SegBlockInfo RefSegBlock { get; set; }

        public SegRun NextRunSeg { get; set; }

        //填充本字段的值
        public void FillValue(byte[] buff, ref int startByte, ref int startBit,ICalcuValue ib)
        {
            //TODO 填充本字段值

            ib.AddIdSeg(RefSegBlock.FullName, this);
        }

        //取本字段的计算值
        public double GetEvalValue()
        {
            switch(ValueType)
            {
                case SegBlockType.Integer:
                    return IntValue;
                case SegBlockType.Real:
                    return RealValue;
            }
            Debug.Assert(false);
            return 0;
        }

    }


}
