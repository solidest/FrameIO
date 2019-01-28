
using FrameIO.Run;

namespace test_crc_sum8_oneof_array
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAO1XQW+CMBS+m/gfTM87bPOy7MYUnYkbZjKvhsgTyaCYWrYY",
                "439fKQotUJRly8TABfrexyvva7/H667d6nTQklg+TN/QY2cXjZllCo4PmI7d",
                "DU2tsX3gWY5oS9Hmdg3MEw1HmIIDBN2IKNfBYDPA0vI2IHqeXNoLQhxN1b2X",
                "HFsKBrFZoCisb3meFFHHi8DmIdGEuL5L3U+QAGag49CP/MDuc8C25J5ZXhh9",
                "8e3Rtk+8qK+ZJ7M0MBjLIQnCtRS2D2s2k4EPLxzAnDcRxt/OMsw9AYasTT3/",
                "iIIvxhU+4ZjfXdatWF7RrxU5zl3rFK1a8wShWvsUULoHEtipvZAAc4t+vPY5",
                "NHrWtX5VHjRCrG3hzNwzBuzQFXulMNma8zXWX/+Kru4V0hVL+OqFVuTsrWDx",
                "keSmDyez+fT95QHlWJMN0lBmFNGv4FerZpakpmo2VbOpmv9PF2PBLJJ6OWO1",
                "E1qVqqnEDkjA218uSXXEiE2+DSuV33SQPB4e+C1euvhwMZda2rLjRaaCXu7Z",
                "ouzwkK+AFaRcUvEuLc9c5fp5mt0LTrOoT6vBxhSNp/otpXClrqoRbiPcmgk3",
                "1yrUYGOeI9wcRvWrV/3iJcW3W/tvDmn8XgsUAAA=");

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
