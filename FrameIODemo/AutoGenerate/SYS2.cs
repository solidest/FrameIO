using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.Runtime;
using FrameIO.Interface;
using System.Diagnostics;
using System.Linq;

namespace demo
{
    public partial class SYS2
    {
        public IChannelBase CHA { get; private set; } = FrameIOFactory.GetChannel("SYS2", "CHA");

		public Parameter<byte?> PROPERTY2a { get; set;} = new Parameter<byte?>();
		public Parameter<sbyte?> PROPERTY2b { get; set;} = new Parameter<sbyte?>();
		public Parameter<uint?> PROPERTY2c { get; set;} = new Parameter<uint?>();
		public Parameter<double?> PROPERTY2d { get; set;} = new Parameter<double?>();
		public ObservableCollection<Parameter<bool?>> PROPERTY2e { get; set; } = new ObservableCollection<Parameter<bool?>>();

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

        public void RecvData()
        {
            try
            {
                var unpack = FrameIOFactory.GetFrameUnpack("MSG1");
                var data = CHA.ReadFrame(unpack);
                PROPERTY2a.Value = data.GetByte(1);
				PROPERTY2b.Value = data.GetSByte(2);
				PROPERTY2c.Value = data.GetUInt(3);
				PROPERTY2d.Value = data.GetDouble(4);
				PROPERTY2e.Clear();
				var __PROPERTY2e = data.GetBoolArray(5);
				if (__PROPERTY2e != null) foreach (var v in __PROPERTY2e) PROPERTY2e.Add(new Parameter<bool?>(v));
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }


		

	}
}
