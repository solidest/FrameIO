using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //字段组
    internal class SegRunGroup : SegRunContainer
    {
        protected internal override string ItemsListToken => SEGMENTLIST_TOKEN;

        internal override SegRunContainer Parent { get; set; }
        internal override SegRunBase Next { get; set; }
        internal override SegRunBase Previous { get; set; }
        internal override SegRunBase First { get; set; }
        internal override SegRunBase Last { get; set; }
        internal override SegRunContainer Root { get; set; }
        internal override string Name { get; set; }

        #region --Initial--

         //从json加载内容
        static internal SegRunGroup LoadFromJson(JObject o, string name, SegRunContainer parent)
        {
            var ret = new SegRunGroup();
            ret.Parent = parent;
            ret.Name = name;
            ret.FillFromJson(o);
            return ret;
        }

        protected internal override SegmentTypeEnum GetItemType(JObject o)
        {
            return (SegmentTypeEnum)Enum.Parse(typeof(SegmentTypeEnum), o[SEGMENTTYPE_TOKEN].Value<string>());
        }

        #endregion
      

        #region --Pack--

        internal override SegRunBase Pack(FramePackBuffer buff, JToken value)
        {
            var v = (value?.Value<JObject>())?? new JObject();
            var seg = First;
            while (seg != null)
            {
                seg = seg.Pack(buff, v[seg.Name]);
            }
            return Next;
        }



        #endregion
    }
}
