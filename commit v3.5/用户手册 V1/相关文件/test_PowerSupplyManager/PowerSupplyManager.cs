using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.Runtime;
using FrameIO.Interface;
using System.Diagnostics;
using System.Linq;

namespace test_PowerSupplyManager
{
    public partial class PowerSupplyManager
    {
        public IChannelBase CHServer;

		 
		public void InitialChannelCHServer(ChannelOption ops)
		{
			if (ops == null) ops = new ChannelOption();
			if (!ops.Contains("serverip")) ops.SetOption("serverip", "192.168.0.150");
			if (!ops.Contains("port")) ops.SetOption("port", 8007);
			if (!ops.Contains("clientip")) ops.SetOption("clientip", "192.168.0.150");
			CHServer = FrameIOFactory.GetChannel(ChannelTypeEnum.TCPSERVER, ops);
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

		
		public void Send_SetPowerSupply()
        {
            try
            {
                var data = new SendDataFrameSettor();
                data.g_header_datatype=3;
				data.g_header_PowerSupplyIdx=2;
				data.g_data.g_SetpointVoltage = SetpointVoltage.Value;
				data.g_data.g_SetpointCurrent = SetpointCurrent.Value;
                CHServer.WriteFrame(data.GetPacker());
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
				var data = new RecvDataFrameGettor(CHServer.ReadFrame(RecvDataFrameGettor.Unpacker));
                LastRecvDatatype.Value = data.g_header_datatype; 
				LastRecvPowerSupplyIdx.Value = data.g_header_PowerSupplyIdx; 
				switch(LastRecvDatatype.Value) {
				case 1:
				Command1HasFeedback.Value = data.g_data.EmFeedbackData.g_Command1HasFeedback; 
				Command2HasFeedback.Value = data.g_data.EmFeedbackData.g_Command2HasFeedback; 
				break;
				case 2:
				Status1.Value = data.g_data.EmStatusData.g_Status1; 
				Status2.Value = data.g_data.EmStatusData.g_Status2; 
				break;
				case 3:
				OutputVoltage.Value = data.g_data.EmPowerSupply_Get.g_OutputVoltage; 
				OutputCurrent.Value = data.g_data.EmPowerSupply_Get.g_OutputCurrent; 
				break;
				case 4:
				UserCustomFunction(data.g_data.EmUserCustom.g_data);
				break;}
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }


		

	}
}
