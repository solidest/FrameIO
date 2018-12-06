using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.Runtime;
using FrameIO.Interface;
using System.Diagnostics;
using System.Linq;

namespace test_udp_receivetimeout
{
    public partial class test_udp_receivetimeout
    {
        public IChannelBase CH_UDP_SEND;
        public IChannelBase CH_UDP_RECV;

         
        public void InitialChannelCH_UDP_SEND(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("localip")) ops.SetOption("localip", "192.168.0.148");
            if (!ops.Contains("localport")) ops.SetOption("localport", 8007);
            if (!ops.Contains("remoteip")) ops.SetOption("remoteip", "192.168.0.148");
            if (!ops.Contains("remoteport")) ops.SetOption("remoteport", 8008);
            if (!ops.Contains("waittimeout")) ops.SetOption("waittimeout", 5000);
            CH_UDP_SEND = FrameIOFactory.GetChannel(ChannelTypeEnum.UDP, ops);
        }
 
        public void InitialChannelCH_UDP_RECV(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("localip")) ops.SetOption("localip", "192.168.0.148");
            if (!ops.Contains("localport")) ops.SetOption("localport", 8008);
            if (!ops.Contains("remoteip")) ops.SetOption("remoteip", "192.168.0.148");
            if (!ops.Contains("remoteport")) ops.SetOption("remoteport", 8007);
            if (!ops.Contains("waittimeout")) ops.SetOption("waittimeout", 5000);
            CH_UDP_RECV = FrameIOFactory.GetChannel(ChannelTypeEnum.UDP, ops);
        }


        public Parameter<uint?> head { get; private set;} = new Parameter<uint?>();
        public Parameter<uint?> len { get; private set;} = new Parameter<uint?>();
        public Parameter<uint?> end { get; private set;} = new Parameter<uint?>();

        //异常处理接口
        private void HandleFrameIOError(FrameIOException ex)
        {
            switch(ex.ErrType)
            {
                case FrameIOErrorType.ChannelErr:
                case FrameIOErrorType.SendErr:
                case FrameIOErrorType.RecvErr:
                case FrameIOErrorType.CheckDtaErr:
                    Debug.WriteLine("位置：{0}    错误：{1}", ex.Position, ex.ErrInfo);
                    break;
            }
        }

        
        public void A_Send()
        {
            try
            {
                var data = new frameSRSettor();
                data.HEAD = head.Value;
                data.LEN = len.Value;
                data.END = end.Value;
                CH_UDP_SEND.WriteFrame(data.GetPacker());
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }


        
        public void A_Recv()
        {
            try
            {
                var data = new frameSRGettor(CH_UDP_RECV.ReadFrame(frameSRGettor.Unpacker));
                head.Value = data.HEAD; 
                len.Value = data.LEN; 
                end.Value = data.END; 
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }


        

    }
}
