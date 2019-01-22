
using FrameIO.Run;

namespace test_tcp
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAKvm5VJQUEorSsxNDQ5SslKoBvGBIsGp6bmpeSU+mcUlCFGg",
                "uIerowuyAEJpSGVBKlAGxPXMK0lNTy1S0kFWlZmel5oCVJCWmFOciizjlFni",
                "nF+aB7LH2AhForIk1b8oBWgQyNjcxJwcFBNd85LzU8BGKgUUZeZmlmSWpaIo",
                "CEvMKQU5yQAmVguXVfJx9RsO3nD1G8KxAWGAKSBRCwCe12EEiwIAAA==");

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
