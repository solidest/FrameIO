
using System.Collections.ObjectModel;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System;

namespace test_crc_sum8_oneof_inMainProtocol
{
    public partial class test_crc_sum8_oneof_inMainProtocol
    {

        //属性声明
        public Parameter<uint?> flag { get; private set;}
        public Parameter<uint?> checkinMain { get; private set;}
        public frame_one one { get; private set; }
        public frame_two two { get; private set; }

        //属性初始化
        public void InitialParameter()
        {
            flag = new Parameter<uint?>();
            checkinMain = new Parameter<uint?>();
            one = new frame_one();
            two = new frame_two();
        }

        //通道声明
        public FioChannel CHS;
        public FioChannel CHC;
        
        //通道初始化
        public void InitialChannelCHS(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("serverip")) ops.SetOption("serverip", "192.168.0.151");
            if (!ops.Contains("port")) ops.SetOption("port", 8007);
            if (!ops.Contains("clientip")) ops.SetOption("clientip", "192.168.0.151");
            ops.SetOption("$channeltype", 3);
            CHS = FioNetRunner.GetChannel(ops);
        }
        

        public void InitialChannelCHC(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("serverip")) ops.SetOption("serverip", "192.168.0.151");
            if (!ops.Contains("port")) ops.SetOption("port", 8007);
            if (!ops.Contains("waittimeout")) ops.SetOption("waittimeout", 10000);
            ops.SetOption("$channeltype", 4);
            CHC = FioNetRunner.GetChannel(ops);
        }

        //异常处理接口
        private void HandleFrameIOError(Exception ex)
        {
            if (ex.GetType() == typeof(FrameIOException))
            {
                switch (((FrameIOException)ex).ErrType)
                {
                    case FrameIOErrorType.ChannelErr:
                    case FrameIOErrorType.SendErr:
                    case FrameIOErrorType.RecvErr:
                    case FrameIOErrorType.CheckDtaErr:
                        Debug.WriteLine("位置：{0}    错误：{1}", ((FrameIOException)ex).Position, ((FrameIOException)ex).ErrInfo);
                        break;
                }
            }
            else
                Debug.WriteLine(ex.ToString());
        }

        //数据发送
        

        //数据接收
        public void A_Recv()
        {
            var __v__ = FioNetRunner.RecvFrame("frameSR", CHC);
            __v__.GetValue("SegFlag", flag);
            switch((enum_end)__v__.GetValue("SegFlag"))
            {
                case enum_end.one:
                {
                    break;
                }
                case enum_end.two:
                {
                    break;
                }
            }
        }

    }
}
