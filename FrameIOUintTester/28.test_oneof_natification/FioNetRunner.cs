
using FrameIO.Run;

namespace test_oneof_natification
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAO1WS2+CQBC+N+l/MJw9VLz1RnWrJm1p0LRnKiMlgcXg0sQY",
                "/nt3UXZ57C5YvUjKQdmZb4Z5fMxwuL8bDIxN4kawdIzHwYGdqWQJfgSYvAQ7",
                "IqRUPkfWtCwQ0NV+C1TDjgtMwIfEGJZRgY/Bo4CNG+6grHkKyCROMXvO2Kwo",
                "9gTsxKOOmNvIDcOKR4TXsZe7NN6TIApI8AMVwIcbpiykh0KWca2B3nqRho0h",
                "3swS14NXwGkfMpo51hRFN5TMKkY4jZg+D71zoj7rWmuStME2bXCcbiuOp7AF",
                "7Nn4ZMCfnheuDMzt62/xsdCjukgdwIJAVHZbiqFIcVRXKyaIKKwjE3fttECr",
                "Os4Rqs4LgJYBIuAWJnBgo+vFlVUFlWM2rHbHvGp36nm3ducTNSe9Jo7edKiB",
                "Np7Nr8TF6+++1kLMr5gvk4uLpttLmtLJpx03kEw93h0ZXjn8OMKSDMG2+GSv",
                "Wy1E1VCsu9bFdsRNbAfpEOcyUFi1MZEj2xgpgJ2YyeFdGcoNlAQsrkyukIqb",
                "pM2fYUnmLldeTghV/ToTYjV30D8hTgbXJURT2BCds8DHV13g43MXuIYot7+z",
                "/vaFJQ789nST/9Gf7BeSLrnjDw8AAA==");

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
