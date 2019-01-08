using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //运行时数据帧
    internal class SegRunFrame : SegRunGroup
    {
        public SegRunFrame()
        {
            _matchlen = 0;
        }

        private int _matchlen;
        private long _matchvalue;


        #region --Initial--

        //从json加载内容
        static public SegRunFrame NewFrame(JObject o, string name)
        {
            var ret = new SegRunFrame();
            ret.Name = name;
            ret.InitialFromJson(o);
            ret.Parent = null;
            ret.Next = null;
            ret.Previous = null;
            ret.First = null;
            ret.Last = null;
            ret.Root = ret;
            return ret;
        }

        protected override void InitialFromJson(JObject o)
        {
            if (o.ContainsKey(HEADERMATCH_TOKEN))
            {
                _matchvalue = o[HEADERMATCH_TOKEN].Value<int>();
                _matchlen = o[HEADERMATCHLEN_TOKEN].Value<int>();
            }
            base.InitialFromJson(o);
        }

        #endregion


    }
}
