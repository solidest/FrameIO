using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //One of 的字段组 case分支
    internal class SegRunOneOfItem : SegRunGroup
    {
        private long _byvalue;

        //从json加载内容
        new static internal SegRunOneOfItem LoadFromJson(JObject o, string name, SegRunContainer parent)
        {
            var ret = new SegRunOneOfItem();
            ret.Name = name;
            ret.Parent = parent;
            ret.FillFromJson(o);
            return ret;
        }

        internal protected override void FillFromJson(JObject o)
        {
            base.FillFromJson(o);
            _byvalue = o[ONEOFBYVALUE_TOKEN].Value<long>();
        }
    }
}
