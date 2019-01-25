
using System.Collections.ObjectModel;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System;

namespace test_udp_10473
{
    public partial class testtcp10473
    {

        //属性声明
        public Parameter<short?> head { get; private set;}
        public ObservableCollection<Parameter<byte?>> len { get; private set; }
        public Parameter<byte?> end { get; private set;}

        //属性初始化
        public void InitialParameter()
        {
            head = new Parameter<short?>();
            len = new ObservableCollection<Parameter<byte?>>(); for (int i = 0; i < 10470; i++) len.Add(new Parameter<byte?>());
            end = new Parameter<byte?>();
        }

        //通道声明
        public FioChannel CH_UDP;
        
        //通道初始化
        public void InitialChannelCH_UDP(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("localip")) ops.SetOption("localip", "192.168.0.151");
            if (!ops.Contains("localport")) ops.SetOption("localport", 8007);
            if (!ops.Contains("remoteip")) ops.SetOption("remoteip", "192.168.0.151");
            if (!ops.Contains("remoteport")) ops.SetOption("remoteport", 8008);
            ops.SetOption("$channeltype", 5);
            CH_UDP = FioNetRunner.GetChannel(ops);
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
            var __v__ = FioNetRunner.RecvFrame("frameSR", CH_UDP);
            __v__.GetValue("HEAD", head);
            __v__.GetValue("LEN", len);
            __v__.GetValue("END", end);
        }

    }
}
