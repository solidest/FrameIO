using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;
using FrameIO.Main;

namespace FrameIO.Run
{
    //解包类创建工厂
    public class FrameIOFactory
    {
        static private ProjectInfo _pj;

        //初始化工厂
        public static void InitialFactory(string binfile)
        {
            _pj = CodeFile.ReadFrameBinFile(binfile);
        }

        //获取数据帧的解析器
        public static IFrameUnpack GetFrameUnpack(string framename)
        {
            return new FrameUnpack(_pj.DicFrame[framename].RootSegBlockGroupInfo);
        }

        //获取数据帧的打包接口
        public static IFramePack GetFramePack(string framename)
        {
            return new FramePack(_pj.DicFrame[framename].RootSegBlockGroupInfo);
        }

        //获取通道的接口
        
        public static IChannelBase GetChannel(string sysname, string channelname)
        {
            var ch = _pj.DicSys[sysname].DicChannel[channelname];
            switch(ch.ChType)
            {
                case syschanneltype.SCHT_CAN:
                    if(ch.DicOption["vendor"].ToString() == "yh")
                    {
                        var chcan =new FrameIO.Driver.YH_CAN_Impl();
                        //chcan.InitConfig(ch.DicOption);
                        return chcan;
                    }
                    else if(ch.DicOption["vendor"].ToString() == "zy")
                    {
                        
                    }
                    break;

                case syschanneltype.SCHT_COM:
                    {
                        var chcom = new FrameIO.Driver.Com_Impl();
                        chcom.InitConfig(ch.DicOption);
                        return chcom;
                    }
                    break;
                case syschanneltype.SCHT_TCPCLIENT:
                    {
                        var chtcpclient = new FrameIO.Driver.TCPClient_Impl();
                        chtcpclient.InitConfig(ch.DicOption);
                        return chtcpclient;
                    }
                    break;
                case syschanneltype.SCHT_TCPSERVER:
                    {
                        var chtcpserver = new FrameIO.Driver.TCPServer_Impl();
                        chtcpserver.InitConfig(ch.DicOption);
                        return chtcpserver;
                    }
                    break;
                case syschanneltype.SCHT_UDP:
                    {
                        var chtudp = new FrameIO.Driver.UDPClient_Impl();
                        chtudp.InitConfig(ch.DicOption);
                        return chtudp;
                    }
                    break;

                   //TODO 添加其它类型的驱动调用

            }
            return null;
        }
    }
}
