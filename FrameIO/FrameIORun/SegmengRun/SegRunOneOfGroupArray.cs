using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //One of 的字段组 case数组分支

    internal class SegRunOneOfGroupArray : SegRunOneOfGroup
    {
        private IExpRun _arrlen;

        //从json加载内容
        new static internal SegRunOneOfGroupArray LoadFromJson(JObject o, string name, SegRunContainer parent)
        {
            var ret = new SegRunOneOfGroupArray();
            ret.Name = name;
            ret.Parent = parent;
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
