using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //OneOf分组
    internal class SegRunOneOfGroup : SegRunContainer
    {
        private string _byseg;

        protected internal override string ItemsListToken => ONEOFLIST_TOKEN;


        #region --Initial--

        //从json加载内容
        static internal SegRunOneOfGroup NewOneOfGroup(JObject o, string name, bool isArray)
        {
            var ret = new SegRunOneOfGroup();
            ret.Name = name;
            ret.InitialFromJson(o);
            if (isArray) ret.InitialArray(o);
            return ret;
        }

        protected override void InitialFromJson(JObject o)
        {
            base.InitialFromJson(o);
            _byseg = o[ONEOFBYSEGMENT_TOKEN].Value<string>();
        }

        protected internal override SegmentTypeEnum GetItemType(JObject o)
        {
            return SegmentTypeEnum.SegOneOfItem;
        }


        #endregion



        #region --Pack--

        public override int GetItemBitLen(JObject parent, JToken theValue)
        {
            var select = GetOneItem(parent);
            return select.GetItemBitLen(theValue?.Value<JObject>(), theValue?[select.Name]?.Value<JObject>());
        }

        public override ISegRun PackItem(IFrameWriteBuffer buff, JObject parent, JArray arr, JToken theValue)
        {
            Debug.Assert(theValue != null);
            var select = GetOneItem(parent);
            select.PackItem(buff, theValue.Value<JObject>(), null, theValue[select.Name]?.Value<JObject>());
            return Next;
        }


        #endregion



        #region --UnPack--


        public override ISegRun UnPackItem(IFrameReadBuffer buff, JObject parent, JArray arr, JToken theValue)
        {
            var select = GetOneItem(parent);
            JObject myov = theValue?.Value<JObject>();
            if(myov == null)
            {
                myov = new JObject();
                if (arr != null)
                    arr.Add(myov);
                else
                    parent.Add(Name, myov);
            }
            var ret = select.UnPackItem(buff, myov, null, myov[select.Name]?.Value<JObject>());
            return ret??Next;
        }

        public override bool GetItemNeedBitLen(ref int len, out ISegRun next, JObject parent, JToken theValue)
        {
            next = this;
            var select = GetOneItem(parent);
            if (select == null) return false;
            return select.GetItemNeedBitLen( ref len, out next, theValue?.Value<JObject>(), theValue?[select.Name].Value<JObject>());
        }

        #endregion

        #region --Helper--

        private SegRunOneOfItem GetOneItem(JObject parent)
        {
            if (parent == null || !parent.ContainsKey(_byseg)) return null;
            var byv = parent[_byseg].Value<long>();
            var it = (SegRunOneOfItem)First;
            while(it!=null)
            {
                if (it.IsDefault)
                    return it;
                else if (it.ByValue == byv)
                    return it;
                it = (SegRunOneOfItem)it.Next;
            }
            return null;
        }

        #endregion
    }
}
