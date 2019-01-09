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

        public override ISegRun PackItem(IFrameWriteBuffer buff, JObject parent, JToken theValue)
        {
            var seg = First;
            while (seg != null)
            {
                seg = seg.Pack(buff, theValue.Value<JObject>());
            }
            return Next;
        }

        public override int GetItemBitLen(JObject parent, JToken theValue)
        {
            int ret = 0;

            var p = First;
            while (p != null)
            {
                ret += p.GetBitLen(theValue?.Value<JObject>());
                p = p.Next;
            }
            return ret;
        }

        #endregion


        #region --UnPack--

        public override ISegRun UnPackItem(IFrameReadBuffer buff, JObject parent, JToken theValue, JArray mycontainer)
        {
            var my = theValue?.Value<JObject>();
            if (my == null)
            {
                my = new JObject();
                if (mycontainer == null)
                    parent.Add(Name, my);
                else
                    mycontainer.Add(my);
            }

            var p = First;
            while (p != null && buff.CanRead)
            {
                p = p.UnPack(buff, my, my[p.Name]);
            }

            return p ?? Next;
        }

        public override bool TryGetItemBitLen(ref int len, JObject parent, JToken theValue)
        {

            var p = First;
            while (p != null)
            {
                if (!TryGetBitLen(ref len, theValue?.Value<JObject>())) return false;
                p = p.Next;
            }
            return true;
        }


        #endregion

    }
}
