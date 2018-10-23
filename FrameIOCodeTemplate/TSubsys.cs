using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;
using FrameIO.Runtime;

namespace PROJECT1
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

    public partial class SYS1
    {
        public IChannelBase CH1 { get; private set; }
        public IChannelBase CH2 { get; private set; }

        public Parameter<uint?> PROPERTYA { get; set; } = new Parameter<uint?>();
        public ObservableCollection<Parameter<int?>> PROPERTYB { get; set; } = new ObservableCollection<Parameter<int?>>();
        public Parameter<double?> PROPERTYC { get; set; }

        //用户自定义类属性
        private PowerSupply[] _powerSupplies = new PowerSupply[10];
        public ReadOnlyCollection<PowerSupply> PowerSupplies => new ReadOnlyCollection<PowerSupply>(_powerSupplies);

        //异常处理接口
        private void HandleFrameIOError(FrameIOException ex)
        {
            switch (ex.ErrType)
            {
                case FrameIOErrorType.ChannelErr:
                case FrameIOErrorType.SendErr:
                case FrameIOErrorType.RecvErr:
                case FrameIOErrorType.CheckDtaErr:
                    Debug.WriteLine("位置：{0}    错误：{1}", ex.Position, ex.ErrInfo);
                    break;
            }
        }

        //初始化通道
        public void InitialChannelCH1(ChannelOption ops)
        {
            if (ops == null) ops = new ChannelOption();
            if(!ops.Contains("vector")) ops.SetOption("vector", "yh");
            if (!ops.Contains("bt1")) ops.SetOption("bt1", 100);
            CH1 = FrameIOFactory.GetChannel(ChannelTypeEnum.CAN, ops);
        }

        public void Recv_()
        {
            try
            {
                var gettor = new TFrameGettor(CH1.ReadFrame(TFrameGettor.Unpacker));
                PROPERTYA.Value = gettor.SegmentA;
                PROPERTYB.Clear();
                for (int i = 0; i < gettor.SegmentD.Length; i++) PROPERTYB.Add(new Parameter<int?>(gettor.SegmentD[i]));
                //PowerSupplies[0].SetpointCurrent.Value = gettor.SegmentD.SetpointCurrent;

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
                var settor = new TFrameSettor();
                settor.SegmentA = PROPERTYA.Value;
                switch (PROPERTYA.Value)
                {
                    case 123:
                        settor.SegmnetC.InnerSegmengAA = PROPERTYA.Value;
                        break;
                    default:
                        settor.SegmnetC.InnerSegmengAA = PROPERTYA.Value;
                        break;
                }
                CH1.WriteFrame(settor.GetPacker());
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
                if (__PROPERTYB != null) foreach (var v in __PROPERTYB) PROPERTYB.Add(new Parameter<byte?>(v));
                if (Onrecvloop != null) foreach (recvloopHandle deleg in Onrecvloop.GetInvocationList()) deleg.BeginInvoke(null, null);
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
