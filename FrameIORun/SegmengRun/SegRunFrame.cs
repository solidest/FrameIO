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
        public ulong MatchValue { get => _matchValue; }
        private ulong _matchValue;


        #region --Initial--

        //从json加载内容
        static public SegRunFrame NewFrame(JObject frame, string name)
        {
            var ret = new SegRunFrame();
            if (frame.ContainsKey(HEADERMATCH_TOKEN))
            {
                ret._matchValue = frame[HEADERMATCH_TOKEN].Value<ulong>();
                ret.MatchHeaderBytesLen = frame[HEADERMATCHLEN_TOKEN].Value<int>();
                Debug.Assert(ret.MatchHeaderBytesLen <= 8);
            }
            ret.InitialFromJson((JObject)frame[SEGMENTLIST_TOKEN]);
            ret.Name = name;
            ret.Parent = null;
            ret.Next = null;
            ret.Previous = null;
            ret.Root = ret;
            return ret;
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
