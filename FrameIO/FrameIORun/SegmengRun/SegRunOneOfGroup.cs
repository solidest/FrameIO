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




        #region --UnPack--


        public override bool TryGetItemBitLen(IFrameBuffer buff, ref int len, JObject parent)
        {
            var select = GetOneItem(buff, parent);
            if (select == null) return false;
            return select.TryGetBitLen(buff, ref len, parent[Name].Value<JObject>());
        }

        #endregion


        #region --Pack--

        public override int GetItemBitLen(IFrameBuffer buff, JObject parent)
        {
            var select = GetOneItem(buff, parent);
            return select.GetBitLen(buff, parent?[Name].Value<JObject>());
        }

        public override ISegRun PackItem(IFrameBuffer buff, JObject parent)
        {
            var select = GetOneItem(buff, parent);
            select.Pack(buff, parent?[select.Name].Value<JObject>());
            return Next;
        }

        #endregion


        #region --Helper--

        private SegRunOneOfItem GetOneItem(IFrameBuffer buff, JObject parent)
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
