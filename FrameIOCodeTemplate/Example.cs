using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using FrameIO.Interface;
using System.IO;
using System.IO.Compression;
using System;
using FrameIO.Runtime;

namespace PROJECT1.SYS1
{

    public class FrameIORuntime
    {

        public static void Initial()
        {
            var config = string.Concat(
                        "H4sIAAAAAAAEAH1PMQ7CMAx0k0AppAg2KpBAMMDMUjZSCVS+wG8ysPMlNngO",
                "T+ASG9ouVHLOPtu98476NCJN3a90jO+jEFJfGQ93951MEQmij9CSJ5IrvElE",
                "+otGsNdycNur2Del7vAWkxPgOFYLqlTYTehkTGTOiqer6FBRBg8DvOHGIRVy",
                "a4Fc/GvSD1C1dtrr5r8X1GF0EPrQyEXX0pRmwKehnoPUOV4eMI34Au8N3xJ0",
                "T56Vctmz0J7//K/Ac22huQy+Pftq+sxb5OtWP5fa0oa2wDpzmc++ewVdUEMe",
                "c9z/AJr2JbroAQAA");


            using (var compressStream = new MemoryStream(Convert.FromBase64String(config)))
            {
                using (var zipStream = new GZipStream(compressStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        FrameIO.Runtime.FrameIOFactory.Initialize(resultStream.ToArray());
                    }
                }
            }
        }

    }

    public class TFrameGettor
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack GetUnpacker()
        {
            return FrameIOFactory.GetFrameUnpacker(2);
        }
        public TFrameGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

        public bool? SegmentA { get => _gettor.GetBool(1); }

        //引用类
        private TFrameGettor _SegmentB;
        public TFrameGettor SegmentB
        {
            get
            {
                if (_SegmentB == null) _SegmentB = new TFrameGettor(_gettor.GetSubFrame(1));
                return _SegmentB;
            }
        }

        //内置类
        private TInnerSegmentC _SegmnetC;
        public TInnerSegmentC SegmnetC
        {
            get
            {
                if (_SegmnetC == null) _SegmnetC = new TInnerSegmentC(this);
                return _SegmnetC;
            }
        }


        public class TInnerSegmentC
        {
            TFrameGettor _parent;
            public TInnerSegmentC(TFrameGettor parent)
            {
                _parent = parent;
            }
            public bool? InnerSegmengAA { get=>_parent._gettor.GetBool(1); }
        }

    }

    public class TFrameSettor
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public TFrameSettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(1);
        }

        public TFrameSettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        

        public bool? SegmentA { set => _settor.SetSegmentValue(2, value); }

        //引用类
        private TFrameSettor _SegmentB;
        public TFrameSettor SegmentB
        {
            get
            {
                if(_SegmentB==null) _SegmentB = new TFrameSettor(_settor.GetSubFrame(3));
                return _SegmentB;
            }
        }

        //内置类
        private TInnerSegmentC _SegmnetC;
        public TInnerSegmentC SegmnetC
        {
            get
            {
                if (_SegmnetC == null) _SegmnetC = new TInnerSegmentC(this);
                return _SegmnetC;
            }
        }

        public class TInnerSegmentC
        {
            TFrameSettor _parent;
            public TInnerSegmentC(TFrameSettor parent)
            {
                _parent = parent;
            }
            public bool? InnerSegmengAA{ set=> _parent._settor.SetSegmentValue(2, value); }
        }
    }


    //public partial class SYS1
    //{
    //    public IChannelBase CH1 { get; private set; }= FrameIOFactory.GetChannel("SYS1", "CH1");
    //    public IChannelBase CH2 { get; private set; }= FrameIOFactory.GetChannel("SYS1", "CH2");

    //    public Parameter<ushort?> PROPERTYA { get; set; } = new Parameter<ushort?>();
    //    public ObservableCollection<Parameter<byte?>> PROPERTYB { get; set; } = new ObservableCollection<Parameter<byte?>>();
    //    public Parameter<double?> PROPERTYC { get; set; }


    //    //异常处理接口
    //    private void HandleFrameIOError(FrameIOException ex)
    //    {
    //        switch(ex.ErrType)
    //        {
    //            case FrameIOErrorType.ChannelErr:
    //            case FrameIOErrorType.SendErr:
    //            case FrameIOErrorType.RecvErr:
    //            case FrameIOErrorType.CheckDtaErr:
    //                Debug.WriteLine("位置：{0}    错误：{1}", ex.Position, ex.ErrInfo);
    //                break;
    //        }
    //    }

               
    //    public void Recv_()
    //    {
    //        try
    //        {
    //            var funpack = FrameIOFactory.GetFrameUnpack("FRAME1");
    //            var data = CH1.ReadFrame(funpack);
    //            PROPERTYA.Value = data.GetUShort("SEG1");
    //            PROPERTYB.Clear();
    //            var __PROPERTYB = data.GetByteArray("SEG2");
    //            if (__PROPERTYB != null) foreach (var v in __PROPERTYB) PROPERTYB.Add(new Parameter<byte?>(v));


    //        }
    //        catch (FrameIOException ex)
    //        {
    //            HandleFrameIOError(ex);
    //        }
    //    }

    //    public void Send_()
    //    {
    //        try
    //        {
    //            var pack = FrameIOFactory.GetFramePack("FRAME1");
    //            pack.SetSegmentValue("SEG1", PROPERTYA.Value);
    //            pack.SetSegmentValue("SEG2", PROPERTYB.Select(p => p.Value).ToArray());
    //            CH1.WriteFrame(pack);
    //        }
    //        catch (FrameIOException ex)
    //        {
    //            HandleFrameIOError(ex);
    //        }
    //    }

    //    public void recvloop()
    //    {
    //        var unpack = FrameIOFactory.GetFrameUnpack("FRAME1");
    //        CH1.BeginReadFrame(unpack, recvloopCallback, null);
    //        _IsStopRecvLoop = false;
    //    }

    //    public void stoprecvloop()
    //    {
    //        _IsStopRecvLoop = true;
    //    }

    //    private bool _IsStopRecvLoop;
    //    public delegate void recvloopHandle();
    //    public event recvloopHandle Onrecvloop;
    //    private void recvloopCallback(ISegmentGettor data, out bool isstop, object AsyncState)
    //    {
    //        try
    //        {
    //            PROPERTYA.Value = data.GetUShort("SEG1");
    //            PROPERTYB.Clear();
    //            var __PROPERTYB = data.GetByteArray("SEG2");
    //            if(__PROPERTYB != null) foreach (var v in __PROPERTYB) PROPERTYB.Add(new Parameter<byte?>(v));
    //            if(Onrecvloop != null) foreach (recvloopHandle deleg in Onrecvloop.GetInvocationList()) deleg.BeginInvoke(null, null);
    //        }
    //        catch (FrameIOException ex)
    //        {
    //            HandleFrameIOError(ex);
    //        }
    //        finally
    //        {
    //            isstop = _IsStopRecvLoop;
    //        }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //}
}
