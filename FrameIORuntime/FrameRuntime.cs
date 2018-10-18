using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{

    //数据帧运行时信息
    internal class FrameRuntime : IRunInitial
    {
        const byte LEN_USHORT = 16;
        const byte LEN_BYTE = 8;
        const byte LEN_CODE = 6;

        private SegmentBaseRun[] _segs;
        private double[] _consts;
        private ExpRun[] _explist;
        private SegmentValidator[] _validats;
        private ushort _endall;

        internal static void Initial(byte[] content)
        {
            Run = new FrameRuntime(content);
        }

        internal static FrameRuntime Run { get; private set; }

        private FrameRuntime(byte[] content)
        {
            //const byte pos_segment = 0;
            //const byte pos_const = 16;
            //const byte pos_expression = 32;
            //const byte pos_validator = 48;

            ushort segmentcount = BitConverter.ToUInt16(content, 0);
            ushort constcount = BitConverter.ToUInt16(content, 2);
            ushort expressioncount = BitConverter.ToUInt16(content, 4);
            ushort validatorcount = BitConverter.ToUInt16(content, 6);

            if (content.Length != (segmentcount + constcount + expressioncount  + validatorcount +1) * 8)
                throw new Exception("runtime");

            int pos = 8;

            //WriteMemory(mst, _constlist);
            //WriteMemory(mst, _expression);
            //WriteMemory(mst, _validatorlist);
            //WriteMemory(mst, _segmentlist);

            _consts = new double[constcount];
            for (int i = 0; i < constcount; i++)
            {
                _consts[i] = BitConverter.ToDouble(content, pos);
                pos += 8;
            }
 
            _explist = new ExpRun[expressioncount];
            for(int i=1; i< expressioncount; i++)
            {
                _explist[i] = new ExpRun(BitConverter.ToUInt64(content, pos));
                pos += 8;
            }

            _validats = new SegmentValidator[validatorcount];
            for(int i=1; i< validatorcount; i++)
            {
                _validats[i] = SegmentValidator.GetSegmentValidator(BitConverter.ToUInt64(content, pos), this);
                pos += 8;
            }

            _segs = new SegmentBaseRun[segmentcount];
            for (int i = 1; i < segmentcount; i++)
            {
                _segs[i] = SegmentFactory[content[pos]&(~0<<LEN_CODE)].Invoke(BitConverter.ToUInt64(content, pos), this);
                pos += 8;
            }
            _endall = (ushort)(_segs.Length - 1);
        }

        internal SegmentBaseRun this[int index]
        {
            get
            {
                return _segs[index];
            }
        }

        internal int SegmentsCount { get => _segs.Length; }
        internal bool IsEnd(ushort idx)
        {
            return (idx == _endall);
        }


        #region --SegmentFactory--

        private delegate SegmentBaseRun CreateSegment(ulong data, IRunInitial ir);

        private static CreateSegment[] SegmentFactory = new CreateSegment[]
        {

            CreateSegmentNullRun, 
            CreateSegmentIntegerRun,//const byte CO_SEGINTEGER = 1;
            CreateSegmentRealRun,//const byte CO_SEGREAL = 2;
            CreateSegmentTextRun,//const byte CO_SEGTEXT = 3;
            CreateSegmentIntegerArrayRun,//const byte CO_SEGINTEGER_ARRAY = 4;
            CreateSegmentRealArrayRun,//const byte CO_SEGREAL_ARRAY = 5;
            CreateSegmentTextArrayRun,//const byte CO_SEGTEXT_ARRAY = 6;
            CreateSegmentBlockIn,//const byte CO_SEGBLOCK_IN = 7;
            CreateSegmentNormalOut,//const byte CO_SEGBLOCK_OUT = 8;
            CreateSegmentOneofInto,//const byte CO_SEGONEOF_IN = 9;
            CreateSegmentNormalOut,//const byte CO_SEGONEOF_OUT = 10;
            CreateSegmentOneofItem,//const byte CO_SEGONEOFITEM = 11;
            CreateSegmentFrameBegin,//const byte CO_FRAME_BEGIN = 12;
            CreateSegmentNormalOut,//const byte CO_FRAME_END = 13;
            CreateSegmentFrameRef//const byte CO_REF_FRAME = 14;
        };

        private static SegmentBaseRun CreateSegmentOneofInto(ulong data, IRunInitial ir)
        {
            return new SegmentOneofInto(data,ir);
        }

        private static SegmentBaseRun CreateSegmentOneofItem(ulong data, IRunInitial ir)
        {
            return new SegmentOneofItem(data, ir);
        }

        private static SegmentBaseRun CreateSegmentFrameRef(ulong data, IRunInitial ir)
        {
            return new SegmentFrameRef(data, ir);
        }

        private static SegmentBaseRun CreateSegmentFrameBegin(ulong data, IRunInitial ir)
        {
            return new SegmentFramBegin(data, ir);
        }

        private static SegmentBaseRun CreateSegmentNullRun(ulong data, IRunInitial ir)
        {
            return null;
        }

        private static SegmentBaseRun CreateSegmentIntegerRun(ulong data, IRunInitial ir)
        {
            return new SegmentIntegerRun(data, ir);
        }

        private static SegmentBaseRun CreateSegmentRealRun(ulong data, IRunInitial ir)
        {
            return new SegmentRealRun(data, ir);
        }

        private static SegmentBaseRun CreateSegmentTextRun(ulong data, IRunInitial ir)
        {
            return new SegmentTextRun(data, ir);
        }

        private static SegmentBaseRun CreateSegmentIntegerArrayRun(ulong data, IRunInitial ir)
        {
            return new SegmentIntegerArrayRun(data, ir);
        }

        private static SegmentBaseRun CreateSegmentRealArrayRun(ulong data, IRunInitial ir)
        {
            return new SegmentRealArrayRun(data, ir);
        }

        private static SegmentBaseRun CreateSegmentTextArrayRun(ulong data, IRunInitial ir)
        {
            return new SegmentTextArrayRun(data, ir);
        }

        private static SegmentBaseRun CreateSegmentBlockIn(ulong data, IRunInitial ir)
        {
            return new SegmentBlockIn(data, ir);
        }

        private static SegmentBaseRun CreateSegmentNormalOut(ulong data, IRunInitial ir)
        {
            return new SegmentNormalOut(data, ir);
        }


        #endregion

        #region --IRunInitial--

        public double GetConst(ushort idx)
        {
            return _consts[idx];
        }

        public ExpRun GetExp(ushort idx)
        {
            return _explist[idx];
        }

        public bool IsConst(ushort idx)
        {
            return _explist[idx].IsConst(this);
        }

        public bool IsConstOne(ushort idx)
        {
            return _explist[idx].IsConstOne(this);
        }

        public SegmentValidator GetValidator(ushort idx, ValidateType type)
        {
            var vlid = _validats[idx];
            while (vlid.NextIdx != 0)
            {
                vlid = _validats[vlid.NextIdx];
                if (vlid.ValidType == type) return vlid;
            }
            return null;
        }


        #endregion

    }


    //初始化接口
    internal interface IRunInitial
    {
        double GetConst(ushort idx);

        bool IsConst(ushort idx);

        bool IsConstOne(ushort idx);
        SegmentValidator GetValidator(ushort idx, ValidateType type);
    }

    //打包动态执行接口
    internal interface IPackRunExp : IRunInitial
    {
        double GetSegmentValue(ushort idx);
        int GetSegmentByteSize(ushort idx);
        ushort GetBitLen(MemoryStream value_buff, ref int bitlen, ushort idx);
        double GetExpValue(ushort idx);
        SetValueInfo GetSetValueInfo(ushort idx);
    }

    //解包动态执行接口
    internal interface IUnpackRunExp:IRunInitial
    {
        bool TryGetSegmentValue(ref double value, ushort idx);
        bool TryGetSegmentByteSize(ref double size, ushort idx);
        bool TryGetBitLen(byte[] buff, ref int bitlen, ref ushort nextseg, ushort idx);
        bool TryGetExpValue(ref double value, ushort idx);
        UnpackInfo GetUnpackInfo(ushort idx);
    }


    //编码类型
    internal enum EncodedType
    {
        Primitive = 1,
        Inversion,
        Complement
    }


}
