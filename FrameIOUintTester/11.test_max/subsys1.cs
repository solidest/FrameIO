
using System.Collections.ObjectModel;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System;

namespace test_max
{
    public partial class subsys1
    {

        //属性声明
        public Parameter<int?> A { get; private set;}

        //属性初始化
        public void InitialParameter()
        {
            A = new Parameter<int?>();
        }

        //通道声明
        public FioChannel CH_SEND;
        public FioChannel CH_RECV;
        
        //通道初始化
        public void InitialChannelCH_SEND(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("localip")) ops.SetOption("localip", "192.168.0.151");
            if (!ops.Contains("localport")) ops.SetOption("localport", 8007);
            if (!ops.Contains("remoteip")) ops.SetOption("remoteip", "192.168.0.151");
            if (!ops.Contains("remoteport")) ops.SetOption("remoteport", 8008);
            if (!ops.Contains("waittimeout")) ops.SetOption("waittimeout", 5000);
            ops.SetOption("$channeltype", 5);
            CH_SEND = FioNetRunner.GetChannel(ops);
        }
        

        public void InitialChannelCH_RECV(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("localip")) ops.SetOption("localip", "192.168.0.151");
            if (!ops.Contains("localport")) ops.SetOption("localport", 8008);
            if (!ops.Contains("remoteip")) ops.SetOption("remoteip", "192.168.0.151");
            if (!ops.Contains("remoteport")) ops.SetOption("remoteport", 8007);
            if (!ops.Contains("waittimeout")) ops.SetOption("waittimeout", 5000);
            ops.SetOption("$channeltype", 5);
            CH_RECV = FioNetRunner.GetChannel(ops);
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
        public void A_SEND()
        {
            var __v__ = FioNetRunner.NewFrameObject("frame1");
            __v__.SetValue("SegA", A);
            FioNetRunner.SendFrame(__v__, CH_SEND);
        }

        //数据接收
        public void A_RECV()
        {
            var __v__ = FioNetRunner.RecvFrame("frame1", CH_RECV);
            __v__.GetValue("SegA", A);
        }

    }
}
