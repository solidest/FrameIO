using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            MatchHeaderBytesLen = 0;
        }

        public int MatchHeaderBytesLen { get; private set; }
        private ulong _matchValue;


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
                _matchValue = o[HEADERMATCH_TOKEN].Value<ulong>();
                MatchHeaderBytesLen = o[HEADERMATCHLEN_TOKEN].Value<int>();
                Debug.Assert(MatchHeaderBytesLen <= 8);
            }
            base.InitialFromJson(o);
        }

        #endregion


        #region --Helper--


        public bool IsMatch(byte[] header)
        {
            var bff = new byte[8];
            for(int i=0; i<MatchHeaderBytesLen; i++)
            {
                bff[i] = header[i];
            }
            return BitConverter.ToUInt64(bff, 0) == _matchValue;
        }


        #endregion


    }
}
