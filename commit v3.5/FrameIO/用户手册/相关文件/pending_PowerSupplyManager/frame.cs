
using System.IO;
using System.IO.Compression;
using System;
using FrameIO.Runtime;
using FrameIO.Interface;


namespace pending_PowerSupplyManager
{
    public class FrameBase
    {
		protected static void InitialBase(){}
        static FrameBase()
        {
            var config = string.Concat(
				"H4sIAAAAAAAEAJNmYARDQoAHqIYdSDsqQPhwmgWNloDQvFD1PAwcQDamPicm",
				"VJoXqo6HgY9BBJ89MlC6CaYPop6HQZRBiih7IOoAI4ak5/gAAAA=");

			using (var compressStream = new MemoryStream(Convert.FromBase64String(config)))
            {
                using (var zipStream = new GZipStream(compressStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        FrameIO.Runtime.FrameIOFactory.Initial(resultStream.ToArray());
                    }
                }
            }
        }
    }

		
	public class Send_OutCommandFeedBackGettor : FrameBase
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack Unpacker
        {
            get { InitialBase(); return FrameIOFactory.GetFrameUnpacker(1); }
        }

        public Send_OutCommandFeedBackGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

		public byte? g_header_datatype { get => _gettor.GetByte(2); }
		public byte? g_header_PowerSupplyIdx { get => _gettor.GetByte(3); }
		public bool? g_Command1HasFeedback { get => _gettor.GetBool(4); }
		public bool? g_Command2HasFeedback { get => _gettor.GetBool(5); }
		public byte? g_h1 { get => _gettor.GetByte(6); }
    }
	public class Send_OutCommandFeedBackSettor : FrameBase
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public Send_OutCommandFeedBackSettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(1);
        }

        public Send_OutCommandFeedBackSettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        public byte? g_header_datatype { set => _settor.SetSegmentValue(2, value); }
		public byte? g_header_PowerSupplyIdx { set => _settor.SetSegmentValue(3, value); }
		public bool? g_Command1HasFeedback { set => _settor.SetSegmentValue(4, value); }
		public bool? g_Command2HasFeedback { set => _settor.SetSegmentValue(5, value); }
		public byte? g_h1 { set => _settor.SetSegmentValue(6, value); }     
    }	
	public class Send_OutputPowerSupplyGettor : FrameBase
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack Unpacker
        {
            get { InitialBase(); return FrameIOFactory.GetFrameUnpacker(8); }
        }

        public Send_OutputPowerSupplyGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

		public byte? g_header_datatype { get => _gettor.GetByte(9); }
		public byte? g_header_PowerSupplyIdx { get => _gettor.GetByte(10); }
		public double? g_OutputVoltage { get => _gettor.GetDouble(11); }
		public double? g_OutputCurrent { get => _gettor.GetDouble(12); }
    }
	public class Send_OutputPowerSupplySettor : FrameBase
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public Send_OutputPowerSupplySettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(8);
        }

        public Send_OutputPowerSupplySettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        public byte? g_header_datatype { set => _settor.SetSegmentValue(9, value); }
		public byte? g_header_PowerSupplyIdx { set => _settor.SetSegmentValue(10, value); }
		public double? g_OutputVoltage { set => _settor.SetSegmentValue(11, value); }
		public double? g_OutputCurrent { set => _settor.SetSegmentValue(12, value); }     
    }	
	public class Send_StatusGettor : FrameBase
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack Unpacker
        {
            get { InitialBase(); return FrameIOFactory.GetFrameUnpacker(14); }
        }

        public Send_StatusGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

		public byte? g_header_datatype { get => _gettor.GetByte(15); }
		public byte? g_header_PowerSupplyIdx { get => _gettor.GetByte(16); }
		public bool? g_Status1 { get => _gettor.GetBool(17); }
		public byte? g_h1 { get => _gettor.GetByte(18); }
		public int? g_Status2 { get => _gettor.GetInt(19); }
    }
	public class Send_StatusSettor : FrameBase
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public Send_StatusSettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(14);
        }

        public Send_StatusSettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        public byte? g_header_datatype { set => _settor.SetSegmentValue(15, value); }
		public byte? g_header_PowerSupplyIdx { set => _settor.SetSegmentValue(16, value); }
		public bool? g_Status1 { set => _settor.SetSegmentValue(17, value); }
		public byte? g_h1 { set => _settor.SetSegmentValue(18, value); }
		public int? g_Status2 { set => _settor.SetSegmentValue(19, value); }     
    }	
	public class Receive_FrameDataGettor : FrameBase
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack Unpacker
        {
            get { InitialBase(); return FrameIOFactory.GetFrameUnpacker(21); }
        }

        public Receive_FrameDataGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

		public byte? g_header_datatype { get => _gettor.GetByte(22); }
		public byte? g_header_idx { get => _gettor.GetByte(23); }
		public double? g_SetpointVoltage { get => _gettor.GetDouble(24); }
		public double? g_SetpointCurrent { get => _gettor.GetDouble(25); }
    }
	public class Receive_FrameDataSettor : FrameBase
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public Receive_FrameDataSettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(21);
        }

        public Receive_FrameDataSettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        public byte? g_header_datatype { set => _settor.SetSegmentValue(22, value); }
		public byte? g_header_idx { set => _settor.SetSegmentValue(23, value); }
		public double? g_SetpointVoltage { set => _settor.SetSegmentValue(24, value); }
		public double? g_SetpointCurrent { set => _settor.SetSegmentValue(25, value); }     
    }

}
