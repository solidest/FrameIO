using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    public class FrameRuntime
    {
        private static Dictionary<string, FrameInfo> __frames = new Dictionary<string, FrameInfo>();

        //注册一类数据帧
        public static void RegisterFrame(string framename, byte[] content)
        {
            if (__frames.ContainsKey(framename)) return;
            var frm = new FrameInfo(content);
            frm.Initial();
            __frames.Add(framename, frm);
        }

        //获取数据帧
        public static FrameInfo GetFrame(string framename)
        {
            return __frames[framename];
        }

    }

    public class FrameInfo
    {
        const byte LEN_USHORT = 16;
        const byte LEN_BYTE = 8;
        const byte LEN_CODE = 6;

        private SegmentBaseRun[] _segs;
        private ConstValue[] _consts;
        private ExpInfo[] _explist;
        private Validator[] _validats;

        public FrameInfo(byte[] content)
        {
            ushort segmentcount = content[0];
            ushort constcount = content[1];
            ushort expressioncount = content[2];
            ushort validatorcount = content[3];
            int pos = 4;
            if (content.Length != segmentcount*8 + constcount*9 + expressioncount*8 + validatorcount*8 + 8)
                throw new Exception("runtime");

            _segs = new SegmentBaseRun[segmentcount];
            for (int i = 0; i < segmentcount; i++)
            {
                _segs[i] = SegmentFactory[content[pos]&(~0<<LEN_CODE)].Invoke(BitConverter.ToUInt64(content, pos));
                pos += 8;
            }

            _consts = new ConstValue[constcount];
            for (int i = 0; i < constcount; i++)
            {
                _consts[i] =new ConstValue(content, pos);
                pos += 9;
            }

            _explist = new ExpInfo[expressioncount];
            for(int i=0; i< expressioncount; i++)
            {
                _explist[i] = new ExpInfo(BitConverter.ToUInt64(content, pos));
                pos += 8;
            }

            _validats = new Validator[validatorcount];
            for(int i=0; i< validatorcount; i++)
            {
                _validats[i] = new Validator(BitConverter.ToUInt64(content, pos));
                pos += 8;
            }
        }

        public void Initial()
        {

        }

        public SegmentBaseRun this[int index]
        {
            get
            {
                return _segs[index];
            }
        }

        public int SegmentsCount { get => _segs.Length; }


        #region --SegmentFactory--

        private delegate SegmentBaseRun CreateSegment(ulong data);

        private static CreateSegment[] SegmentFactory = new CreateSegment[]
        {
            CreateSegmentNullInfo, 
            CreateSegmentIntegerInfo,//const byte CO_SEGINTEGER = 1;
            CreateSegmentRealInfo, //const byte CO_SEGREAL = 2;
            CreateSegmentTextInfo, //const byte CO_SEGTEXT = 3;
            CreateSegmentNullInfo,//const byte CO_SEGBLOCK_IN = 4;
            CreateSegmentNullInfo,//const byte CO_SEGBLOCK_OUT = 5;
            CreateSegmentNullInfo,//const byte CO_SEGONEOF_IN = 6;
            CreateSegmentNullInfo,//const byte CO_SEGONEOF_OUT = 7;
            CreateSegmentNullInfo,//const byte CO_SEGONEOFITEM_IN = 8;
            CreateSegmentNullInfo //const byte CO_SEGONEOFITEM_OUT = 9;           
        };

        private static SegmentBaseRun CreateSegmentNullInfo(ulong data)
        {
            return null;
        }

        private static SegmentBaseRun CreateSegmentIntegerInfo(ulong data)
        {
            return new SegmentIntegerInfo(data);
        }

        private static SegmentBaseRun CreateSegmentRealInfo(ulong data)
        {
            return new SegmentRealInfo(data);
        }

        private static SegmentBaseRun CreateSegmentTextInfo(ulong data)
        {
            return new SegmentTextInfo(data);
        }

        #endregion

    }

    
    public class SegmentIntegerInfo : SegmentBaseRun
    {
        public EncodedType Encoded { get; private set; }
        public bool IsBigOrder { get; private set; }
        public bool IsSigned { get; private set; }
        public byte BitCount { get; private set; }
        private ushort _repeated;
        private ushort _value;
        private ushort _validator;
        public SegmentIntegerInfo(ulong token) : base(token)
        {
            const byte pos_encoded = 6;
            const byte pos_byteorder = 8;
            const byte pos_issigned = 9;
            const byte pos_bitcount = 10;
            const byte pos_repeated = 16;
            const byte pos_value = 32;
            const byte pos_validate = 48;
            const byte len_bitcount = 6;

            Encoded = (EncodedType)GetTokenByte(token, pos_encoded, 2);
            IsBigOrder = GetTokenBool(token, pos_byteorder);
            IsSigned = GetTokenBool(token, pos_issigned);
            BitCount = GetTokenByte(token, pos_bitcount, len_bitcount);
            _repeated = GetTokenUShort(token, pos_repeated);
            _value = GetTokenUShort(token, pos_value);
            _validator = GetTokenUShort(token, pos_validate);

        }
    }

    public class SegmentRealInfo : SegmentBaseRun
    {
        public bool IsDouble { get; private set; }
        public EncodedType Encoded { get; private set; }
        public bool IsBigOrder { get; private set; }
        private ushort _repeated;
        private ushort _value;
        private ushort _validator;
        public SegmentRealInfo(ulong token) : base(token)
        {

            const byte pos_encoded = 6;
            const byte pos_byteorder = 7;
            const byte pos_isdouble = 9;
            //const byte not_used = 10;
            const byte pos_repeated = 16;
            const byte pos_value = 32;
            const byte pos_validate = 48;
            Encoded = (EncodedType)GetTokenByte(token, pos_encoded, 2);
            IsBigOrder = GetTokenBool(token, pos_byteorder);
            IsDouble = GetTokenBool(token, pos_isdouble);
            _repeated = GetTokenUShort(token, pos_repeated);
            _value = GetTokenUShort(token, pos_value);
            _validator = GetTokenUShort(token, pos_validate);
        }
    }

    public class SegmentTextInfo : SegmentBaseRun
    {
        private ushort _repeated;
        private ushort _bytesize;
        public SegmentTextInfo(ulong token) : base(token)
        {
            const byte pos_repeated = 32;
            const byte pos_bytesize = 48;

            _repeated = GetTokenUShort(token, pos_repeated);
            _bytesize = GetTokenUShort(token, pos_bytesize);
        }
    }


    public class ConstValue
    {
        byte[] _data = new byte[9];
        public ConstValue(byte[] buff, int pos_start)
        {
            for(int i = 0; i<9; i++)
            {
                _data[i] = buff[pos_start];
            }
        }

        public ulong GetULong()
        {
            return Convert.ToUInt64(_data[1]);
        }

        public byte GetByte()
        {
            return _data[0];
        }

        public bool IsIntOne()
        {
            for (int i = 2; i < 9; i++)
                if (_data[i] != 0) return false;
            return _data[1] == 1;
        }
    }

    public class ExpInfo
    {
        const byte pos_left = 32;
        const byte pos_right = 48;
        private ExpType _optype;
        private ushort _left;
        private ushort _right;
        private ConstValue[] _const;
        public ExpInfo(ulong token)
        {
            _optype = (ExpType)SegmentBaseRun.GetTokenByte(token, 0, 8);
            _left = SegmentBaseRun.GetTokenUShort(token, pos_left);
            _right = SegmentBaseRun.GetTokenUShort(token, pos_right);
        }

        public bool IsConst
        {
            get
            {
                return _optype == ExpType.EXP_INT || _optype == ExpType.EXP_REAL;
            }
        }

        public bool IsIntOne()
        {
            return (_optype == ExpType.EXP_INT && _const[_left].IsIntOne());
        }
    } 

    public class Validator
    {
        public Validator(ulong token)
        {

        }
    }
    
    public enum ExpType
    {
        EXP_INT = 1,
        EXP_REAL,
        EXP_ID,
        EXP_BYTESIZEOF,
        EXP_ADD,
        EXP_SUB,
        EXP_MUL,
        EXP_DIV
    }

    public enum EncodedType
    {
        Primitive = 1,
        Inversion,
        Complement
    }
}
