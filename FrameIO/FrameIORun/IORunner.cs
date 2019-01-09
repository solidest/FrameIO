using FrameIO.Interface;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace FrameIO.Run
{
    public static class IORunner
    {

        #region --Frame--

        static private Dictionary<string, SegRunFrame> _frms = new Dictionary<string, SegRunFrame>();

        //从json字符串加载全部数据帧
        internal static void InitialFromJson(string json)
        {
            var jfrms = JObject.Parse(json)[SegRunBase.FRAMELIST_TOKEN].Value<JArray>();

            foreach (JObject vo in jfrms)
            {
                var p = (JProperty)vo.First;
                var o = p.Value.Value<JObject>();
                _frms.Add(p.Name, SegRunFrame.NewFrame(o, p.Name));
            }
        }

        //获取一个数据帧的空数据对象
        public static FrameObject NewFrameObject(string frameName)
        {
            return new FrameObject(frameName);
        }

        //取数据帧
        internal static SegRunFrame GetFrame(string name)
        {
            return _frms[name];
        }

        #endregion

        #region --Channel--

        public static IOChannel GetChannel(ChannelTypeEnum chtype, ChannelOption chops)
        {
            var ops = chops.Options;
            IChannelBase ich = null;
            switch (chtype)
            {
                case ChannelTypeEnum.CAN:
                    if (ops["vendor"].ToString() == "yh")
                    {
                        ich = new FrameIO.Driver.YH_CAN_Impl();
                    }
                    else if (ops["vendor"].ToString() == "zy")
                    {

                    }
                    break;

                case ChannelTypeEnum.COM:
                    ich = new FrameIO.Driver.Com_Impl();
                    break;

                case ChannelTypeEnum.TCPCLIENT:
                    ich = new FrameIO.Driver.TCPClient_Impl();
                    break;

                case ChannelTypeEnum.TCPSERVER:
                    ich = new FrameIO.Driver.TCPServer_Impl();
                    break;

                case ChannelTypeEnum.UDP:
                    ich = new FrameIO.Driver.UDPClient_Impl();
                    break;

                case ChannelTypeEnum.DIO:
                    ich = new FrameIO.Driver.DIO_Impl();
                    break;
            }

            Debug.Assert(ich != null);
            return new IOChannel(ich, chops);
        }

        #endregion

    }
}
