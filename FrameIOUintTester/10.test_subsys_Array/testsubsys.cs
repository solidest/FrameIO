
using System.Collections.ObjectModel;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System;

namespace test_subsys_Array
{
    public partial class testsubsys
    {

        //属性声明
        public Parameter<uint?> head { get; private set;}
        public Parameter<uint?> len { get; private set;}
        public Parameter<uint?> end { get; private set;}
        public ObservableCollection<position> pos { get; private set; }

        //属性初始化
        public void InitialParameter()
        {
            head = new Parameter<uint?>();
            len = new Parameter<uint?>();
            end = new Parameter<uint?>();
            pos = new ObservableCollection<position>(); for(int i=0; i<3; i++) pos.Add(new position());
        }

        //通道声明
        public FioChannel CH_UDP_SEND;
        public FioChannel CH_UDP_RECV;

        //通道初始化
        public void InitialChannelCH_UDP_SEND(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("localip")) ops.SetOption("localip", "192.168.0.151");
            if (!ops.Contains("localport")) ops.SetOption("localport", 8007);
            if (!ops.Contains("remoteip")) ops.SetOption("remoteip", "192.168.0.151");
            if (!ops.Contains("remoteport")) ops.SetOption("remoteport", 8008);
            ops.SetOption("$channeltype", 5);
            CH_UDP_SEND = FioNetRunner.GetChannel(ops);
        }
        

        public void InitialChannelCH_UDP_RECV(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("localip")) ops.SetOption("localip", "192.168.0.151");
            if (!ops.Contains("localport")) ops.SetOption("localport", 8008);
            if (!ops.Contains("remoteip")) ops.SetOption("remoteip", "192.168.0.151");
            if (!ops.Contains("remoteport")) ops.SetOption("remoteport", 8007);
            ops.SetOption("$channeltype", 5);
            CH_UDP_RECV = FioNetRunner.GetChannel(ops);
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
            {
                var __vvs__ = new Collection<FioNetObject>();
                for (int i = 0; i < pos.Count; i++)
                {
                    var __vv__ = new FioNetObject();
                    __vv__.SetValue("jingdu", pos[i].jingdu);
                    __vv__.SetValue("weidu", pos[i].weidu);
                    __vvs__.Add(__vv__);
                }
                __v__.SetValue("JWD", __vvs__);
            }
            __v__.SetValue("END", end);
            FioNetRunner.SendFrame(__v__, CH_UDP_SEND);
        }

        //数据接收
        public void A_Recv()
        {
            var __v__ = FioNetRunner.RecvFrame("frameSR", CH_UDP_RECV);
            __v__.GetValue("HEAD", head);
            __v__.GetValue("LEN", len);
            __v__.GetValue("END", end);
        }

    }
}
