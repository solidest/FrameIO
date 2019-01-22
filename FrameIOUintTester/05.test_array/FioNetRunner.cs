
using FrameIO.Run;

namespace test_array
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAFWOzQrCMBCE74LvEHr2IIoXb1oqCKUVWrxKsGsMJKlst0Ip",
                "fXeTFGly2Z9vhtkd1yvGkgtyDY8TIh+SIxsds7QCocFQLjtaqOVpWdRZUYds",
                "cdfDB6zi1qshEIBz7CawepKDEfS21kMoVVIYaCx9cdVBqJwlpW1v3Cv7XSQM",
                "BCU2gP6s5kpFxzLzbBsfmdxQaknyC5HhzlXvXt7+2TQPvtky/QDoiHuVJAEA",
                "AA==");

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
