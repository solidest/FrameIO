
using System.Collections.ObjectModel;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System;

namespace test_array
{
    public partial class testarray
    {

        //属性声明
        public ObservableCollection<Parameter<uint?>> content { get; private set; }

        //属性初始化
        public void InitialParameter()
        {
            content = new ObservableCollection<Parameter<uint?>>(); for (int i = 0; i < 5; i++) content.Add(new Parameter<uint?>());
        }

        //通道声明
        public FioChannel CH_COM3;
        public FioChannel CH_COM4;

        //通道初始化
        public void InitialChannelCH_COM3(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("portname")) ops.SetOption("portname", "COM3");
            if (!ops.Contains("baudrate")) ops.SetOption("baudrate", 9600);
            if (!ops.Contains("parity")) ops.SetOption("parity", 0);
            if (!ops.Contains("databits")) ops.SetOption("databits", 8);
            if (!ops.Contains("stopbits")) ops.SetOption("stopbits", 1);
            ops.SetOption("$channeltype", 1);
            CH_COM3 = FioNetRunner.GetChannel(ops);
        }
        

        public void InitialChannelCH_COM4(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("portname")) ops.SetOption("portname", "COM4");
            if (!ops.Contains("baudrate")) ops.SetOption("baudrate", 9600);
            if (!ops.Contains("parity")) ops.SetOption("parity", 0);
            if (!ops.Contains("databits")) ops.SetOption("databits", 8);
            if (!ops.Contains("stopbits")) ops.SetOption("stopbits", 1);
            ops.SetOption("$channeltype", 1);
            CH_COM4 = FioNetRunner.GetChannel(ops);
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
            var __v__ = FioNetRunner.NewFrameObject("Frame_Array");
            __v__.SetValue("CONTENT", content);
            FioNetRunner.SendFrame(__v__, CH_COM3);
        }
        

        public void A_Recv()
        {
            var __v__ = FioNetRunner.NewFrameObject("Frame_Array");
            __v__.SetValue("CONTENT", content);
            FioNetRunner.SendFrame(__v__, CH_COM4);
        }

        //数据接收
        

    }
}
