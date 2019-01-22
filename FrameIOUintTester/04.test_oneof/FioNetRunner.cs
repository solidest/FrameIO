
using FrameIO.Run;

namespace test_enum
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAO1Vz2vCMBS+C/4PpWcP091262YnwrbKLIOdpNhnCTRpiakg",
                "0v99SWzzo1o78DSWXJq87+vLy/c+eKfxyPP8V5pg2KyBpP6TdxIhHlxDhoGw",
                "N7RnOsrj8yAO4u9VaAY1PT6WwBFxXBIGGVB/YrJQRkDcskvyPZjIM2IvRUXE",
                "XY8zCzgyiGjKE4m0OMlzK2NItkUqU/orijBi6AAWIS5CUmGBi+9GFmjiX0le",
                "iZIf2lg9Md86+MyIQLRb0KIqrbRzKLmeEWl+EGSlnMmTv3dFlgiIchm/aNqF",
                "+utYMsBmeqOU9p3TLtzTaIV/BO/hlRJ+33XN7uu+YvS5QBNuukHRhlyhiBfd",
                "b1d9wZZCzP6fEHbAOtoiacdeUekOx3bfP+jYYOEM2+jg/Gr5VR/UttnIz1nD",
                "Zh5+wvbg5qGbh24eiuXmYSuEm4d/o09uHp51uGcejkf1D6eO4TwjDgAA");

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
