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
        private SegRunFrame()
        {
            _matchlen = 0;
        }

        private int _matchlen;
        private long _matchvalue;

        internal override SegRunContainer Parent { get => null; set => throw new NotImplementedException(); }
        internal override SegRunBase Next { get => null; set => throw new NotImplementedException(); }
        internal override SegRunBase Previous { get => null; set => throw new NotImplementedException(); }
        internal override SegRunBase First { get; set; }
        internal override SegRunBase Last { get; set; }
        internal override SegRunContainer Root { get=>this; set => throw new NotImplementedException(); }

        //从json加载内容
        static internal SegRunFrame LoadFromJson(JObject o, string name)
        {
            var ret = new SegRunFrame();
            ret.Name = name;
            ret.FillFromJson(o);           
            return ret;
        }

        internal protected override void FillFromJson(JObject o)
        {
            base.FillFromJson(o);
            if (o.ContainsKey(HEADERMATCH_TOKEN))
            {
                _matchvalue = o[HEADERMATCH_TOKEN].Value<int>();
                _matchlen = o[HEADERMATCHLEN_TOKEN].Value<int>();
            }
        }



        #region --Pack--

        internal override SegRunBase Pack(FramePackBuffer buff, JToken value)
        {
            var v = value.Value<JObject>();
            var seg = First;
            while (seg != null)
            {
                seg = seg.Pack(buff, v[seg.Name]);
            }
            return null;
        }


        #endregion


    }
}
