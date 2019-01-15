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
        private IExpRun _arrLen;


        public override bool IsArray => _arrLen!=null;


        #region --Initial--

        //从json加载内容
        static internal SegRunOneOfGroup NewOneOfGroup(JObject o, string name, bool isArray)
        {
            var ret = new SegRunOneOfGroup();
            ret.Name = name;
            ret.InitialFromJson((JObject)o[ONEOFLIST_TOKEN]);
            ret._byseg = o[ONEOFBYSEGMENT_TOKEN].Value<string>();
            if (isArray) ret._arrLen = Helper.GetExp(o[ARRAYLEN_TOKEN]);
            return ret;
        }


        protected internal override SegmentTypeEnum GetItemType(JObject o)
        {
            return SegmentTypeEnum.SegOneOfItem;
        }


        #endregion


        #region --Pack--

        public override void Pack(IFrameWriteBuffer buff, JObject parent, JToken theValue)
        {
            if (_arrLen != null)
            {
                SegRunArray.Pack(_arrLen, PackItem, this, buff, parent, (JArray)theValue);
            }
            else
                PackItem(buff, parent, theValue);

        }

        public override int GetBitLen(JObject parent)
        {
            return (_arrLen != null) ? SegRunArray.GetBitLen(_arrLen, GetItemBitLen, this, parent) : GetItemBitLen(parent, parent[Name]);
        }

        public int GetItemBitLen(JObject parent, JToken theValue)
        {
            var select = SelectedOneItem(parent);
            return select.GetItemBitLen(theValue?.Value<JObject>(), theValue?[select.Name]?.Value<JObject>());
        }

        private void PackItem(IFrameWriteBuffer buff, JObject parent, JToken theValue)
        {
            Debug.Assert(theValue != null);
            var select = SelectedOneItem(parent);
            select.Pack(buff, theValue.Value<JObject>(), theValue[select.Name]);
        }

        #endregion


        #region --Unpack--

        //自下而上 分支执行完毕
        public override bool LookUpNextValueSeg(out SegRunValue firstSeg, out JContainer pc, out int repeated, JObject ctxOfChild)
        {
            var myp = GetValueParent(ctxOfChild);

            //分支循环
            if (IsArray && ctxOfChild.Next != null)
            {
                var select = SelectedOneItem(myp);
                var next = (JObject)ctxOfChild.Next;
                return select.LookUpFirstValueSeg(out firstSeg, out pc, out repeated, next, next);
            }


            //向兄弟传递
            if (Next != null)
            {
                return Next.LookUpFirstValueSeg(out firstSeg, out pc, out repeated, myp, myp[Next.Name]);
            }

            //向上传递
            if (Parent == null)
            {
                firstSeg = null;
                pc = null;
                repeated = 0;
                return true;
            }
            else
                return Parent.LookUpNextValueSeg(out firstSeg, out pc, out repeated, myp);
        }


        //自上而下 进入分支
        public override bool LookUpFirstValueSeg(out SegRunValue firstSeg, out JContainer pc, out int repeated, JObject ctx, JToken theValue)
        {
            //无法继续
            var select = SelectedOneItem(ctx);
            if (select==null || IsArray && !_arrLen.CanCalc(ctx, this))
            {
                firstSeg = null;
                pc = ctx;
                repeated = 0;
                return false;
            }

            //空白
            if (First == null)
            {
                return LookUpNextValueSeg(out firstSeg, out pc, out repeated, ctx);
            }

            //初始化自身
            JObject myselect = null;

            if (theValue == null)
            {
                var my = new JObject();
                ctx.Add(Name, my);
                if (IsArray)
                {
                    var arr = new JArray();
                    for (int i = 0; i < _arrLen.GetInt(ctx, this); i++)
                    {
                        arr.Add(new JObject());
                    }
                    my.Add(select.Name, arr);
                    myselect = (JObject)arr.First;
                }
                else
                {
                    myselect = new JObject();
                    my.Add(select.Name, myselect);
                }
            }
            else
                myselect = (JObject)((JObject)theValue)[select.Name];

            //向下查找
            return select.LookUpFirstValueSeg(out firstSeg, out pc, out repeated, (JObject)theValue, myselect);
        }


        #endregion

        #region --Helper--

        private SegRunOneOfItem SelectedOneItem(JObject ctx)
        {
            if (ctx == null || !ctx.ContainsKey(_byseg)) return null;
            var byv = ctx[_byseg].Value<long>();
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

        public override JToken GetDefaultValue()
        {
            return new JObject();
        }

        public override JToken GetAutoValue(IFrameWriteBuffer buff, JObject parent)
        {
            return GetDefaultValue();
        }


        #endregion
    }
}
