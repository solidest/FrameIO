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
        public override bool IsArray  => _arrLen != null;

        private IExpRun _arrLen;

        #region --Initial--

         //从json加载内容
        static internal SegRunGroup NewSegGroup(JObject o, string name)
        {
            var ret = new SegRunGroup();
            ret.Name = name;
            ret.InitialFromJson(o);
            return ret;
        }

        static internal SegRunGroup NewSegGroupArray(JObject o, string name, JObject owner)
        {
            var ret = new SegRunGroup();
            ret.Name = name;
            ret._arrLen = Helper.GetExp(owner[ARRAYLEN_TOKEN]);
            ret.InitialFromJson(o);
            return ret;
        }

        protected internal override SegmentTypeEnum GetItemType(JObject o)
        {
            return (SegmentTypeEnum)Enum.Parse(typeof(SegmentTypeEnum), o[SEGMENTTYPE_TOKEN].Value<string>());
        }

        #endregion


        #region --Pack--

        public override void Pack(IFrameWriteBuffer buff, JObject parent, JToken theValue)
        {
            if (IsArray)
            {
                SegRunArray.Pack(_arrLen, PackItem, this, buff, parent, (JArray)theValue);
            }
            else
                PackItem(buff, parent, theValue);

        }


        private void PackItem(IFrameWriteBuffer buff, JObject parent, JToken theValue)
        {
            var my = (JObject)theValue;
            var seg = First;
            while (seg != null)
            {
                var tv = my[seg.Name];
                if(tv == null)
                {
                    tv = seg.GetAutoValue(buff, parent);
                    my.Add(seg.Name, tv);
                }
                seg.Pack(buff, my, tv);
                seg = seg.Next;
            }
        }


        public override JToken GetDefaultValue()
        {
            return new JObject();
        }

        public override int GetBitLen(JObject parent)
        {
            return (_arrLen !=null) ? SegRunArray.GetBitLen(_arrLen, GetItemBitLen, this, parent) : GetItemBitLen(parent, parent[Name]);
        }

        public int GetItemBitLen(JObject parent, JToken theValue)
        {
            if (theValue == null) return 0;
            int ret = 0;
            
            var p = First;
            while (p != null)
            {
                ret += p.GetBitLen(parent);
                p = p.Next;
            }
            return ret;
        }

        #endregion

        #region --Unpack--

        //子节点完毕 查找下一个兄弟
        public override bool LookUpNextValueSeg(out SegRunValue firstSeg, out JContainer pc, out int repeated, JObject ctxOfChild)
        {
            var myp = GetValueParent(ctxOfChild);

            //自身传递
            if (IsArray && ctxOfChild.Next != null)
            {
                return LookUpFirstValueSeg(out firstSeg, out pc, out repeated, myp, ctxOfChild.Next);
            }

            //向兄弟传递
            if(Next != null)
            {
                return Next.LookUpFirstValueSeg(out firstSeg, out pc, out repeated, myp, myp[Next.Name]);
            }

            //向上传递
            if (Parent == null) //已经到达末尾
            {
                firstSeg = null;
                pc = null;
                repeated = 0;
                return true;
            }
            else
                return Parent.LookUpNextValueSeg(out firstSeg, out pc, out repeated, myp);
        }


        //自上而下查找
        public override bool LookUpFirstValueSeg(out SegRunValue firstSeg, out JContainer pc, out int repeated, JObject ctx, JToken theValue)
        {

            //无法继续
            if(IsArray && !_arrLen.CanCalc(ctx, this))
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
            JObject my = null;

            if (theValue == null)
            {
                if (IsArray)
                {
                    var arr = new JArray();
                    for (int i = 0; i < _arrLen.GetInt(ctx, this); i++)
                    {
                        arr.Add(new JObject());
                    }
                    ctx.Add(Name, arr);
                    my = (JObject)arr.First;
                }
                else
                {
                    my = new JObject();
                    ctx.Add(Name, my);
                }
            }
            else
                my = (JObject)theValue;

            //向下查找
            return First.LookUpFirstValueSeg(out firstSeg, out pc, out repeated, my, my[First.Name]);
        }


        public override JToken GetAutoValue(IFrameWriteBuffer buff, JObject parent)
        {
            return GetDefaultValue();
        }


        #endregion

    }
}
