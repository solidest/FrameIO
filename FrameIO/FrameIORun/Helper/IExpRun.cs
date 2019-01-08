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

        public double GetDouble(JObject vParent, SegRunContainer segParent)
        {
            return _v;
        }

        public long GetLong(JObject vParent, SegRunContainer segParent)
        {
            return _v;
        }

        public bool TryGetDouble(JObject vParent, SegRunContainer segParent, ref double v)
        {
            v = _v;
            return true;
        }

        public bool TryGetLong(JObject vParent, SegRunContainer segParent, ref long v)
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

        public double GetDouble(JObject vParent, SegRunContainer segParent)
        {
            return _v;
        }

        public long GetLong(JObject vParent, SegRunContainer segParent)
        {
            return (long)_v;
        }

        public bool TryGetDouble(JObject vParent, SegRunContainer segParent, ref double v)
        {
            v = _v;
            return true;
        }

        public bool TryGetLong(JObject vParent, SegRunContainer segParent, ref long v)
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

        public double GetDouble(JObject vParent, SegRunContainer segParent)
        {
            return vParent[_v].Value<double>();
        }

        public long GetLong(JObject vParent, SegRunContainer segParent)
        {
            return vParent[_v].Value<long>();
        }

        public bool TryGetDouble(JObject vParent, SegRunContainer segParent, ref double v)
        {
            if (vParent != null && vParent.ContainsKey(_v))
            {
                v = vParent[_v].Value<double>();
                return true;
            }
            return false;
        }

        public bool TryGetLong(JObject vParent, SegRunContainer segParent, ref long v)
        {
            if (vParent != null && vParent.ContainsKey(_v))
            {
                v = vParent[_v].Value<long>();
                return true;
            }

            return false;
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

        public double GetDouble(JObject vParent, SegRunContainer segParent)
        {
            switch (_t)
            {
                case ExpCalcType.EXP_ADD:
                    return _left.GetDouble(vParent, segParent) + _right.GetDouble(vParent, segParent);
                case ExpCalcType.EXP_SUB:
                    return _left.GetDouble(vParent, segParent) - _right.GetDouble(vParent, segParent);
                case ExpCalcType.EXP_MUL:
                    return _left.GetDouble(vParent, segParent) * _right.GetDouble(vParent, segParent);
                case ExpCalcType.EXP_DIV:
                    return _left.GetDouble(vParent, segParent) / _right.GetDouble(vParent, segParent);
            }
            throw new Exception("unknow");
        }

        public long GetLong(JObject vParent, SegRunContainer segParent)
        {
            return (long)GetDouble(vParent, segParent);
        }

        public bool TryGetDouble(JObject vParent, SegRunContainer segParent, ref double v)
        {
            double d1=0, d2=0;
            if (_left.TryGetDouble(vParent, segParent, ref d1) && _right.TryGetDouble(vParent, segParent, ref d2))
            {
                v = d1 + d2;
                return true;
            }

            return false;
        }

        public bool TryGetLong(JObject vParent, SegRunContainer segParent, ref long v)
        {
            double d = 0;
            if( TryGetDouble(vParent, segParent, ref d))
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

        public double GetDouble(JObject vParent, SegRunContainer segParent)
        {
            return GetLong(vParent, segParent);
        }

        public long GetLong(JObject vParent, SegRunContainer segParent)
        {
            var len = segParent[_seg].GetBitLen(vParent);
            if (len % 8 != 0) throw new Exception("runtime 数据帧字段未能整字节对齐");
            return len / 8;
        }

        public bool TryGetDouble(JObject vParent, SegRunContainer segParent, ref double v)
        {
            long lv = 0;
            if(TryGetLong(vParent, segParent, ref lv))
            {
                v = lv;
                return true;
            }
            return false;
        }

        public bool TryGetLong(JObject vParent, SegRunContainer segParent, ref long v)
        {
            int lv = 0;
            if(segParent[_seg].TryGetBitLen(ref lv, vParent))
            {
                if (lv % 8 != 0) throw new Exception("runtime 数据帧字段未能整字节对齐");
                v = lv;
                return true;
            }
            return false;
        }
    }


    //表达式接口
    internal interface IExpRun
    {
        bool IsConst { get; }
        bool IsIntOne { get; }
        bool IsThis { get; }

        long GetLong(JObject vParent, SegRunContainer segParent);
        double GetDouble(JObject vParent, SegRunContainer segParent);

        bool TryGetLong(JObject vParent, SegRunContainer segParent, ref long v);
        bool TryGetDouble(JObject vParent, SegRunContainer segParent, ref double v);
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
