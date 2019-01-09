using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public override ISegRun PackItem(IFrameWriteBuffer buff, JObject parent, JArray arr, JToken theValue)
        {
            var my = theValue?.Value<JObject>();
            if(my == null)
            {
                my = new JObject();
                if (arr != null)
                    arr.Add(my);
                else
                    parent.Add(Name, my);
            }
            var seg = First;
            while (seg != null)
            {
                seg = seg.Pack(buff, my);
            }
            return Next;
        }

        public override int GetItemBitLen(JObject parent, JToken theValue)
        {
            int ret = 0;
            var my = theValue?.Value<JObject>();
            var p = First;
            while (p != null)
            {
                ret += p.GetBitLen(my);
                p = p.Next;
            }
            return ret;
        }

        #endregion


        #region --Unpack--

        public override ISegRun UnpackItem(IFrameReadBuffer buff, JObject parent, JArray arr, JToken theValue)
        {
            if(parent == null)
            {
                Debug.Assert(arr == null);
                parent = ((StopPosition)buff.StopPosition).Parent;
                theValue = parent[Name];
            }

            var my = theValue?.Value<JObject>();
            if (my == null)
            {
                my = new JObject();
                if (arr != null)
                    arr.Add(my);
                else
                    parent.Add(Name, my);
            }

            var p = First;
            while (p != null)
            {
                if(!buff.CanRead)
                {
                    buff.StopPosition = new StopPosition() { Parent = parent };
                    return p;
                }
                p = p.Unpack(buff, my);
            }

            return Next;
        }

        public override bool GetItemNeedBitLen(ref int len, out ISegRun next, JObject parent, JToken theValue)
        {
            var p = First;
            while (p != null)
            {
                if (!GetNeedBitLen(ref len, out next, theValue?.Value<JObject>()))
                {
                    return false;
                }
                p = p.Next;
            }
            next = Next;
            return true;
        }


        #endregion

    }
}
