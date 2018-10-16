using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    public class ExpRun
    {
        const byte pos_left = 32;
        const byte pos_right = 48;
        private ExpType _optype;
        private ushort _left;
        private ushort _right;


        public ExpRun(ulong token)
        {
            _optype = (ExpType)SegmentBaseRun.GetTokenByte(token, 0, 8);
            _left = SegmentBaseRun.GetTokenUShort(token, pos_left);
            _right = SegmentBaseRun.GetTokenUShort(token, pos_right);
        }

        public bool IsConstOne(IRunInitial ir)
        {
            return (_optype == ExpType.EXP_NUMBER && ir.GetConst(_left) ==1);
        }

        public bool IsConst(IRunInitial ir)
        {
            switch (_optype)
            {
                case ExpType.EXP_NUMBER:
                    return true;
                case ExpType.EXP_ADD:
                case ExpType.EXP_SUB:
                case ExpType.EXP_MUL:
                case ExpType.EXP_DIV:
                    return ir.IsConst(_left) && ir.IsConst(_right);
            }
            return false;
        }

        public double GetConstValue(IRunInitial ir)
        {
            switch (_optype)
            {
                case ExpType.EXP_NUMBER:
                    return ir.GetConst(_left);
                case ExpType.EXP_ADD:
                    return ir.GetConst(_left) + ir.GetConst(_right);
                case ExpType.EXP_SUB:
                    return ir.GetConst(_left) - ir.GetConst(_right);
                case ExpType.EXP_MUL:
                    return ir.GetConst(_left) * ir.GetConst(_right);
                case ExpType.EXP_DIV:
                    return ir.GetConst(_left) / ir.GetConst(_right);
            }
            throw new Exception("runtime");
        }

        public double GetExpValue(IPackRunExp ir)
        {
            switch (_optype)
            {
                case ExpType.EXP_NUMBER:
                    return ir.GetConst(_left);
                case ExpType.EXP_ADD:
                    return ir.GetExpValue(_left) + ir.GetExpValue(_right);
                case ExpType.EXP_SUB:
                    return ir.GetExpValue(_left) - ir.GetExpValue(_right);
                case ExpType.EXP_MUL:
                    return ir.GetExpValue(_left) * ir.GetExpValue(_right);
                case ExpType.EXP_DIV:
                    return ir.GetExpValue(_left) / ir.GetExpValue(_right);
                case ExpType.EXP_REF_SEGMENT:
                    return ir.GetSegmentValue(_left);
                case ExpType.EXP_FUN_BYTESIZEOF:
                    return ir.GetSegmentByteSize(_left);
            }
            throw new Exception("runtime");
        }

        public bool TryGetExpValue(ref double value, IUnpackRunExp ir)
        {
            double d1=0, d2=0;

            switch (_optype)
            {
                case ExpType.EXP_NUMBER:
                    value = ir.GetConst(_left);
                    return true;
                case ExpType.EXP_ADD:
                    if (ir.TryGetExpValue(ref d1, _left) && ir.TryGetExpValue(ref d2, _right))
                    {
                        value = d1 + d2;
                        return true;
                    }
                    else
                        return false;
                case ExpType.EXP_SUB:
                    if (ir.TryGetExpValue(ref d1, _left) && ir.TryGetExpValue(ref d2, _right))
                    {
                        value = d1 - d2;
                        return true;
                    }
                    else
                        return false;
                case ExpType.EXP_MUL:
                    if (ir.TryGetExpValue(ref d1, _left) && ir.TryGetExpValue(ref d2, _right))
                    {
                        value = d1 * d2;
                        return true;
                    }
                    else
                        return false;
                case ExpType.EXP_DIV:
                    if (ir.TryGetExpValue(ref d1, _left) && ir.TryGetExpValue(ref d2, _right))
                    {
                        value = d1 / d2;
                        return true;
                    }
                    else
                        return false;
                case ExpType.EXP_REF_SEGMENT:
                    return ir.TryGetSegmentValue(ref value, _left);
                case ExpType.EXP_FUN_BYTESIZEOF:
                    return ir.TryGetSegmentByteSize(ref value, _left);
            }
            throw new Exception("runtime");
        }

        //const byte EXP_NUMBER = 1;
        //const byte EXP_ADD = 2;
        //const byte EXP_SUB = 3;
        //const byte EXP_MUL = 4;
        //const byte EXP_DIV = 5;
        //const byte EXP_REF_SEGMENT = 6;
        //const byte EXP_FUN_BYTESIZEOF = 7;
        public enum ExpType
        {
            EXP_NUMBER = 1,
            EXP_ADD = 2,
            EXP_SUB = 3,
            EXP_MUL = 4,
            EXP_DIV = 5,
            EXP_REF_SEGMENT = 6,
            EXP_FUN_BYTESIZEOF = 7
        }

    }




}
