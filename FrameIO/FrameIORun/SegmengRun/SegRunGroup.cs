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


        #region --Initial--

         //从json加载内容
        static internal SegRunGroup NewSegGroup(JObject o, string name, bool isArray)
        {
            var ret = new SegRunGroup();
            ret.Name = name;
            ret.InitialFromJson(o);
            if (isArray) ret.InitialArray(o);
            return ret;
        }

        protected internal override SegmentTypeEnum GetItemType(JObject o)
        {
            return (SegmentTypeEnum)Enum.Parse(typeof(SegmentTypeEnum), o[SEGMENTTYPE_TOKEN].Value<string>());
        }

        #endregion


        #region --Pack--

        public override ISegRun PackItem(IFrameBuffer buff, JObject parent)
        {
            var my = parent?[Name]?.Value<JObject>();
            var seg = First;
            while (seg != null)
            {
                seg = seg.Pack(buff, my);
            }
            return Next;
        }

        public override int GetItemBitLen(IFrameBuffer buff, JObject parent)
        {
            var my = parent?[Name]?.Value<JObject>();
            int ret = 0;

            var p = First;
            while (p != null)
            {
                ret += p.GetBitLen(buff, my);
                p = p.Next;
            }
            return ret;
        }

        #endregion


        #region --UnPack--

        public override bool TryGetItemBitLen(IFrameBuffer buff, ref int len, JObject parent)
        {
            var my = parent?[Name]?.Value<JObject>();

            var p = First;
            while (p != null)
            {
                if (!TryGetBitLen(buff, ref len, my)) return false;
                p = p.Next;
            }
            return true;
        }


        #endregion

    }
}
