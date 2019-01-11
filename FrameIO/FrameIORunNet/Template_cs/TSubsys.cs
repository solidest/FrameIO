
//using System.Collections.ObjectModel;
//using FrameIO.Run;
//using FrameIO.Interface;
//using System.Diagnostics;
//using System.Linq;
//using System;

//namespace main
//{
//    public partial class System_A
//    {

//        //属性声明
//        public Parameter<uint?> _CommandSize { get; private set; }
//        public Parameter<uint?> _WidgetIdent { get; private set; }
//        public Parameter<uint?> _UnusedPad { get; private set; }
//        public Parameter<uint?> _ParameterIdent { get; private set; }
//        public Parameter<byte?> _Parameter_3Byte { get; private set; }
//        public ObservableCollection<Parameter<byte?>> _Parameter_ArrayByte { get; private set; }
//        public CreateTime Name { get; private set; }
//        public ObservableCollection<Parameter_1Byte> _Parameter_1Byte { get; private set; }
//        public ObservableCollection<Parameter_2Byte> _Parameter_2Byte { get; private set; }
//        public Parameter_null Name { get; private set; }
//        public Parameter_9Byte Name { get; private set; }

//        //属性初始化
//        private void InitialParameter()
//        {
//            _CommandSize = new Parameter<uint?>();
//            _WidgetIdent = new Parameter<uint?>();
//            _UnusedPad = new Parameter<uint?>();
//            _ParameterIdent = new Parameter<uint?>();
//            _Parameter_3Byte = new Parameter<byte?>();
//            _Parameter_ArrayByte = new ObservableCollection<Parameter<byte?>>();
//            _CreateTime = new CreateTime();
//            _Parameter_1Byte = new ObservableCollection<Parameter_1Byte>();
//            _Parameter_2Byte = new ObservableCollection<Parameter_2Byte>();
//            _Parameter_null = new Parameter_null();
//            _Parameter_9Byte = new Parameter_9Byte();
//        }

//        //通道声明
//        public FioChannel CH_UDP_SEND;
//        public FioChannel CH_UDP_RECV;

//        //通道初始化
//        public void InitialChannelCH_UDP_SEND(ChannelOption ops)
//        {
//            if (ops == null) ops = new ChannelOption();
//            if (!ops.Contains("localip")) ops.SetOption("localip", "192.168.0.153");
//            if (!ops.Contains("localport")) ops.SetOption("localport", 8007);
//            if (!ops.Contains("remoteip")) ops.SetOption("remoteip", "192.168.0.153");
//            if (!ops.Contains("remoteport")) ops.SetOption("remoteport", 8008);
//            ops.SetOption("$channeltype", 5);
//            CH_UDP_SEND = FioNetRunner.GetChannel(ops);
//        }


//        public void InitialChannelCH_UDP_RECV(ChannelOption ops)
//        {
//            if (ops == null) ops = new ChannelOption();
//            if (!ops.Contains("localip")) ops.SetOption("localip", "192.168.0.153");
//            if (!ops.Contains("localport")) ops.SetOption("localport", 8008);
//            if (!ops.Contains("remoteip")) ops.SetOption("remoteip", "192.168.0.153");
//            if (!ops.Contains("remoteport")) ops.SetOption("remoteport", 8007);
//            ops.SetOption("$channeltype", 5);
//            CH_UDP_RECV = FioNetRunner.GetChannel(ops);
//        }

//        //异常处理接口
//        private void HandleFrameIOError(Exception ex)
//        {
//            if (ex.GetType() == typeof(FrameIOException))
//            {
//                switch (((FrameIOException)ex).ErrType)
//                {
//                    case FrameIOErrorType.ChannelErr:
//                    case FrameIOErrorType.SendErr:
//                    case FrameIOErrorType.RecvErr:
//                    case FrameIOErrorType.CheckDtaErr:
//                        Debug.WriteLine("位置：{0}    错误：{1}", ((FrameIOException)ex).Position, ((FrameIOException)ex).ErrInfo);
//                        break;
//                }
//            }
//            else
//                Debug.WriteLine(ex.ToString());
//        }

//        //数据发送


//        //数据接收


//    }
//}
