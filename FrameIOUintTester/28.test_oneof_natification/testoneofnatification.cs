
using System.Collections.ObjectModel;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System;

namespace test_oneof_natification
{
    public partial class testoneofnatification
    {

        //属性声明
        public Parameter<int?> head { get; private set;}
        public Parameter<int?> len { get; private set;}
        public Parameter<int?> end { get; private set;}

        //属性初始化
        public void InitialParameter()
        {
            head = new Parameter<int?>();
            len = new Parameter<int?>();
            end = new Parameter<int?>();
        }

        //通道声明
        public FioChannel CH_UDP_SEND;
        public FioChannel CH_UDP_RECV;
        
        //通道初始化
        public void InitialChannelCH_UDP_SEND(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("localip")) ops.SetOption("localip", "127.0.0.1");
            if (!ops.Contains("localport")) ops.SetOption("localport", 8007);
            if (!ops.Contains("remoteip")) ops.SetOption("remoteip", "127.0.0.1");
            if (!ops.Contains("remoteport")) ops.SetOption("remoteport", 8008);
            ops.SetOption("$channeltype", 5);
            CH_UDP_SEND = FioNetRunner.GetChannel(ops);
        }
        

        public void InitialChannelCH_UDP_RECV(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("localip")) ops.SetOption("localip", "127.0.0.1");
            if (!ops.Contains("localport")) ops.SetOption("localport", 8008);
            if (!ops.Contains("remoteip")) ops.SetOption("remoteip", "127.0.0.1");
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
        public void A_Send(GRADE frameSR_GRADEmenu, oneofGrade frameSR_grade_G2_F2branch)
        {
            var __v__ = FioNetRunner.NewFrameObject("frameSR");
            __v__.SetValue("GRADEmenu", (int)frameSR_GRADEmenu);
            __v__.SetValue("HEAD", head);
            __v__.SetValue("END", end);
            switch(frameSR_GRADEmenu)
            {
                case GRADE.G1:
                {
                    break;
                }
                case GRADE.G2:
                {
                    __v__.SetValue("grade.G2.WEHEAD", len);
                    __v__.SetValue("grade.G2.F2branch", len);
                    switch(frameSR_grade_G2_F2branch)
                    {
                        case oneofGrade.A1:
                        {
                            __v__.SetValue("grade.G2.OneofGradeMenu.A1.SCORE", len);
                            break;
                        }
                        case oneofGrade.A2:
                        {
                            __v__.SetValue("grade.G2.OneofGradeMenu.A2.THREE", len);
                            break;
                        }
                    }
                    break;
                }
                case oneofGrade.G3:
                {
                    break;
                }
            }
            FioNetRunner.SendFrame(__v__, CH_UDP_SEND);
        }
        

        public void A_Recv(GRADE frameSR_GRADEmenu, oneofGrade frameSR_grade_G2_F2branch)
        {
            var __v__ = FioNetRunner.NewFrameObject("frameSR");
            __v__.SetValue("GRADEmenu", (int)frameSR_GRADEmenu);
            __v__.SetValue("HEAD", head);
            __v__.SetValue("END", end);
            switch(frameSR_GRADEmenu)
            {
                case GRADE.G1:
                {
                    break;
                }
                case GRADE.G2:
                {
                    __v__.SetValue("grade.G2.WEHEAD", len);
                    __v__.SetValue("grade.G2.F2branch", len);
                    switch(frameSR_grade_G2_F2branch)
                    {
                        case oneofGrade.A1:
                        {
                            __v__.SetValue("grade.G2.OneofGradeMenu.A1.SCORE", len);
                            break;
                        }
                        case oneofGrade.A2:
                        {
                            __v__.SetValue("grade.G2.OneofGradeMenu.A2.THREE", len);
                            break;
                        }
                    }
                    break;
                }
                case oneofGrade.G3:
                {
                    break;
                }
            }
            FioNetRunner.SendFrame(__v__, CH_UDP_RECV);
        }

        //数据接收
        

    }
}
