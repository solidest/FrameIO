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


        public double GetDouble(JObject ctx, ISegRun theSeg)
        {
            return _v;
        }

        public long GetLong(JObject ctx, ISegRun theSeg)
        {
            return _v;
        }

        public bool CanCalc(JObject ctx, ISegRun theSeg)
        {
            return true;
        }

        public int GetInt(JObject ctx, ISegRun theSeg)
        {
            return (int)_v;
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


        public double GetDouble(JObject ctx, ISegRun theSeg)
        {
            return _v;
        }

        public long GetLong(JObject ctx, ISegRun theSeg)
        {
            return (long)_v;
        }

        public bool CanCalc(JObject ctx, ISegRun theSeg)
        {
            return true;
        }

        public int GetInt(JObject ctx, ISegRun theSeg)
        {
            return (int)_v;
        }
    }

    internal class ExpIdValue : IExpRun
    {
        private string _id;

        public ExpIdValue(string v)
        {
            _id = v;
        }

        public bool IsConst => false;

        public bool IsIntOne => false;

        public bool IsThis => false;


        public double GetDouble(JObject ctx, ISegRun theSeg)
        {
            return ctx[_id].Value<double>();
        }

        public long GetLong(JObject ctx, ISegRun theSeg)
        {
            return ctx[_id].Value<long>();
        }

        public bool CanCalc(JObject ctx, ISegRun theSeg)
        {
            return (ctx != null && ctx.ContainsKey(_id));
        }

        public int GetInt(JObject ctx, ISegRun theSeg)
        {
            return ctx[_id].Value<int>();
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

        public bool IsIntOne => (IsConst && GetLong(null, null)==1);

        public bool IsThis => false;

        public double GetDouble(JObject ctx, ISegRun theSeg)
        {
            switch (_t)
            {
                case ExpCalcType.EXP_ADD:
                    return _left.GetDouble(ctx, theSeg) + _right.GetDouble(ctx, theSeg);
                case ExpCalcType.EXP_SUB:
                    return _left.GetDouble(ctx, theSeg) - _right.GetDouble(ctx, theSeg);
                case ExpCalcType.EXP_MUL:
                    return _left.GetDouble(ctx, theSeg) * _right.GetDouble(ctx, theSeg);
                case ExpCalcType.EXP_DIV:
                    return _left.GetDouble(ctx, theSeg) / _right.GetDouble(ctx, theSeg);
            }
            throw new Exception("unknow");
        }

        public long GetLong(JObject ctx, ISegRun theSeg)
        {
            return (long)GetDouble(ctx, theSeg);
        }

        public bool CanCalc(JObject ctx, ISegRun theSeg)
        {
            return _left.CanCalc(ctx, theSeg) && _right.CanCalc(ctx, theSeg);
        }

        public int GetInt(JObject ctx, ISegRun theSeg)
        {
            return (int)GetDouble(ctx, theSeg);
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

        public bool CanCalc(JObject ctx, ISegRun theSeg)
        {
            return true;
        }

        public double GetDouble(JObject ctx, ISegRun theSeg)
        {
            return GetLong(ctx, theSeg);
        }

        public int GetInt(JObject ctx, ISegRun theSeg)
        {
            ISegRun seg = null;
            if (IsThis)
                seg = theSeg.Parent;
            else
                seg = theSeg.Parent[_seg];
            var len = seg.GetBitLen(ctx);
            if (len % 8 != 0) throw new Exception("runtime 数据帧字段未能整字节对齐");
            return len / 8;
        }

        public long GetLong(JObject ctx, ISegRun theSeg)
        {
            return GetInt(ctx, theSeg);
        }

    }


    //表达式接口
    internal interface IExpRun
    {
        bool IsConst { get; }
        bool IsIntOne { get; }
        bool IsThis { get; }
        bool CanCalc(JObject ctx, ISegRun theSeg);
        long GetLong(JObject ctx, ISegRun theSeg);
        int GetInt(JObject ctx, ISegRun theSeg);
        double GetDouble(JObject ctx, ISegRun theSeg);
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
