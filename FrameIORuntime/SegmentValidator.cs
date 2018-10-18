using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{

    //字段验证
    internal class SegmentValidator
    {

        internal static SegmentValidator GetSegmentValidator(ulong token, IRunInitial ir)
        {
            if (token == 0) return null;
            const byte CO_VALIDATOR_MAX = 1;
            const byte CO_VALIDATOR_MIN = 2;
            const byte CO_VALIDATOR_EQUAL = 3;
            const byte CO_VALIDATOR_CHECK = 4;

            const byte POS_VALIDATOR_VALUE = 32;
            const byte POS_VALIDATOR_NEXT = 48;

            const byte pos_checktype = 6;
            const byte pos_checkbegin = 16;
            const byte pos_checkend = 32;

            byte type= SegmentBaseRun.GetTokenByte(token, 0, 6);
            switch (type)
            {
                case CO_VALIDATOR_MAX:
                    return new SegmentMaxValidator(ir.GetConst(SegmentBaseRun.GetTokenUShort(token, POS_VALIDATOR_VALUE)), SegmentBaseRun.GetTokenUShort(token, POS_VALIDATOR_NEXT));
                case CO_VALIDATOR_MIN:
                    return new SegmentMinValidator(ir.GetConst(SegmentBaseRun.GetTokenUShort(token, POS_VALIDATOR_VALUE)), SegmentBaseRun.GetTokenUShort(token, POS_VALIDATOR_NEXT));
                case CO_VALIDATOR_EQUAL:
                    return new SegmentEqualValidator(ir.GetConst(SegmentBaseRun.GetTokenUShort(token, POS_VALIDATOR_VALUE)), SegmentBaseRun.GetTokenUShort(token, POS_VALIDATOR_NEXT));
                case CO_VALIDATOR_CHECK:
                    return new SegmentCheckValidator(SegmentBaseRun.GetTokenByte(token, pos_checktype, 6), SegmentBaseRun.GetTokenUShort(token, pos_checkbegin), SegmentBaseRun.GetTokenUShort(token, pos_checkend), SegmentBaseRun.GetTokenUShort(token, POS_VALIDATOR_NEXT));
                default:
                    break;
            }
            throw new Exception("runtime");
        }

        internal SegmentValidator(ushort next_idx, ValidateType type)
        {
            NextIdx = next_idx;
            ValidType = type;
        }

        internal ValidateType ValidType { get; private set; }
        internal ushort NextIdx { get; private set; }
    }


    //最大值验证
    internal class SegmentMaxValidator: SegmentValidator
    {
        private double _max;
        internal SegmentMaxValidator(double maxv, ushort next_idx):base(next_idx, ValidateType.Max)
        {
            _max = maxv;
        }
    }

    //最小值验证
    internal class SegmentMinValidator : SegmentValidator
    {
        private double _min;
        internal SegmentMinValidator(double minv, ushort next_idx) : base(next_idx, ValidateType.Min)
        {
            _min = minv;
        }
    }

    //相等值验值
    internal class SegmentEqualValidator : SegmentValidator
    {
        private double _value;
        internal SegmentEqualValidator(double value, ushort next_idx) : base(next_idx, ValidateType.ToEnum)
        {
            _value = value;
        }
    }


    //校验值验值
    internal class SegmentCheckValidator : SegmentValidator
    {
        private byte _checktype;
        private ushort _checkbegin;
        private ushort _checkend;
        public SegmentCheckValidator(byte checktype, ushort checkbeing, ushort checkend, ushort next_idx) : base(next_idx, ValidateType.Check)
        {
            _checktype = checktype;
            _checkbegin = checkbeing;
            _checkend = checkend;
        }

        internal ulong GetCheckValue(byte[] buff, IPackRunExp ir)
        {
            return 0;
        }
    }

    //字段验证规则类型
    internal enum ValidateType
    {
        Check = 1,
        Max = 2,
        Min = 4,
        ToEnum = 8
    }


}
