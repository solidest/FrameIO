using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
   
    interface IValidate
    {
        bool Valid(SegRunBase seg);
    }

    internal class Validete 
    {
        private List<IValidate> _vs;

        private void AddValidate(IValidate v)
        {
            if (v == null) return;
            if (_vs == null) _vs = new List<IValidate>();
            _vs.Add(v);
        }

        public void AddMaxValidate(JObject o)
        {
            //HACK
        }

        public void AddMinValidate(JObject o)
        {
            //HACK

        }

        public void AddCheckValidate(JObject o)
        {
            //HACK

        }

        public bool Valid(SegRunBase seg)
        {
            if (_vs == null) return true;
            foreach(var v in _vs)
            {
                if (!v.Valid(seg)) return false;
            }
            return true;
        }
    }

    #region --验证插件类--


    //最大值验证
    internal class SegmentMaxValidator : IValidate
    {
        private double _max;
        internal SegmentMaxValidator(double maxv)
        {
            _max = maxv;
        }

        public bool Valid(SegRunBase seg)
        {
            throw new NotImplementedException();
        }

        //internal bool Valid(double value)
        //{
        //    return (value <= _max);
        //}
    }

    //最小值验证
    internal class SegmentMinValidator : IValidate
    {
        private double _min;
        internal SegmentMinValidator(double minv)
        {
            _min = minv;
        }

        public bool Valid(SegRunBase seg)
        {
            throw new NotImplementedException();
        }

        //internal bool Valid(double value)
        //{
        //    return (value >= _min);
        //}
    }



    //校验值验值
    internal class SegmentCheckValidator : IValidate
    {
        //private byte _checktype;

        //public ushort ChecekBeginSegIdx { get; }
        //public ushort ChecekEndSegIdx { get; }
        //public SegmentCheckValidator(byte checktype, ushort checkbeing, ushort checkend, ushort next_idx) : base(next_idx, ValidateType.Check)
        //{
        //    _checktype = checktype;
        //    ChecekBeginSegIdx = checkbeing;
        //    ChecekEndSegIdx = checkend;
        //}


        //internal ulong GetCheckValue(byte[] buff, int beginpos, int endpos)
        //{
        //    if (endpos <= beginpos) return 0;
        //    return CRCHelper.GetCheckValue(_checktype, buff, beginpos, endpos);
        //}

        public bool Valid(SegRunBase seg)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

}
