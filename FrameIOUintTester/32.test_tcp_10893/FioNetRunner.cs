
using FrameIO.Run;

namespace test_tcp_1000
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAL1RTQuCQBC9B/0H2bMHjT6km5ZQYBYZ3ZccbUHXWNdAxP/e",
                "rGHqrQ522d157/Fm3mw1nWgaiQRNITiTtVapGpEDlbc71rOFpfchp5SQe8AV",
                "1RIBxClw6bFcdg6I71x72wc66aV8ADKq3HMJMQii91Us5hCiIKJJDn3GYXKT",
                "FVz1MZcDAuc6ihCNlG1Kk2Tg6PJbFjaW5CRYyiR7wkBwpUmhRjJarP6wxHP9",
                "b2PYQtByYNwguLBYqnWaxnxl/BzV+ldS1x/xw0ZO8X40Fx71C4gs14TZAgAA");

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
