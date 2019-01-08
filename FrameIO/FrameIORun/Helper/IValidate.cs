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
        bool Valid(IFrameWriteBuffer buff, SegRunValue seg, JToken value);
        string ErrorInfo { get; }
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

        public bool Valid(IFrameWriteBuffer buff, SegRunValue seg, JToken value)
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

        public bool Valid(IFrameWriteBuffer buff, SegRunValue seg, JToken value)
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

        public bool Valid(IFrameWriteBuffer buff, SegRunValue seg, JToken value)
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
            return false;
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

        public bool Valid(IFrameWriteBuffer buff, SegRunValue seg, JToken value)
        {

            var res = GetCheckResult(buff, value.Parent.Value<JObject>(), seg.Parent);
            var ret = (value.Value<ulong>() == res);
            if (!ret) ErrorInfo = "校验失败";
            return ret;
        }

        public ulong GetCheckResult(IFrameWriteBuffer buff, JObject vParent, SegRunContainer segParent)
        {
            //var ctx = new ExpRunCtx(buff, vParent, segParent);

            //return CRCHelper.GetCheckValue(_checktype, buff.GetBuffer(), ctx.GetStartPos(_begin_seg), ctx.GetEndPos(_end_seg));
            //HACK
            return 0;
        }
    }

    #endregion

}
