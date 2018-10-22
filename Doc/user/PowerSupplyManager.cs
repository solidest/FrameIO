using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System.Linq;

namespace demo
{
	public class FeedbackData
	{
		public Parameter<bool?> Command1HasFeedback { get; } = new Parameter<bool?>();
		public Parameter<bool?> Command2HasFeedback { get; } = new Parameter<bool?>();
	}
	
	public class StatusData
	{
		public Parameter<bool?> Status1 { get; } = new Parameter<bool?>();
		public Parameter<int?> Status2 { get; } = new Parameter<int?>();
	}
	
	public class PowerSupply
	{
		// 向电源设置的设定电压和设定电流.
		public Parameter<double?> SetpointVoltage { get; } = new Parameter<double?>();
		public Parameter<double?> SetpointCurrent { get; } = new Parameter<double?>();
		
		// 从电源获得的输出电压和输出电流.
		public Parameter<double?> OutputVoltage { get; } = new Parameter<double?>();
		public Parameter<double?> OutputCurrent { get; } = new Parameter<double?>();
	}
	
    public partial class PowerSupplyManager
    {
        public IChannelBase CH1 { get; private set; } = FrameIOFactory.GetChannel("SYS1", "CH1");

		public FeedbackData FeedbackData { get; } = new FeedbackData();
		pubic StatusData StatusData { get; } = new StatusData();
		
		// 不一定非要用下面的写法, 意思就是个数组.
		// 数量 10 也是在配置的时候配置好的.
		private PowerSupply[] _powerSupplies = new PowerSupply[10];
		public ReadOnlyCollection<PowerSupply> PowerSupplies => new ReadOnlyCollection<PowerSupply>(_powerSupplies);
		
		public BatteryManager()
		{
			for (int i = 0; i < _powerSupplies.Length; i++)
			{
				_powerSupplies[i] = new PowerSupply();
			}
		}
		
		public void SetPowerSupplySetpointData(int powerSupplyIndex)
		{
			// _powerSupplies[powerSupplyIndex] 的 SetpointVoltage 和 SetpointCurrent 已在外部设置.
			
			var powerSupply = _powerSupplies[powerSupplyIndex];			
			
			try
			{
				var pack = FrameIOFactory.GetFramePack("XXX");
				pack.SetSegmentValue("PowerSupplyIndex", powerSupplyIndex);
				pack.SetSegmentValue("SepointVoltage", powerSupply.SetpointVoltage.Value);
				pack.SetSegmentValue("SetpointCurrent", powerSupply.SetpointCurrent.Value);
                CH1.WriteFrame(pack);
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
		}
		
		public void ReceiveData()
		{			
			// FeedbackData 和 StatusData 的数据可能从不同的数据包返回，这里假设每个数据包只包含一个数据.
			// 不知道最后是不是定义成一个 frame.
			// 如果是定义不同的 frame，那代码估计应该是: 
			
			var unpack = FrameIOFactory.GetFrameUnpack("Feedback1");
			var data = CH1.ReadFrame(unpack);
			FeedbackData.Command1HasFeedback.Value = data.GetBool("HasFeedback");
			
			// 以及:
			
			var unpack = FrameIOFactory.GetFrameUnpack("Feedback2");
			var data = CH1.ReadFrame(unpack);
			FeedbackData.Command2HasFeedback.Value = data.GetBool("HasFeedback");			
			
			var unpack = FrameIOFactory.GetFrameUnpack("Status1");
			var data = CH1.ReadFrame(unpack);
			StatusData.Status1.Value = data.GetBool("Status1");
			
			var unpack = FrameIOFactory.GetFrameUnpack("Status2");
			var data = CH1.ReadFrame(unpack);
			StatusData.Status2.Value = data.GetInt("Status2");
			
			// 接收对方 PowerSupply 的数据，有可能 10 个 PowerSupply 全部回传，也有可能只传一个.
			// 只传一个电源的情况:
			
			var unpack = FrameIOFactory.GetFrameUnpack("PowerSupplyStatus");
			var data = CH1.ReadFrame(unpack);
			int powerSupplyIndex = data.GetInt("PowerSupplyIndex");
			var powerSupply = _powerSupplies[powerSupplyIndex];
			powerSupply.OutputVoltage.Value = data.GetDouble("Voltage");
			powerSupply.OutputCurrent.Value = data.GetDouble("Current");
		}
		
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
	}
}
