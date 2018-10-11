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
                        chcan.Open(ch.DicOption);
                        return chcan;
                    }
                    else if(ch.DicOption["vendor"].ToString() == "zlg")
                    {
                        
                    }
                    break;

                case syschanneltype.SCHT_COM:


                    break;

                   //TODO 添加其它类型的驱动调用

            }
            return null;
        }
    }
}
