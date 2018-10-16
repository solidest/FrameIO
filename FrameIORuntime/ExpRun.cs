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

        public double GetExpValue(IRunExp ir)
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
                case ExpType.EXP_REF_SEGMENT:
                    return ir.GetSegmentValue(_left);
                case ExpType.EXP_FUN_BYTESIZEOF:
                    return ir.GetSegmentByteSize(_left);
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


    public interface IRunExp:IRunInitial
    {
        double GetSegmentValue(ushort idx);
        int GetSegmentByteSize(ushort idx);
        ushort GetBitLen(ref int bitlen, ushort idx);

        double GetExpValue(ushort idx);


    }



}
