using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.Runtime;
using FrameIO.Interface;
using System.Diagnostics;
using System.Linq;

namespace demo
{
    public partial class SYS1
    {
        public IChannelBase CH1 { get; private set; } = FrameIOFactory.GetChannel("SYS1", "CH1");

		public Parameter<byte?> PROPERTYa { get; set;} = new Parameter<byte?>();
		public Parameter<sbyte?> PROPERTYb { get; set;} = new Parameter<sbyte?>();
		public Parameter<uint?> PROPERTYc { get; set;} = new Parameter<uint?>();
		public Parameter<double?> PROPERTYd { get; set;} = new Parameter<double?>();
		public ObservableCollection<Parameter<bool?>> PROPERTYe { get; set; } = new ObservableCollection<Parameter<bool?>>();

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

		
		public void SendData()
        {
            try
            {
                var pack = FrameIOFactory.GetFramePack("MSG1");
                pack.SetSegmentValue(1, PROPERTYa.Value);
				pack.SetSegmentValue(2, PROPERTYb.Value);
				pack.SetSegmentValue(3, PROPERTYc.Value);
				pack.SetSegmentValue(4, PROPERTYd.Value);
				pack.SetSegmentValue(5, PROPERTYe.Select(p => p.Value).ToArray());
                CH1.WriteFrame(pack.GetPack());
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }


		

		

	}
}
