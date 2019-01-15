
using FrameIO.Run;

namespace test_enum
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAO1WTWuDQBC9B/IfxHMPNb31ZhsbAm0NjRR6ChInsuCuYtZA",
                "CP737hrdD6PZll5aqxfdec9x38wb9TSdWJb9lIcYNmsg0SY4ZuDY99aJAwxa",
                "Q4yB0Ge0pzLK4nM3cIOPlacGJZ1nYQhfLgmFGHL7RmWhmEDECLsw2YOKPCD6",
                "mBaEP+tupgFHCn4esUQ8LQ6TRMvokW0aVSntVY4wougAGuE9TAq+pdsmVgrU",
                "fnVfPGcoQmZ/V8j5ojqdRdXGfIPtYYCWDFKPFJjj/FxN3pc7zbQaZfoE/N0i",
                "T4tMSzuHjA26T+obOFlUTuVVt7eLXCHAt0v1F4VpH0sKWE2vbKXR6bThnkYL",
                "vGNuv9d1ye7rvmD0uUASrrpB0EyuEMSL7jdHecHumvv/UAg9oC31IknHdlTp",
                "B45t6zc61l2Mhq3rMPpV86tciMve76H4UVNqOH4VO8fr9+o06BjIb9x0Un4C",
                "+P87ymMMAAA=");

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
