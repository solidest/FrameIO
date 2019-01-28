
using FrameIO.Run;

namespace test_crc_sum8_oneof
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAO1WTW+CQBC9m/gfyJ576Mel6c0qWhNbTKVeDZGRksKuwaWN",
                "Mf737q4CuywrmvRQDFxg5z1mmTfM7Oy6HctCq8SLYfaOnqwdXzPLDIIYMJ2E",
                "G1pYD/Zh5AWyrWC72zUwhC/HmEIACbqRWWGAwWeElRdtQEaeQ9onKeZbPdwr",
                "wJaCk/jMEXcbe1GkeLTxkvjCJZomYRzS8BsUgktsnMYcB3ZfAPYVeO5FKf/i",
                "28y2z1E06Lm1UToYnNUoIelacTuANdvJwccXjmShm0wTb5cVFgjBULaZ9x9T",
                "iGW/0idk8d2VYUN6ZbxXBZyb64JtynnOMOW+IJz8B3Ja3b+QE7WkZ9deY6MX",
                "uzdodbDQxH5rZchK7uqVqAL7n7D8ymOzR9P5Yvbx+og01VSDslQVRfSH/GmX",
                "K4vUdrm2y7Vd7nIZWHRuVWlemRKXdDkjd5gQMV6KEjJ75GqK3+uidlks8sfj",
                "g7gdUncY3hfKyHhqfC91vP87u58azvWO1cw4tI7TzDCq5qIGRCIb6+YbY+Ep",
                "U0xbeA2J43oKTzuqGxDJOYWncUxHremIVSq229n/AjXgnNnrEgAA");

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
