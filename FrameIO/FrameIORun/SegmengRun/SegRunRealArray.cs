using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //小数数组
    internal class SegRunRealArray : SegRunReal
    {
        private IExpRun _arrlen;

        //从json加载内容
        new static internal SegRunRealArray LoadFromJson(JObject o, string name, SegRunContainer parent)
        {
            var ret = new SegRunRealArray();
            ret.Parent = parent;
            ret.Name = name;
            ret.FillFromJson(o);
            return ret;
        }

        internal protected override void FillFromJson(JObject o)
        {
            base.FillFromJson(o);
            _arrlen = Helper.GetExp(o[ARRAYLEN_TOKEN]);
        }

    }
}
