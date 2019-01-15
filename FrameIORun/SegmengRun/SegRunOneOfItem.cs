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
            var myp = GetValueParent(ctxOfChild);
            return Parent.LookUpNextValueSeg(out firstSeg, out pc, out repeated, (JObject)myp.Parent.Parent);
        }


        #endregion

    }
}
