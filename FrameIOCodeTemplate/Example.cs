using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.CodeTemplate;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;

namespace PROJECT1.SYS1
{
    public partial class SYS1
    {
        private IChannelBase CH1;
        private IChannelBase CH2;

        public Parameter<ushort?> PROPERTYA { get; set; }
        public ObservableCollection<Parameter<byte?>> PROPERTYB { get; set; }
        public Parameter<double?> PROPERTYC { get; set; }

        public void Initialization()
        {
            try
            {
                CH1 = FrameIOFactory.GetChannel("SYS1", "CH1");
                CH1.Open();
                CH2 = FrameIOFactory.GetChannel("SYS1", "CH2");
                CH2.Open();
            }
            catch(FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }

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

               
        public void Recv_()
        {
            try
            {
                var funpack = FrameIOFactory.GetFrameUnpack("FRAME1");
                var data = CH1.ReadFrame(funpack);
                PROPERTYA.Value = data.GetUShort("SEG1");
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }

        public void Send_()
        {
            try
            {
                var pack = FrameIOFactory.GetFramePack("FRAME1");
                pack.SetSegmentValue("SEG1", PROPERTYA.Value);
                CH1.WriteFrame(pack);
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
