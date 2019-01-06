using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //OneOf分组
    internal class SegRunOneOfGroup : SegRunContainer
    {
        private SegRunInteger _byseg;

        protected internal override string ItemsListToken => ONEOFLIST_TOKEN;

        internal override SegRunContainer Parent {  get; set;  }
        internal override SegRunBase Next {  get; set;  }
        internal override SegRunBase Previous {  get; set;  }
        internal override SegRunBase First {  get; set;  }
        internal override SegRunBase Last {  get; set;  }
        internal override SegRunContainer Root {  get; set;  }

        #region --Initial--

        //从json加载内容
        static internal SegRunOneOfGroup LoadFromJson(JObject o, string name, SegRunContainer parent)
        {
            var ret = new SegRunOneOfGroup();
            ret.Name = name;
            ret.Parent = parent;
            ret.FillFromJson(o);
            return ret;
        }

        internal protected override void FillFromJson(JObject o)
        {
            base.FillFromJson(o);
            _byseg = (SegRunInteger)Parent[o[ONEOFBYSEGMENT_TOKEN].Value<string>()];
        }

        protected internal override SegmentTypeEnum GetItemType(JObject o)
        {
            return SegmentTypeEnum.SegOneOfItem;
        }

        #endregion


        #region --Pack--

        internal override SegRunBase Pack(FramePackBuffer buff, JToken value)
        {
            
            return Next;
        }



        #endregion
    }
}
