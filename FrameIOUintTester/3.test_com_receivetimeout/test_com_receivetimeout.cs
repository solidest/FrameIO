using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.Runtime;
using FrameIO.Interface;
using System.Diagnostics;
using System.Linq;

namespace test_com_receivetimeout
{
    public partial class test_com_receivetimeout
    {
        public IChannelBase CH_COM3;
        public IChannelBase CH_COM4;

         
        public void InitialChannelCH_COM3(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("portname")) ops.SetOption("portname", "COM3");
            if (!ops.Contains("baudrate")) ops.SetOption("baudrate", 9600);
            if (!ops.Contains("parity")) ops.SetOption("parity", 0);
            if (!ops.Contains("databits")) ops.SetOption("databits", 8);
            if (!ops.Contains("stopbits")) ops.SetOption("stopbits", 1);
            if (!ops.Contains("receivetimeout")) ops.SetOption("receivetimeout", 5000);
            CH_COM3 = FrameIOFactory.GetChannel(ChannelTypeEnum.COM, ops);
        }
 
        public void InitialChannelCH_COM4(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if (!ops.Contains("baudrate")) ops.SetOption("baudrate", 9600);
            if (!ops.Contains("portname")) ops.SetOption("portname", "COM4");
            if (!ops.Contains("parity")) ops.SetOption("parity", 0);
            if (!ops.Contains("databits")) ops.SetOption("databits", 8);
            if (!ops.Contains("stopbits")) ops.SetOption("stopbits", 1);
            if (!ops.Contains("receivetimeout")) ops.SetOption("receivetimeout", 5000);
            CH_COM4 = FrameIOFactory.GetChannel(ChannelTypeEnum.COM, ops);
        }


        public Parameter<uint?> head { get; set;} = new Parameter<uint?>();
        public Parameter<uint?> len { get; set;} = new Parameter<uint?>();
        public Parameter<uint?> end { get; set;} = new Parameter<uint?>();

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
                var data = new FrameSRSettor();
                data.HEAD = head.Value;
                data.LEN = len.Value;
                data.END = end.Value;
                CH_COM3.WriteFrame(data.GetPacker());
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
                var data = new FrameSRGettor(CH_COM4.ReadFrame(FrameSRGettor.Unpacker));
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
