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
        bool Valid(IFrameReadBuffer buff, SegRunNumber seg, JToken value);
        string ErrorInfo { get; }
    }

    internal class Validete 
    {
        private List<IValidate> _vs = new List<IValidate>();


        public SegmentMaxValidator AddMaxValidate(JToken max)
        {
            if (max == null) return null;
            var v = new SegmentMaxValidator(max.Value<double>());
            _vs.Add(v);
            return v;
        }

        public SegmentMinValidator AddMinValidate(JToken min)
        {
            if (min == null) return null;
            var v = new SegmentMinValidator(min.Value<double>());
            _vs.Add(v);
            return v;
        }

        public SegmentCheckValidator AddCheckValidate(JToken checktype, JToken begin, JToken end)
        {
            if (checktype == null) return null;
            var v = new SegmentCheckValidator(Helper.GetCheckType(checktype), begin?.Value<string>(), end?.Value<string>());
            _vs.Add(v);
            return v;

        }

        public bool Valid(IFrameReadBuffer buff, SegRunNumber seg, JToken value)
        {
            if (_vs == null) return true;
            bool ret = true;
            foreach(var v in _vs)
            {
                if (!v.Valid(buff, seg, value))
                {
                    seg.LogError(Interface.FrameIOErrorType.RecvErr, v.ErrorInfo);
                    ret = false;
                }
            }
            return ret;
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

        public string ErrorInfo { get; set; }

        public bool Valid(IFrameReadBuffer buff, SegRunNumber seg, JToken value)
        {
            bool ret = false;
            switch (value.Type)
            {
                case JTokenType.Integer:
                    ret = (value.Value<long>() <= _max);
                    break;

                case JTokenType.Float:
                    ret = (value.Value<double>() <= _max);
                    break;
                    
            }
            if (!ret) ErrorInfo = "超过最大值";
            return ret;
        }
    }

    //最小值验证
    internal class SegmentMinValidator : IValidate
    {
        private double _min;
        internal SegmentMinValidator(double minv)
        {
            _min = minv;
        }

        public string ErrorInfo { get; set; }

        public bool Valid(IFrameReadBuffer buff, SegRunNumber seg, JToken value)
        {
            bool ret = false;
            switch (value.Type)
            {
                case JTokenType.Integer:
                    ret = (value.Value<long>() >= _min);
                    break;

                case JTokenType.Float:
                    ret = (value.Value<double>() >= _min);
                    break;

            }
            if (!ret) ErrorInfo = "小于最小值";
            return ret;
        }
    }



    //校验值验值
    internal class SegmentCheckValidator : IValidate
    {
        private CheckTypeEnum _checktype;
        private string _begin_seg;
        private string _end_seg;

        public string ErrorInfo { get; set; }

        public SegmentCheckValidator(CheckTypeEnum checktype, string beginSeg, string endSeg)
        {
            _checktype = checktype;
            _begin_seg = beginSeg;
            _end_seg = endSeg;
        }

        public bool Valid(IFrameReadBuffer buff, SegRunNumber seg, JToken value)
        {
            var res = GetCheckResult(buff, value.Parent.Parent.Value<JObject>(), seg.Parent, value);
            var ret = (value.Value<ulong>() == res);
            if (!ret) ErrorInfo = "校验失败";
            return ret;
        }

        public ulong GetCheckResult(IFrameReadBuffer buff, JObject vParent, SegRunContainer segParent, JToken theValue)
        {
            int i1 = 0;
            int i2 = 0;
            
            if(_begin_seg != null)
            {
                var first = vParent[_begin_seg];
                while (first.Type == JTokenType.Object && first.First != null)
                    first = ((JProperty)first.First).Value;
                i1 = buff.GetBytePos(first.Parent);
            }
            
            if(_end_seg!=null)
            {
                var end = vParent[_end_seg];
                while (end.Type == JTokenType.Object && end.Last != null)
                    end = ((JProperty)end.Last).Value;
                i2 = buff.GetBytePos(end.Parent) + (segParent[_end_seg].GetBitLen(vParent)/8);
            }
            else
            {
                i2 = buff.GetBytePos(theValue.Parent);
            }


            return CRCHelper.GetCheckValue(_checktype, buff.GetBuffer(), i1, i2);

        }

        public ulong GetCheckResult(IFrameWriteBuffer buff, JObject vParent, SegRunContainer segParent, SegRunBase seg)
        {
            int i1 = 0;
            int i2 = 0;
            if(_begin_seg != null)
            {
                var first = vParent[_begin_seg];
                while (first.Type== JTokenType.Object && first.First != null) 
                    first = ((JProperty)first.First).Value;
                i1 = buff.GetBytePos(first.Parent);
            }

            JToken end = null;
            var endseg = _end_seg;
            if(_end_seg != null)
                end = vParent[_end_seg];
            else
            {
                endseg = seg.Previous.Name;
                end = vParent[endseg];
            }
            while (end.Type == JTokenType.Object && end.Last != null)
                end = ((JProperty)end.Last).Value;
            i2 = buff.GetBytePos(end.Parent) + (segParent[endseg].GetBitLen(vParent) / 8);

            return CRCHelper.GetCheckValue(_checktype, buff.GetBuffer(), i1, i2);

        }

    }

    #endregion

}
