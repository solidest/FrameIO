
using FrameIO.Run;

namespace test_real_max_min
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAN2RPwvCMBDFd8HvUDJLqYqLm38iCrVKK+7BXksgSSVNxVL6",
                "3U0qtcnk5OKS5H7v8e6ONOOR56FMEg5JjJZeY2pNEsg5CBXSUg1U8z1ebW0w",
                "WC/1HbRiyoNQkINEE9tFcwGpNmSElWAra6o2RSVMn/nMEWoFJ5nqIBPLCWNO",
                "Iha3Iu0i0VlSThV9gGO4ElaZkYKetR8VhTj6hzVw9P03YiBuRwN6fccKotBP",
                "xrXhkYqeL3xXIc9emQaBPyz5fnSXPtoXEIC1q6kCAAA=");

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
