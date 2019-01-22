
using FrameIO.Run;

namespace test_subsys_Array
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAN1UsW7CMBDdkfiHyHOHqmxstKS0KAIEVTu7zWFcOXbk2KAI",
                "8e+1jWjixoIgNUM7Jffu5XLv7tn7fi+K0FriDFZLNIz2NjbICkgGXCW0UBVq",
                "8Kd4NK4DFfWlzMFkbPjMFRCQ6KbOooRDaghrzAqoZ+6pehCa2/8M7rxEqWAu",
                "U1PIls0wY17FmH+I1JVEC0kzqugWPMIrZtq2dHvCDt9ZlMSz/yBj+nZ5GxMp",
                "dD6SEpe+kvCGXe6TcpLqn3Cw+hKwp8fRLHjiPDKBVYNxfiitBhMejj8gx9kB",
                "/S0xY6HfWbOLbtVUQV0YcitNgBO1sZ4LuCOe/eGzenxxj6MklIvCfCh4i2sq",
                "ZOAW+z5v3E5PcsCk1zbcNGfHa+n3Dl8+G0HUQAYAAA==");

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
