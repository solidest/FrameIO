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


        public double GetDouble(JObject vParent, ISegRun theSeg)
        {
            return _v;
        }

        public long GetLong(JObject vParent, ISegRun theSeg)
        {
            return _v;
        }

        public bool HaveValue(JObject vParent, ISegRun theSeg)
        {
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


        public double GetDouble(JObject vParent, ISegRun theSeg)
        {
            return _v;
        }

        public long GetLong(JObject vParent, ISegRun theSeg)
        {
            return (long)_v;
        }

        public bool HaveValue(JObject vParent, ISegRun theSeg)
        {
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


        public double GetDouble(JObject vParent, ISegRun theSeg)
        {
            return vParent[_v].Value<double>();
        }

        public long GetLong(JObject vParent, ISegRun theSeg)
        {
            return vParent[_v].Value<long>();
        }

        public bool HaveValue(JObject vParent, ISegRun theSeg)
        {
            return (vParent != null && vParent.ContainsKey(_v));
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

        public double GetDouble(JObject vParent, ISegRun theSeg)
        {
            switch (_t)
            {
                case ExpCalcType.EXP_ADD:
                    return _left.GetDouble(vParent, theSeg) + _right.GetDouble(vParent, theSeg);
                case ExpCalcType.EXP_SUB:
                    return _left.GetDouble(vParent, theSeg) - _right.GetDouble(vParent, theSeg);
                case ExpCalcType.EXP_MUL:
                    return _left.GetDouble(vParent, theSeg) * _right.GetDouble(vParent, theSeg);
                case ExpCalcType.EXP_DIV:
                    return _left.GetDouble(vParent, theSeg) / _right.GetDouble(vParent, theSeg);
            }
            throw new Exception("unknow");
        }

        public long GetLong(JObject vParent, ISegRun theSeg)
        {
            return (long)GetDouble(vParent, theSeg);
        }

        public bool HaveValue(JObject vParent, ISegRun theSeg)
        {
            return _left.HaveValue(vParent, theSeg) && _right.HaveValue(vParent, theSeg);
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

        public double GetDouble(JObject vParent, ISegRun theSeg)
        {
            return GetLong(vParent, theSeg);
        }

        public long GetLong(JObject vParent, ISegRun theSeg)
        {
            var len = theSeg.GetBitLen(vParent);
            if (len % 8 != 0) throw new Exception("runtime 数据帧字段未能整字节对齐");
            return len / 8;
        }

        public bool HaveValue(JObject vParent, ISegRun theSeg)
        {
            int len = 0;
            return theSeg.GetNeedBitLen(ref len, out theSeg, vParent);
        }

    }


    //表达式接口
    internal interface IExpRun
    {
        bool IsConst { get; }
        bool IsIntOne { get; }
        bool IsThis { get; }

        long GetLong(JObject vParent, ISegRun theSeg);
        double GetDouble(JObject vParent, ISegRun theSeg);

        bool HaveValue(JObject vParent, ISegRun theSeg);
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
