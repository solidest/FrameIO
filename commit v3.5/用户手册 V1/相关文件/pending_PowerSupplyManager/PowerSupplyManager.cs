using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.Runtime;
using FrameIO.Interface;
using System.Diagnostics;
using System.Linq;

namespace pending_PowerSupplyManager
{
    public partial class PowerSupplyManager
    {
        public IChannelBase CHClient;

		 
		public void InitialChannelCHClient(ChannelOption ops)
		{
			if (ops == null) ops = new ChannelOption();
			if (!ops.Contains("serverip")) ops.SetOption("serverip", "192.168.0.150");
			if (!ops.Contains("port")) ops.SetOption("port", 8007);
			CHClient = FrameIOFactory.GetChannel(ChannelTypeEnum.TCPCLIENT, ops);
		}


		public Parameter<bool?> Command1HasFeedback { get; set;} = new Parameter<bool?>();
		public Parameter<bool?> Command2HasFeedback { get; set;} = new Parameter<bool?>();
		public Parameter<bool?> Status1 { get; set;} = new Parameter<bool?>();
		public Parameter<int?> Status2 { get; set;} = new Parameter<int?>();
		public Parameter<double?> SetpointVoltage { get; set;} = new Parameter<double?>();
		public Parameter<double?> SetpointCurrent { get; set;} = new Parameter<double?>();
		public Parameter<double?> OutputVoltage { get; set;} = new Parameter<double?>();
		public Parameter<double?> OutputCurrent { get; set;} = new Parameter<double?>();
		public Parameter<byte?> LastRecvDatatype { get; set;} = new Parameter<byte?>();
		public Parameter<byte?> LastRecvPowerSupplyIdx { get; set;} = new Parameter<byte?>();

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

		
		public void Send_OutputPowerSupply()
        {
            try
            {
                var data = new Send_OutputPowerSupplySettor();
                data.g_header_datatype=3;
				data.g_header_PowerSupplyIdx = LastRecvPowerSupplyIdx.Value;
				data.g_OutputVoltage = OutputVoltage.Value;
				data.g_OutputCurrent = OutputCurrent.Value;
                CHClient.WriteFrame(data.GetPacker());
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }

		public void Send_OutCommandFeedBack()
        {
            try
            {
                var data = new Send_OutCommandFeedBackSettor();
                data.g_header_datatype=1;
				data.g_Command1HasFeedback = Command1HasFeedback.Value;
				data.g_Command2HasFeedback = Command2HasFeedback.Value;
                CHClient.WriteFrame(data.GetPacker());
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }

		public void Send_OutputStatusData()
        {
            try
            {
                var data = new Send_StatusSettor();
                data.g_header_datatype=2;
				data.g_Status1 = Status1.Value;
				data.g_Status2 = Status2.Value;
                CHClient.WriteFrame(data.GetPacker());
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }


		
        public void Recv_RecvData()
        {
            try
            {
				var data = new Receive_FrameDataGettor(CHClient.ReadFrame(Receive_FrameDataGettor.Unpacker));
                SetpointVoltage.Value = data.g_SetpointVoltage; 
				SetpointCurrent.Value = data.g_SetpointCurrent; 
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }


		

	}
}
