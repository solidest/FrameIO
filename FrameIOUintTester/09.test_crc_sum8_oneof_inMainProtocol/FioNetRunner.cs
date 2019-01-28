
using FrameIO.Run;

namespace test_crc_sum8_oneof_inMainProtocol
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAO1WsW6DMBDdI+UfkOcObbpU3WhC0khJiUqaNULhQq2CHRHT",
                "Kor492ITjA0BOnShhQV873H2vbuDOw8HhoH2kRuC84oejTNfpxYH/BAIW+Aj",
                "K6yZfRq4vmor2OvTAVKEL+eEgQ8RulFZ2CfgpYS9GxxBRZ4wG9OY8K3uRxpw",
                "YmBHXuqIuw3dINA8WmRHPeESrSIcYoY/QSOsqUXikOOQ3rdAPA3euEHMT3yb",
                "2xKJoom5bo3SJmDvZxGND5rbCRzSnWxyeeFCFrqpNPF2WWGBUAJlW/3+cwah",
                "6lc5Qh7fXRmuSa+Km9eAn+a6YNflXDLqcl8QGmtA0tpqQRIrSc+vpMJGz5Y5",
                "6XUw0MJ6+X8y6AZtqUuE2Bf91X4tB973a9+vfb+2ydDUr8VCPir/+vE77D4w",
                "WbqYdGWykTKoRhGHPKs1W222ztvyAVU404iKqYgPOVUHNIekVNmDuGWyZSPj",
                "VhtUmoZGs3O6XimT6telm3FUvg5dCqOuFLV/cF+KHYnjz5TicJB8A7ofYhlM",
                "DwAA");

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
