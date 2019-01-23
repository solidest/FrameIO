
using FrameIO.Run;

namespace test_oneof_before
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAO1WTWuDQBC9B/IfxHMONYVSerONDYG2hkYKPQUTJ7LgrmLW",
                "QAj+97p+7a6JtSVNwXa96M48Z+e9HeUdhgNN0x9jF8NyAcTT77QDC2XBBfgY",
                "CH1CW8qjRdwVAxzq7CPIMmw5IxR8iPWRiEI+AbbDxg22IGbuEX0IE8L2MW6k",
                "xJ6CHXtZIVYWu0EgVbTIOvTykvo8RhhRtAMJ8OYGCWvpqoqlI5HH6nI8bn+R",
                "xvoP0JiYjum8z63LUbke/yQXJ7RIglme3Zd5g9/g2knTJmBvpnGYRFLZCUTZ",
                "V2qT8gUGrpUTcfnrzU83zwBrl2YbGc1Uex8zClgsL7RS8TSa6ZbfR51/MZ+t",
                "Ey18/dQ5uu30a0TbFHDAp9NQw7qmogYenX51pUfoXIjx/xNCDkhLWSQ+sSdU",
                "OmNim/w7J9acqoEtdVDzKs0rX9SP5UN+KzQsXdYrrHfKZfXNniiXpVyWclm9",
                "+Vkrl1UKoVxWP85JuaxCh3Nc1nCQfgCTQfECzxIAAA==");

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
