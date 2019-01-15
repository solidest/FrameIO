
using System.Collections.ObjectModel;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System.Linq;
using System;

namespace test_enum
{
    public partial class testenum
    {

        //属性声明
        public Parameter<uint?> datetype { get; private set;}
        public Parameter<uint?> name1 { get; private set;}
        public Parameter<uint?> name2 { get; private set;}
        public Parameter<uint?> age1 { get; private set;}
        public Parameter<uint?> age2 { get; private set;}

        //属性初始化
        public void InitialParameter()
        {
            datetype = new Parameter<uint?>();
            name1 = new Parameter<uint?>();
            name2 = new Parameter<uint?>();
            age1 = new Parameter<uint?>();
            age2 = new Parameter<uint?>();
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
        public void A_Send_Type1()
        {
            var __v__ = FioNetRunner.NewFrameObject("Frame_Send_Type1");
            __v__.SetValue("DATATYPE", datetype);
            __v__.SetValue("NAME1", name1);
            __v__.SetValue("NAME2", name2);
            FioNetRunner.SendFrame(__v__, CH_COM3);
        }

        //数据接收
        public void A_Send_Type2()
        {
            var __v__ = FioNetRunner.RecvFrame("Frame_Send_Type2", CH_COM4);
            __v__.GetValue("DATATYPE", datetype);
            __v__.GetValue("AGE1", age1);
            __v__.GetValue("AGE2", age2);
        }
        

        public void A_Recv()
        {
            var __v__ = FioNetRunner.RecvFrame("Frame_Recv", CH_COM4);
        switch((Enum_Type)DATATYPE) {

        case Enum_Type.enum_type1:

            switch((Enum_Type)__v__.GetValue("Frame_Recv.DATATYPE"))
            {
                case Enum_Type.enum_type1:
                {
                    __v__.GetValue("DAT.enum_type1.NAME1", name1);
                    __v__.GetValue("DAT.enum_type1.NAME2", name2);
                    break;
                }
                case Enum_Type.enum_type2:
                {
                    break;
                }
            }
        break;

        case Enum_Type.enum_type2:

        break;

        }

        }

    }
}
