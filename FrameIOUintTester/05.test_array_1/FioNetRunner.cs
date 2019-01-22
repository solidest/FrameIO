
using FrameIO.Run;

namespace SingleByteArray
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAM2RwQuCMBjF70H/g+zcIezWzWpQIBYZ3Ud+rsE2Y34GIv7v",
                "bUrpTnn0su37vcfbHmuWiyAguWEK0ivZBo2bLUmBK9AYixIHavmRRocxGKy3",
                "+gVWceNJI3AwZDV2Ca4hs4acyRLGyk7gvqi0u2cTekKNcDaZDXKxiknpJVL9",
                "KLIuklyMUALFGzzDncnKPWn9Ze1PJTFNptaIjGG1F9yRGDTHp7WGM65Jk8m/",
                "9a9m//Wz69ofus0u7Qf82Qns0AIAAA==");

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
