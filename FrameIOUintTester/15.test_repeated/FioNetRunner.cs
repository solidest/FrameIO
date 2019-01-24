
using FrameIO.Run;

namespace test_repeated
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAN2RMQvCMBCFd8H/UDI7iG5uVQMKpYoV92CvMZCkkl6FUvrf",
                "vVS0zeYoLknue493d6SdTqKIFU4YyE5sFbW+JpKBNGAxURUOlPiOx9sxGKzn",
                "5g6k+HJvESQ4Nhu7lLSQk6EQuoKxsla4KWvr+ywXgdAgHFxOQT7WCK2DRG6v",
                "Zd5HsqNTRqF6QGC4CF37keZv1n1UlvD0H9bg6de/ETsnmiC4JwlYiTeyLn5u",
                "zdejv+jonuyf+9KrAgAA");

            IORunner.InitialFromGZipBase64(config);
            
        }

        //获取一个通道
        public static FioChannel GetChannel(ChannelOption chops)
        {
            if (chops == null || !chops.Contains("$channeltype")) return null;
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
            IORunner.SendFrame(data.TheObject, ch);
        }

        //接收数据
        public static FioNetObject RecvFrame(string frame, FioChannel ch)
        {
            return new FioNetObject(IORunner.RecvFrame(frame, ch));
        }
    }
}
