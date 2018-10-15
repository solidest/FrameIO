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
            __frames.Add(framename, frm);
        }

        //获取数据帧
        public static FrameInfo GetFrame(string framename)
        {
            return __frames[framename];
        }

    }

    public class FrameInfo : IRunInitial
    {
        const byte LEN_USHORT = 16;
        const byte LEN_BYTE = 8;
        const byte LEN_CODE = 6;

        private SegmentBaseRun[] _segs;
        private double[] _consts;
        private ExpRun[] _explist;
        private Validator[] _validats;

        public FrameInfo(byte[] content)
        {
            //const byte pos_segment = 0;
            //const byte pos_const = 16;
            //const byte pos_expression = 32;
            //const byte pos_validator = 48;

            ushort segmentcount = BitConverter.ToUInt16(content, 0);
            ushort constcount = BitConverter.ToUInt16(content, 2);
            ushort expressioncount = BitConverter.ToUInt16(content, 4);
            ushort validatorcount = BitConverter.ToUInt16(content, 6);

            if (content.Length != segmentcount*8 + constcount*8 + expressioncount*8 + validatorcount*8 + 8)
                throw new Exception("runtime");

            int pos = 8;

            //WriteMemory(mst, _constlist);
            //WriteMemory(mst, _expression);
            //WriteMemory(mst, _validatorlist);
            //WriteMemory(mst, _segmentlist);

            _consts = new double[constcount];
            pos += 8;
            for (int i = 0; i < constcount; i++)
            {
                _consts[i] = BitConverter.ToDouble(content, pos);
                pos += 8;
            }
 
            _explist = new ExpRun[expressioncount];
            pos += 8;
            for(int i=1; i< expressioncount; i++)
            {
                _explist[i] = new ExpRun(BitConverter.ToUInt64(content, pos));
                pos += 8;
            }

            _validats = new Validator[validatorcount];
            pos += 8;
            for(int i=1; i< validatorcount; i++)
            {
                _validats[i] = new Validator(BitConverter.ToUInt64(content, pos));
                pos += 8;
            }

            _segs = new SegmentBaseRun[segmentcount];
            pos += 8;
            for (int i = 1; i < segmentcount; i++)
            {
                _segs[i] = SegmentFactory[content[pos]&(~0<<LEN_CODE)].Invoke(BitConverter.ToUInt64(content, pos), this);
                pos += 8;
            }
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

        private delegate SegmentBaseRun CreateSegment(ulong data, IRunInitial ir);

        private static CreateSegment[] SegmentFactory = new CreateSegment[]
        {
            //const byte CO_SEGINTEGER = 1;
            //const byte CO_SEGREAL = 2;
            //const byte CO_SEGTEXT = 3;
            //const byte CO_SEGINTEGER_ARRAY = 4;
            //const byte CO_SEGREAL_ARRAY = 5;
            //const byte CO_SEGTEXT_ARRAY = 6;
            //const byte CO_SEGBLOCK_IN = 7;
            //const byte CO_SEGBLOCK_OUT = 8;
            //const byte CO_SEGONEOF_IN = 9;
            //const byte CO_SEGONEOF_OUT = 10;
            //const byte CO_SEGONEOFITEM_IN = 11;
            //const byte CO_SEGONEOFITEM_OUT = 12;
            CreateSegmentNullRun, 
            CreateSegmentIntegerRun,
            CreateSegmentRealRun,
            CreateSegmentTextRun,
            CreateSegmentIntegerArrayRun,
            CreateSegmentRealArrayRun,
            CreateSegmentTextArrayRun,
            CreateSegmentNullRun,
            CreateSegmentNullRun,
            CreateSegmentNullRun 
        };

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

        #endregion

    }



    public class Validator
    {
        public Validator(ulong token)
        {

        }
    }
    


    public enum EncodedType
    {
        Primitive = 1,
        Inversion,
        Complement
    }
}
