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

        //初始化数据帧
        public static void Initialize(byte[] config)
        {
            FrameRuntime.Initialize(config);
        }

        //初始化符号表
        public static void InitializeSymbols(byte[] symbols)
        {
            FrameRuntime.InitializeSymbols(symbols);
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
                        chcan.InitConfig(ops);
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
                case ChannelTypeEnum.UDP:
                    {
                        var chudp = new FrameIO.Driver.UDPClient_Impl();
                        chudp.InitConfig(ops);
                        return chudp;
                    }
                case ChannelTypeEnum.DIO:
                    {
                        var chtcdio = new FrameIO.Driver.DIO_Impl();
                        chtcdio.InitConfig(ops);
                        return chtcdio;
                    }
                    //TODO 添加其它类型的驱动调用

            }
            return null;
        }
    }
}
