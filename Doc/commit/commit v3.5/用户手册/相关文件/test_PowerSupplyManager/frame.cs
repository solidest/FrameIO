
using System.IO;
using System.IO.Compression;
using System;
using FrameIO.Runtime;
using FrameIO.Interface;


namespace test_PowerSupplyManager
{
    public class FrameBase
    {
		protected static void InitialBase(){}
        static FrameBase()
        {
            var config = string.Concat(
				"H4sIAAAAAAAEAG2PQQ7CQAhF6diaVqk2urCLmmiiN3BvNfFintgj+KZAolEW",
				"8wY+H2bOMpckhXzH6+qX0VA7O+dljM7srOQ3FGUBbwfLgw3bKqllid7IFibY",
				"wxk8whJ2kzdH63MUZf1n3mo69/SZrng38JEK91uu7NplX+n+YB97TFfeMnzq",
				"g/MZfaYrO/MT7snqwdbryl9Of3WrvwFhK2ZreAEAAA==");

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

		
	public class RecvDataFrameGettor : FrameBase
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack Unpacker
        {
            get { InitialBase(); return FrameIOFactory.GetFrameUnpacker(1); }
        }

        public RecvDataFrameGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

		public byte? g_header_datatype { get => _gettor.GetByte(2); }
		public byte? g_header_PowerSupplyIdx { get => _gettor.GetByte(3); }
		public g_dataGettor g_data { get { if (_g_data == null) _g_data = new g_dataGettor(_gettor); return _g_data; } }private g_dataGettor _g_data;
		public class g_dataGettor
		{
		    ISegmentGettor _gettor;
		    public g_dataGettor(ISegmentGettor gettor)
		    {
				_gettor = gettor;
		    }
		    public FeedbackDataGettor EmFeedbackData { get { if (_EmFeedbackData == null) _EmFeedbackData = new FeedbackDataGettor(_gettor.GetSubFrame(5)); return _EmFeedbackData; } } private FeedbackDataGettor _EmFeedbackData;
			public StatusDataGettor EmStatusData { get { if (_EmStatusData == null) _EmStatusData = new StatusDataGettor(_gettor.GetSubFrame(6)); return _EmStatusData; } } private StatusDataGettor _EmStatusData;
			public PowerSupply_GetGettor EmPowerSupply_Get { get { if (_EmPowerSupply_Get == null) _EmPowerSupply_Get = new PowerSupply_GetGettor(_gettor.GetSubFrame(7)); return _EmPowerSupply_Get; } } private PowerSupply_GetGettor _EmPowerSupply_Get;
			public UserCustomFrameGettor EmUserCustom { get { if (_EmUserCustom == null) _EmUserCustom = new UserCustomFrameGettor(_gettor.GetSubFrame(8)); return _EmUserCustom; } } private UserCustomFrameGettor _EmUserCustom;
		}
    }
	public class RecvDataFrameSettor : FrameBase
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public RecvDataFrameSettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(1);
        }

        public RecvDataFrameSettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        public byte? g_header_datatype { set => _settor.SetSegmentValue(2, value); }
		public byte? g_header_PowerSupplyIdx { set => _settor.SetSegmentValue(3, value); }
		public g_dataSettor g_data { get { if (_g_data == null) _g_data = new g_dataSettor(_settor); return _g_data; } }private g_dataSettor _g_data;
		public class g_dataSettor
		{
		    ISegmentSettor _settor;
		    public g_dataSettor(ISegmentSettor settor)
		    {
				_settor = settor;
		    }
		    public FeedbackDataSettor EmFeedbackData { get { if (_EmFeedbackData == null) _EmFeedbackData = new FeedbackDataSettor(_settor.GetSubFrame(5)); return _EmFeedbackData; } } private FeedbackDataSettor _EmFeedbackData;
			public StatusDataSettor EmStatusData { get { if (_EmStatusData == null) _EmStatusData = new StatusDataSettor(_settor.GetSubFrame(6)); return _EmStatusData; } } private StatusDataSettor _EmStatusData;
			public PowerSupply_GetSettor EmPowerSupply_Get { get { if (_EmPowerSupply_Get == null) _EmPowerSupply_Get = new PowerSupply_GetSettor(_settor.GetSubFrame(7)); return _EmPowerSupply_Get; } } private PowerSupply_GetSettor _EmPowerSupply_Get;
			public UserCustomFrameSettor EmUserCustom { get { if (_EmUserCustom == null) _EmUserCustom = new UserCustomFrameSettor(_settor.GetSubFrame(8)); return _EmUserCustom; } } private UserCustomFrameSettor _EmUserCustom;
		}     
    }	
	public class SendDataFrameGettor : FrameBase
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack Unpacker
        {
            get { InitialBase(); return FrameIOFactory.GetFrameUnpacker(11); }
        }

        public SendDataFrameGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

		public byte? g_header_datatype { get => _gettor.GetByte(12); }
		public byte? g_header_PowerSupplyIdx { get => _gettor.GetByte(13); }
		public PowerSupply_SetGettor g_data { get { if (_g_data == null) _g_data = new PowerSupply_SetGettor(_gettor.GetSubFrame(14)); return _g_data; }} private PowerSupply_SetGettor _g_data;
    }
	public class SendDataFrameSettor : FrameBase
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public SendDataFrameSettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(11);
        }

        public SendDataFrameSettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        public byte? g_header_datatype { set => _settor.SetSegmentValue(12, value); }
		public byte? g_header_PowerSupplyIdx { set => _settor.SetSegmentValue(13, value); }
		public PowerSupply_SetSettor g_data { get { if (_g_data == null) _g_data = new PowerSupply_SetSettor(_settor.GetSubFrame(14)); return _g_data; }} private PowerSupply_SetSettor _g_data;     
    }	
	public class UserCustomFrameGettor : FrameBase
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack Unpacker
        {
            get { InitialBase(); return FrameIOFactory.GetFrameUnpacker(16); }
        }

        public UserCustomFrameGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

		public double?[] g_data { get { if (_g_data == null) _g_data = _gettor.GetDoubleArray(17); return _g_data; } } private double?[] _g_data;
    }
	public class UserCustomFrameSettor : FrameBase
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public UserCustomFrameSettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(16);
        }

        public UserCustomFrameSettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        public double?[] g_data { set => _settor.SetSegmentValue(17, value); }     
    }	
	public class FeedbackDataGettor : FrameBase
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack Unpacker
        {
            get { InitialBase(); return FrameIOFactory.GetFrameUnpacker(19); }
        }

        public FeedbackDataGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

		public bool? g_Command1HasFeedback { get => _gettor.GetBool(20); }
		public bool? g_Command2HasFeedback { get => _gettor.GetBool(21); }
		public byte? g_h1 { get => _gettor.GetByte(22); }
    }
	public class FeedbackDataSettor : FrameBase
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public FeedbackDataSettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(19);
        }

        public FeedbackDataSettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        public bool? g_Command1HasFeedback { set => _settor.SetSegmentValue(20, value); }
		public bool? g_Command2HasFeedback { set => _settor.SetSegmentValue(21, value); }
		public byte? g_h1 { set => _settor.SetSegmentValue(22, value); }     
    }	
	public class StatusDataGettor : FrameBase
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack Unpacker
        {
            get { InitialBase(); return FrameIOFactory.GetFrameUnpacker(24); }
        }

        public StatusDataGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

		public bool? g_Status1 { get => _gettor.GetBool(25); }
		public byte? g_h1 { get => _gettor.GetByte(26); }
		public int? g_Status2 { get => _gettor.GetInt(27); }
    }
	public class StatusDataSettor : FrameBase
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public StatusDataSettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(24);
        }

        public StatusDataSettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        public bool? g_Status1 { set => _settor.SetSegmentValue(25, value); }
		public byte? g_h1 { set => _settor.SetSegmentValue(26, value); }
		public int? g_Status2 { set => _settor.SetSegmentValue(27, value); }     
    }	
	public class PowerSupply_SetGettor : FrameBase
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack Unpacker
        {
            get { InitialBase(); return FrameIOFactory.GetFrameUnpacker(29); }
        }

        public PowerSupply_SetGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

		public double? g_SetpointVoltage { get => _gettor.GetDouble(30); }
		public double? g_SetpointCurrent { get => _gettor.GetDouble(31); }
    }
	public class PowerSupply_SetSettor : FrameBase
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public PowerSupply_SetSettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(29);
        }

        public PowerSupply_SetSettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        public double? g_SetpointVoltage { set => _settor.SetSegmentValue(30, value); }
		public double? g_SetpointCurrent { set => _settor.SetSegmentValue(31, value); }     
    }	
	public class PowerSupply_GetGettor : FrameBase
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack Unpacker
        {
            get { InitialBase(); return FrameIOFactory.GetFrameUnpacker(33); }
        }

        public PowerSupply_GetGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

		public double? g_OutputVoltage { get => _gettor.GetDouble(34); }
		public double? g_OutputCurrent { get => _gettor.GetDouble(35); }
    }
	public class PowerSupply_GetSettor : FrameBase
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public PowerSupply_GetSettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(33);
        }

        public PowerSupply_GetSettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        public double? g_OutputVoltage { set => _settor.SetSegmentValue(34, value); }
		public double? g_OutputCurrent { set => _settor.SetSegmentValue(35, value); }     
    }

}
