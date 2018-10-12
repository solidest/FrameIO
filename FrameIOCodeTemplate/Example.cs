using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.CodeTemplate;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System.Linq;

namespace PROJECT1.SYS1
{
    public partial class SYS1
    {
        private IChannelBase CH1;
        private IChannelBase CH2;

        public Parameter<ushort?> PROPERTYA { get; set; } = new Parameter<ushort?>();
        public ObservableCollection<Parameter<byte?>> PROPERTYB { get; set; } = new ObservableCollection<Parameter<byte?>>();
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
                PROPERTYB.Clear();
                var __PROPERTYB = data.GetByteArray("SEG2");
                if (__PROPERTYB != null) foreach (var v in __PROPERTYB) PROPERTYB.Add(new Parameter<byte?>(v));


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
                pack.SetSegmentValue("SEG2", PROPERTYB.Select(p => p.Value).ToArray());
                CH1.WriteFrame(pack);
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }

        public bool IsStopRecvLoop { get; set; }
        public void recvloop()
        {
            var unpack = FrameIOFactory.GetFrameUnpack("FRAME1");
            CH1.BeginReadFrame(unpack, recvloopCallback, null);
        }

        public delegate void recvloopHandle();
        public event recvloopHandle Onrecvloop;
        private void recvloopCallback(IFrameData data, out bool isstop, object AsyncState)
        {
            try
            {
                PROPERTYA.Value = data.GetUShort("SEG1");
                PROPERTYB.Clear();
                var __PROPERTYB = data.GetByteArray("SEG2");
                if(__PROPERTYB != null) foreach (var v in __PROPERTYB) PROPERTYB.Add(new Parameter<byte?>(v));
                if(Onrecvloop != null) foreach (recvloopHandle deleg in Onrecvloop.GetInvocationList()) deleg.BeginInvoke(null, null);
                isstop = IsStopRecvLoop;
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
