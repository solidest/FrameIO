
using System.Collections.ObjectModel;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System;

namespace Tcpserver
{
    public partial class Tcpserver
    {

        //属性声明
        public Parameter<uint?> len { get; private set;}
        public Parameter<uint?> end { get; private set;}
        public Parameter<uint?> head { get; private set;}

        //属性初始化
        public void InitialParameter()
        {
            len = new Parameter<uint?>();
            end = new Parameter<uint?>();
            head = new Parameter<uint?>();
        }

        //通道声明
        public FioChannel CHS;
        public FioChannel CHS_2;
        public FioChannel CHC;
        public FioChannel CHC_2;
        
        //通道初始化
        public void InitialChannelCHS(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("serverip")) ops.SetOption("serverip", "192.168.0.151");
            if (!ops.Contains("port")) ops.SetOption("port", 8008);
            if (!ops.Contains("clientip")) ops.SetOption("clientip", "192.168.0.151");
            ops.SetOption("$channeltype", 3);
            CHS = FioNetRunner.GetChannel(ops);
        }
        

        public void InitialChannelCHS_2(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("serverip")) ops.SetOption("serverip", "192.168.0.151");
            if (!ops.Contains("port")) ops.SetOption("port", 8007);
            if (!ops.Contains("clientip")) ops.SetOption("clientip", "192.168.0.151");
            ops.SetOption("$channeltype", 3);
            CHS_2 = FioNetRunner.GetChannel(ops);
        }
        

        public void InitialChannelCHC(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("serverip")) ops.SetOption("serverip", "192.168.0.151");
            if (!ops.Contains("port")) ops.SetOption("port", 8008);
            ops.SetOption("$channeltype", 4);
            CHC = FioNetRunner.GetChannel(ops);
        }
        

        public void InitialChannelCHC_2(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("serverip")) ops.SetOption("serverip", "192.168.0.151");
            if (!ops.Contains("port")) ops.SetOption("port", 8007);
            ops.SetOption("$channeltype", 4);
            CHC_2 = FioNetRunner.GetChannel(ops);
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
        public void A_Send()
        {
            var __v__ = FioNetRunner.NewFrameObject("frameSR");
            __v__.SetValue("HEAD", head);
            __v__.SetValue("LEN", len);
            __v__.SetValue("END", end);
            FioNetRunner.SendFrame(__v__, CHS);
        }
        

        public void A_Send_2()
        {
            var __v__ = FioNetRunner.NewFrameObject("frameSR");
            __v__.SetValue("HEAD", head);
            __v__.SetValue("LEN", len);
            __v__.SetValue("END", end);
            FioNetRunner.SendFrame(__v__, CHS_2);
        }

        //数据接收
        public void A_Recv()
        {
            var __v__ = FioNetRunner.RecvFrame("frameSR", CHC);
            __v__.GetValue("HEAD", head);
            __v__.GetValue("LEN", len);
            __v__.GetValue("END", end);
        }
        

        public void A_Recv_2()
        {
            var __v__ = FioNetRunner.RecvFrame("frameSR", CHC_2);
            __v__.GetValue("HEAD", head);
            __v__.GetValue("LEN", len);
            __v__.GetValue("END", end);
        }

    }
}
