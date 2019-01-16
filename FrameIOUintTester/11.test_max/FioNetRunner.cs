
using FrameIO.Run;

namespace test_max
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAFWOQQvCIBiG70H/QTzHcEWXbhUdgqKg6C75TT5QF85FY+y/",
                "pxsyvajf87y8n/1yQQitLNdQ0h3pw+jBA6QG4y7YuJlOfJ+COfrsPuBNGM/G",
                "gQRLV2kKpQHhAxVXDaTmgO5Ytybs2awz0Tm4WeGLQq3mSmWNJ/OuxVhJ7xY1",
                "OvxCFnhx1YYvsRRe0US+LXLDf9GUrGDRDNNjvPwx/AH4qvH4MQEAAA==");

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
