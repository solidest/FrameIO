
using FrameIO.Run;

namespace test_subsys
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAN1UPW/CMBDdkfgPkecOVdnYKKR8CEEFFZ1Nc0RGjh05dhFC",
                "+e+1jSBxY0GQyABTcu9eXu7dnX1ot4IAbQROYLlA3eBgYo0sIU6AySnJZIFq",
                "fBT2BmWgoH7tU9AZE46ZhBgEeimzSMwg0oQNphmUM+9E9rli5j+dNyexlzAX",
                "kRYysgmm1FEM2Q+PrCT6FCQhkvyCQ1hhqkxJrycsP2fRNJw9g43J9/VpDAVX",
                "qWvCP1yb2xIWR+o/7BVeAHasWJoBT5wPyrGsMC73o1ZP/H1xe2M5OyD3MjPg",
                "ak2rVTTrpghyz/TD2QOfxeOLfRwtoZRn+kPOalxDvi2tMdTL29noSfVs4q0F",
                "Vzew4bG0W/kfZe8/dSAGAAA=");

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
