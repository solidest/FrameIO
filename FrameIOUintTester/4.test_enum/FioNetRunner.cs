
using FrameIO.Run;

namespace test_enum
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAO1Vz2uDMBS+F/o/SM47zO62m1tdKXSzrDLYqUh9lYCJksZC",
                "Kf7vS6zmh9W5sdPaeknyvi/P9+N7ehyPHAe9sIjAegU0XoeHHFz06BwlIKAV",
                "JAQoX+Ad11Zhn3qhF34ufdOo6dKLQORxTjkkwNCdycIJhVgQtlG6AxN5wvw5",
                "K6h818PEAg4cAhYLR9ItidLU8ujTTRZXLtGSYYI53oNF+IjSQoZ039hKhaI3",
                "79V3LyWRyf9N5LSpllNStTDfYbO/QEmGmU8LInG5VpP3406LXAfTDCgE2xnL",
                "itxyO4VcDHpA6wuSrCpn8qrr7SJXCMhwuf2hGIpjzoGY7o1QmjzdNtzTaIV3",
                "zO3vuq7Zfd1XjD4VaMK3alC0IVUo4ln3m6c8Y3fN/TUUwjZYR7tIWrEdVfqD",
                "Ytv5DyrWm90EW9fhpldLr/qgttb/cDwqvwAJMUvyrAkAAA==");

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
