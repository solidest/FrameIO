using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //One of 的字段组 case分支
    internal class SegRunOneOfItem : SegRunGroup
    {
        private long? _byvalue;

        #region --Initial--

        //从json加载内容
        static public SegRunOneOfItem NewOneOfItem(JObject o, string name)
        {
            var ret = new SegRunOneOfItem();
            ret.Name = name;
            if (name != "other") ret._byvalue = o[ONEOFBYVALUE_TOKEN].Value<long?>();
            ret.InitialFromJson((JObject)o[SEGMENTLIST_TOKEN]);
            return ret;
        }

        #endregion

        #region --Helper--

        internal protected bool IsDefault { get => _byvalue == null; }

        internal protected long ByValue { get => _byvalue??0;  }


        #endregion


        #region --Unpack--

        //自下而上 分支执行完毕
        public override bool LookUpNextValueSeg(out SegRunValue firstSeg, out JContainer pc, out int repeated, JObject ctxOfChild)
        {
            //return ((SegRunOneOfGroup)Parent).LookUpNextValueSeg(out firstSeg, out pc, out repeated, (JObject)ctxOfChild.Parent.Parent);
            var myp = GetValueParent(ctxOfChild);
            return Parent.LookUpNextValueSeg(out firstSeg, out pc, out repeated, (JObject)myp.Parent.Parent);
        }



        ////向下穿透
        //public override bool LookUpFirstValueSeg(out SegRunValue firstSeg, out JContainer pc, out int repeated, JObject ctx, JToken theValue)
        //{

        //    //空白
        //    if (First == null)
        //    {
        //        return LookUpNextValueSeg(out firstSeg, out pc, out repeated, ctx);
        //    }

        //    //初始化自身
        //    JObject my = null;

        //    if (theValue == null)
        //    {
        //        my = new JObject();
        //        ctx.Add(Name, my);
        //    }
        //    else
        //        my = (JObject)theValue;

        //    //向下查找
        //    return First.LookUpFirstValueSeg(out firstSeg, out pc, out repeated, my, my[First.Name]);
        //}



        #endregion

    }
}
