using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal class ExpLongValue : IExpRun
    {
        private long _v;
        public ExpLongValue(long v)
        {
            _v = v;
        }

        public bool IsConst => true;

        public bool IsIntOne => (_v==1);

        public bool IsThis => false;

        public double GetDouble(IExpRunCtx ctx)
        {
            return _v;
        }

        public long GetLong(IExpRunCtx ctx)
        {
            return _v;
        }

        public bool TryGetDouble(IExpRunCtx ctx, ref double v)
        {
            v = _v;
            return true;
        }

        public bool TryGetLong(IExpRunCtx ctx, ref long v)
        {
            v = _v;
            return true;
        }
    }

    internal class ExpDoubleValue : IExpRun
    {
        private double _v;

        public ExpDoubleValue(double d)
        {
            _v = d;
        }

        public bool IsConst => true;

        public bool IsIntOne => (_v==1);

        public bool IsThis => false;

        public double GetDouble(IExpRunCtx ctx)
        {
            return _v;
        }

        public long GetLong(IExpRunCtx ctx)
        {
            return (long)_v;
        }

        public bool TryGetDouble(IExpRunCtx ctx, ref double v)
        {
            v = _v;
            return true;
        }

        public bool TryGetLong(IExpRunCtx ctx, ref long v)
        {
            v = (long)_v;
            return true;
        }
    }

    internal class ExpStringValue : IExpRun
    {
        private string _v;

        public ExpStringValue(string v)
        {
            _v = v;
        }

        public bool IsConst => false;

        public bool IsIntOne => false;

        public bool IsThis => false;

        public double GetDouble(IExpRunCtx ctx)
        {
            return ctx.GetDouble(_v);
        }

        public long GetLong(IExpRunCtx ctx)
        {
            return ctx.GetLong(_v);
        }

        public bool TryGetDouble(IExpRunCtx ctx, ref double v)
        {
            return ctx.TryGetDouble(_v, ref v);
        }

        public bool TryGetLong(IExpRunCtx ctx, ref long v)
        {
            return ctx.TryGetLong(_v, ref v);
        }
    }

    internal class ExpCalc : IExpRun
    {
        private ExpCalcType _t;
        private IExpRun _left;
        private IExpRun _right;

        public ExpCalc(ExpCalcType t, IExpRun left, IExpRun right)
        {
            _t = t;
            _left = left;
            _right = right;
        }

        public bool IsConst => (_left.IsConst && _right.IsConst);

        public bool IsIntOne => (IsConst && GetLong(null)==1);

        public bool IsThis => false;

        public double GetDouble(IExpRunCtx ctx)
        {
            switch (_t)
            {
                case ExpCalcType.EXP_ADD:
                    return _left.GetDouble(ctx) + _right.GetDouble(ctx);
                case ExpCalcType.EXP_SUB:
                    return _left.GetDouble(ctx) - _right.GetDouble(ctx);
                case ExpCalcType.EXP_MUL:
                    return _left.GetDouble(ctx) * _right.GetDouble(ctx);
                case ExpCalcType.EXP_DIV:
                    return _left.GetDouble(ctx) / _right.GetDouble(ctx);
            }
            throw new Exception("unknow");
        }

        public long GetLong(IExpRunCtx ctx)
        {
            return (long)GetDouble(ctx);
        }

        public bool TryGetDouble(IExpRunCtx ctx, ref double v)
        {
            double d1=0, d2=0;
            if (_left.TryGetDouble(ctx, ref d1) && _right.TryGetDouble(ctx, ref d2))
            {
                v = d1 + d2;
                return true;
            }

            return false;
        }

        public bool TryGetLong(IExpRunCtx ctx, ref long v)
        {
            double d = 0;
            if( TryGetDouble(ctx, ref d))
            {
                v = (long)d;
                return true;
            }

            return false;
        }
    }

    internal class ExpByteSizeOf : IExpRun
    {
        private string _seg;
        public ExpByteSizeOf(string seg)
        {
            _seg = seg;
        }
        public bool IsConst => false;

        public bool IsIntOne => false;

        public bool IsThis => (_seg == "this");

        public double GetDouble(IExpRunCtx ctx)
        {
            return GetLong(ctx);
        }

        public long GetLong(IExpRunCtx ctx)
        {
            return IsThis ? ctx.GetSizeOfThis() : ctx.GetSizeOfSegment(_seg);
        }

        public bool TryGetDouble(IExpRunCtx ctx, ref double v)
        {
            v = GetDouble(ctx);
            return true;
        }

        public bool TryGetLong(IExpRunCtx ctx, ref long v)
        {
            v = GetLong(ctx);
            return true;
        }
    }


    //表达式接口
    internal interface IExpRun
    {
        bool IsConst { get; }
        bool IsIntOne { get; }
        bool IsThis { get; }

        long GetLong(IExpRunCtx ctx);
        double GetDouble(IExpRunCtx ctx);

        bool TryGetLong(IExpRunCtx ctx, ref long v);
        bool TryGetDouble(IExpRunCtx ctx, ref double v);
    }

    //表达式执行环境接口
    internal interface IExpRunCtx
    {
        long GetLong(string id);
        double GetDouble(string id);

        bool TryGetLong(string id, ref long v);
        bool TryGetDouble(string id, ref double v);

        int GetStartPos(string seg);
        int GetEndPos(string seg);

        int GetSizeOfSegment(string seg);
        int GetSizeOfThis();
    }


    //计算类型
    internal enum ExpCalcType
    {
        EXP_ADD,
        EXP_SUB,
        EXP_MUL,
        EXP_DIV
    }

}
