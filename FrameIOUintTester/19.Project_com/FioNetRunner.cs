
using FrameIO.Run;

namespace Project_com
{
    public static class FioNetRunner
    {
        //初始化
        static FioNetRunner()
        {
            var config = string.Concat(

                "H4sIAAAAAAAEAO2aXU/iQBSG7zfZ/2B67YWg65K9pNUq6oKW9euGnKXHdiKd",
                "krbjxhj+uzOFTj+wYhNIdDlcUDrnZXrOM29nKO3L9287O4Yj/sbPcYLB6ArH",
                "yJ7wOIIAjV87LyosBReQjH25395vtfd2i43d5wTjc+QqmAUc9ALkyTmLk7wP",
                "2X6C4GJUbMrFw+epOqDaPeUJelK3W1Qxj6MrBQ8wibEY6bLEDAVXR2odlgIy",
                "s340P6DhBDCZlHo84uPQTbs0BhELWCLLLgmuYSJUSntZ20xHjQUm1wyDALi7",
                "uZI666xoGB5xEaRfrg74opD4wwQGMH7ERA68l/hfpPz3yrEggZVl9Dn2H+wo",
                "FNNSvxZOkbt9vviCElf9UZSnvVTPjTRyx3oM+JBBOGDcOwZ+JlhVVJ/YqRzO",
                "4oEKuWWVt6vhmjO1YUYfH/XVo68VNS7I4++6QctWuUILl9yRvWZLajm8MUZP",
                "6G4dinJDabeMybj3mQW8x+yQe/ciPFMbn23A0wdNPd0ss5rs/pcBJW9rFE28",
                "LYB7Q2COL7ddCNdq6MOmhrbk1GwDOwN2yXRqWzd8y052fPnrRK1fnlq8JBOC",
                "k2uzNV1OfUNBZAraE4lCLQp0PlW0lYWT0ORaW6G4FWjKjfKOOrsI0xImORlf",
                "4SUZaJmM6QvHZ4TmrR+k8G+UrVam4mIDv2S36yWTRGIVmP3q1fMnJXNHZFIy",
                "xTn5N7Ib5JYgNG+hkTMPoVmgUUhugNyyjIRcMteaPjp+KGiJzrW2pCFhWMAu",
                "gHAYXQh76aU1OSTT3qc3EohIrs3+Z20RC82iTSw0i31ioVkcEAvN4gex0CwO",
                "iYVm8ZNYaBadrWNRbnjv5mXx2j+9ynV8tDbzuElrrxpfdSuzaXI1Ca5tVFvV",
                "m7FzwSewODm8DlPJROmfFht0eOMnqpomV5MgOXy7HJ7v6I+FpxdNH8ePcfpo",
                "59d6tLbYmBahcz2yB9cj589Fx9D1zj+kG/k2ewUZRF67uywAAA==");

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
