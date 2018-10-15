using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.CodeTemplate;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;
using System.Linq;

namespace PROJECT1.SYS1
{

    public partial class FrameRun
    {

        public static FrameRun  CreateTFrameRun()
        {
            return new FrameRun(string.Concat(
                        "Cgp0ZXN0LnByb3RvIjUKBFRlc3QSCwoDY21kGAEgASgJEg8KB2ludW1iZXIY",
                        "AiABKAUSDwoHZm51bWJlchgDIAMoAmIGcHJvdG8z"));
        }

        private FrameRun(string content)
        {
            Content = System.Convert.FromBase64String(content);
        }
        
        public byte[] Content { get; private set; }
    }


    public partial class SYS1
    {
        public IChannelBase CH1 { get; private set; }= FrameIOFactory.GetChannel("SYS1", "CH1");
        public IChannelBase CH2 { get; private set; }= FrameIOFactory.GetChannel("SYS1", "CH2");

        public Parameter<ushort?> PROPERTYA { get; set; } = new Parameter<ushort?>();
        public ObservableCollection<Parameter<byte?>> PROPERTYB { get; set; } = new ObservableCollection<Parameter<byte?>>();
        public Parameter<double?> PROPERTYC { get; set; }


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

        public void recvloop()
        {
            var unpack = FrameIOFactory.GetFrameUnpack("FRAME1");
            CH1.BeginReadFrame(unpack, recvloopCallback, null);
            _IsStopRecvLoop = false;
        }

        public void stoprecvloop()
        {
            _IsStopRecvLoop = true;
        }

        private bool _IsStopRecvLoop;
        public delegate void recvloopHandle();
        public event recvloopHandle Onrecvloop;
        private void recvloopCallback(ISegmentGettor data, out bool isstop, object AsyncState)
        {
            try
            {
                PROPERTYA.Value = data.GetUShort("SEG1");
                PROPERTYB.Clear();
                var __PROPERTYB = data.GetByteArray("SEG2");
                if(__PROPERTYB != null) foreach (var v in __PROPERTYB) PROPERTYB.Add(new Parameter<byte?>(v));
                if(Onrecvloop != null) foreach (recvloopHandle deleg in Onrecvloop.GetInvocationList()) deleg.BeginInvoke(null, null);
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
            finally
            {
                isstop = _IsStopRecvLoop;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
