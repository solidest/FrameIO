using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //打包使用的二进制缓冲流
    internal class FramePackBuffer
    {

        private int _bitPos;
        private Dictionary<JToken, SegRunInfo> _infos;

        public FramePackBuffer()
        {
            _bitPos = 0;
            _infos = new Dictionary<JToken, SegRunInfo>();
        }

        #region --Write-- 

        //写入到缓冲
        public void DoWriteValue(SegRunValue seg, JValue value)
        {
            var info = AddInfo(value, seg, _bitPos, seg.BitLen);

            //HACK 写入
            //seg.GetBuffer(value);

            _bitPos += info.BitLen;
        }


        #endregion




        #region --Helper--

        private SegRunInfo AddInfo(JToken value, SegRunBase seg, int bitPos, int bitLen)
        {
            var ret = new SegRunInfo(value, seg, bitPos, bitLen);
            _infos.Add(value, ret);
            return ret;
        }

        //运行时字段信息
        internal class SegRunInfo
        {
            public SegRunInfo(JToken value, SegRunBase seg, int bitPos, int bitLen)
            {
                Value = value;
                Segment = seg;
                BitPos = bitPos;
                BitLen = bitLen;
            }

            //在内存流中的比特位置
            internal int BitPos { get; private set; }

            //对应的数值内容
            internal JToken Value { get; private set; }

            //对应的数据帧字段
            internal SegRunBase Segment { get; private set; }

            //比特位长度
            internal int BitLen { get; private set; }

        }

        #endregion


    }
}
