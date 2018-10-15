using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    class SegmentIntegerRun : SegmentBaseRun
    {
        public EncodedType Encoded { get; private set; }
        public bool IsBigOrder { get; private set; }
        public bool IsSigned { get; private set; }
        public byte BitCount { get; private set; }

        private ushort _value;
        private ushort _validator;

        public SegmentIntegerRun(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte pos_encoded = 6;
            const byte pos_byteorder = 8;
            const byte pos_issigned = 9;
            const byte pos_bitcount = 10;
            const byte pos_value = 32;
            const byte pos_validate = 48;
            const byte len_bitcount = 6;

            Encoded = (EncodedType)GetTokenByte(token, pos_encoded, 2);
            IsBigOrder = GetTokenBool(token, pos_byteorder);
            IsSigned = GetTokenBool(token, pos_issigned);
            BitCount = GetTokenByte(token, pos_bitcount, len_bitcount);
            _value = GetTokenUShort(token, pos_value);
            _validator = GetTokenUShort(token, pos_validate);
        }

        #region --Pack--

        public override ushort Pack(IList<ulong> value_buff, Stream pack, ref ulong cach, ref int cach_pos, SegmentValueInfo info, IRunExp ir)
        {
            if(!info.IsSetValue)
            {
                if (_value == 0)
                    SetSegmentValue(value_buff, (ulong)0, info);
                else if (IsSigned)
                {
                    long v = (long)ir.GetExpValue(_value);
                    SetSegmentValue(value_buff, v, info);
                }
                else
                {
                    ulong v = (ulong)ir.GetExpValue(_value);
                    SetSegmentValue(value_buff, v, info);
                }
               
            }
            
            CommitValue(value_buff[info.StartPos], pack, ref cach, ref cach_pos, BitCount);
            return 0;
        }

        public override double GetValue(IList<ulong> value_buff, SegmentValueInfo info)
        {
            if (!info.IsSetValue)
                return 0;
            else
            {
                if (IsSigned)
                    return UnpackToLong(value_buff[info.StartPos], BitCount, Encoded, IsBigOrder);
                else
                    return IsBigOrder ? GetBigOrder(value_buff[info.StartPos], 64) : value_buff[info.StartPos];
            }
        }


        #endregion

        #region --Run--

        public override ushort GetBitLen(ref int bitlen, SegmentValueInfo info, IRunExp ir)
        {
            bitlen += BitCount;
            return 0;
        }


        #endregion


        #region --Unpack--

        public override ushort Unpack(byte[] buff, ref int pos_bit, SegmentUnpackInfo info)
        {
            throw new NotImplementedException();
        }

        public override ushort TryUnpack(ushort next_fill_seg, SegmentUnpackInfo info)
        {
            throw new NotImplementedException();
        }


        #endregion


        #region --SetValue--

        public void SetSegmentValue(IList<ulong> value_buff, ulong? value)
        {
            ulong newv = (value == null ? 0 : (ulong)value);
            if (IsBigOrder) newv = GetBigOrder(newv, BitCount);
            value_buff.Add(newv);
        }

        public void SetSegmentValue(IList<ulong> value_buff, long? value)
        {
            long v = (value == null ? 0 : (long)value);
            if (v < 0)
                if (IsSigned)
                    value_buff.Add(PackToULong(v, BitCount, Encoded, IsBigOrder));
                else
                    SetSegmentValue(value_buff, (ulong)-v);
            else
                SetSegmentValue(value_buff, (ulong)v);
        }


        public override void SetSegmentValue(IList<ulong> value_buff, ulong? value, SegmentValueInfo info)
        {
            info.StartPos = value_buff.Count;
            info.Count = 1;
            info.IsSetValue = true ;

            SetSegmentValue(value_buff, value);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, long? value, SegmentValueInfo info)
        {
            info.StartPos = value_buff.Count;
            info.Count = 1;
            info.IsSetValue = true;

            SetSegmentValue(value_buff, value);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, bool? value, SegmentValueInfo info)
        {
            if (value == null || !(bool)value)
                SetSegmentValue(value_buff, (ulong)1, info);
            else
                SetSegmentValue(value_buff, (ulong)0, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, sbyte? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (long?)value, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, byte? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (ulong?)value, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, short? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (long?)value, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, ushort? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (ulong?)value, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, int? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (long?)value, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, uint? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (ulong?)value, info);
        }


        public override void SetSegmentValue(IList<ulong> value_buff, float? value, SegmentValueInfo info)
        {
            if (IsSigned)
                SetSegmentValue(value_buff, (long?)value, info);
            else
                SetSegmentValue(value_buff, (ulong?)value, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, double? value, SegmentValueInfo info)
        {
            if (IsSigned)
                SetSegmentValue(value_buff, (long?)value, info);
            else
                SetSegmentValue(value_buff, (ulong?)value, info);
        }

        #endregion


    }
}
