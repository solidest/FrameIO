using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System.Linq;

namespace main
{
    public partial class SYS1
    {
        public IChannelBase CH1 { get; private set; } = FrameIOFactory.GetChannel("SYS1", "CH1");

		

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
