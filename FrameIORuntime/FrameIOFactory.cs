using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;


namespace FrameIO.Runtime
{
    //解包类创建工厂
    public class FrameIOFactory
    {

        #region --NotUsed--

        //获取数据帧的解析器
        public static IFrameUnpack GetFrameUnpack(string framename)
        {
            return null;
        }

        //获取数据帧的打包接口
        public static ISegmentSettor GetFramePack(string framename)
        {
            return null;// new FramePack(_pj.DicFrame[framename].RootSegBlockGroupInfo);
        }

        #endregion

        //初始化
        public static void Initialize(byte[] config)
        {
            FrameRuntime.Initialize(config);
        }

        //获取数据帧的字段设置接口
        public static ISegmentSettor GetFrameSettor(ushort idx)
        {
            return ((SegmentFramBegin)FrameRuntime.Run[idx]).GetFramePacker();
        }

        public static void Initial(byte[] v)
        {
            FrameRuntime.Initialize(v);
        }

        //获取数据帧解包接口
        public static IFrameUnpack GetFrameUnpacker(ushort idx)
        {
            return ((SegmentFramBegin)FrameRuntime.Run[idx]).GetFrameUnpacker();
        }

        //获取通道的接口

        public static IChannelBase GetChannel(ChannelTypeEnum chtype, ChannelOption options)
        {
            var ops = options.GetOptions();
            switch (chtype)
            {
                case ChannelTypeEnum.CAN:
                    if (ops["vendor"].ToString() == "yh")
                    {
                        var chcan = new FrameIO.Driver.YH_CAN_Impl();
                        //chcan.InitConfig(ch.DicOption);
                        return chcan;
                    }
                    else if (ops["vendor"].ToString() == "zy")
                    {

                    }
                    break;
                case ChannelTypeEnum.COM:
                    {
                        var chcom = new FrameIO.Driver.Com_Impl();
                        chcom.InitConfig(ops);
                        return chcom;
                    }
                case ChannelTypeEnum.TCPCLIENT:
                    {
                        var chtcpclient = new FrameIO.Driver.TCPClient_Impl();
                        chtcpclient.InitConfig(ops);
                        return chtcpclient;
                    }
                case ChannelTypeEnum.TCPSERVER:
                    {
                        var chtcpserver = new FrameIO.Driver.TCPServer_Impl();
                        chtcpserver.InitConfig(ops);
                        return chtcpserver;
                    }
                    //TODO 添加其它类型的驱动调用

            }
            return null;
        }
    }
}
