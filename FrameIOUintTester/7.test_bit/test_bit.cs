using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.Runtime;
using FrameIO.Interface;
using System.Diagnostics;
using System.Linq;

namespace test_bit
{
    public partial class test_bit
    {
        public IChannelBase CHS;
		public IChannelBase CHC;

		 
		public void InitialChannelCHS(ChannelOption ops)
		{
			if (ops == null) ops = new ChannelOption();
			if (!ops.Contains("serverip")) ops.SetOption("serverip", "192.168.0.153");
			if (!ops.Contains("port")) ops.SetOption("port", 8007);
			if (!ops.Contains("clientip")) ops.SetOption("clientip", "192.168.0.153");
			CHS = FrameIOFactory.GetChannel(ChannelTypeEnum.TCPSERVER, ops);
		}
 
		public void InitialChannelCHC(ChannelOption ops)
		{
			if (ops == null) ops = new ChannelOption();
			if (!ops.Contains("serverip")) ops.SetOption("serverip", "192.168.0.153");
			if (!ops.Contains("port")) ops.SetOption("port", 8007);
			CHC = FrameIOFactory.GetChannel(ChannelTypeEnum.TCPCLIENT, ops);
		}


		public Parameter<byte?> len { get; set;} = new Parameter<byte?>();
		public Parameter<byte?> end { get; set;} = new Parameter<byte?>();
		public Parameter<byte?> head { get; set;} = new Parameter<byte?>();

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
                CHS.WriteFrame(data.GetPacker());
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
				var data = new frameSRGettor(CHC.ReadFrame(frameSRGettor.Unpacker));
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