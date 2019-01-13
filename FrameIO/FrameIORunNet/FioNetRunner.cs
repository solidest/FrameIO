using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    public static class FioNetRunner
    {
        //获取一个通道
        public static FioChannel GetChannel(ChannelOption chops)
        {
            var ret = IORunner.GetChannel((ChannelTypeEnum)chops.GetOption("$channeltype"), chops);
            
            return ret;
        }

        //获取一个数据帧的空数据对象
        public static FioNetObject NewFrameObject(string frameName)
        {
            return new FioNetObject(IORunner.NewFrameObject(frameName));
        }

        //发送数据
        public static void SendFrame(FioNetObject data, FioChannel ch)
        {

        }

        //接收数据
        public static FioNetObject RecvFrame(string frame, FioChannel ch)
        {
            return null;
        }
    }
}
