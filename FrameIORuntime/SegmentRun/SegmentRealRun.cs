using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    public class SegmentRealRun : SegmentBaseRun
    {
        public bool IsDouble { get; private set; }
        public EncodedType Encoded { get; private set; }
        public bool IsBigOrder { get; private set; }
        private ushort _value;
        private ushort _validator;
        public SegmentRealRun(ulong token, IRunInitial ir) : base(token, ir)
        {

            const byte pos_encoded = 6;
            const byte pos_byteorder = 7;
            const byte pos_isdouble = 9;
            //const byte not_used = 10;
            const byte pos_value = 32;
            const byte pos_validate = 48;
            Encoded = (EncodedType)GetTokenByte(token, pos_encoded, 2);
            IsBigOrder = GetTokenBool(token, pos_byteorder);
            IsDouble = GetTokenBool(token, pos_isdouble);
            _value = GetTokenUShort(token, pos_value);
            _validator = GetTokenUShort(token, pos_validate);
        }

        #region --Pack--

        public override ushort Pack(IList<ulong> value_buff, Stream pack, ref ulong cach, ref int pos, SegmentValueInfo info, IRunExp ir)
        {
            if (!info.IsSetValue)
            {
                if (_value == 0)
                    SetSegmentValue(value_buff, (double)0, info);
                else
                { 
                    double v = ir.GetExpValue(_value);
                    SetSegmentValue(value_buff, v, info);
                }
            }
            CommitValue(value_buff[info.StartPos], pack, ref cach, ref pos, (byte)(IsDouble?64:32));
            return 0;
        }

        //取字段值
        public override double GetValue(IList<ulong> value_buff, SegmentValueInfo info)
        {
            if (!info.IsSetValue)
                return 0;
            else
                return IsDouble? UnpackToDouble(value_buff[info.StartPos], Encoded, IsBigOrder) : UnpackToFloat(value_buff[info.StartPos], Encoded, IsBigOrder);
        }


        //取字段的字节大小
        public override ushort GetBitLen(ref int bitlen, SegmentValueInfo info, IRunExp ir)
        {
            bitlen += ( IsDouble ? 64 : 32);
            return 0;
        }

        #endregion

        #region --Unpack

        public override ushort Unpack(byte[] buff, ref int pos_bit, SegmentUnpackInfo info)
        {
            return 0;
        }

        public override ushort TryUnpack(ushort next_fill_seg, SegmentUnpackInfo info)
        {
            return 0;
        }

        #endregion


        #region --SetValue--


        public void SetSegmentValue(IList<ulong> value_buff, double? value)
        {
            double v = (value == null ? 0 : (double)value);
            value_buff.Add(PackToULong(v, Encoded, IsBigOrder));
        }

        public void SetSegmentValue(IList<ulong> value_buff, float? value)
        {
            float v = (value == null ? 0 : (float)value);
            value_buff.Add(PackToULong(v, Encoded, IsBigOrder));
        }

        public override void SetSegmentValue(IList<ulong> value_buff, float? value, SegmentValueInfo info)
        {
            info.IsSetValue = true;
            info.Count = 1;
            info.StartPos = value_buff.Count;

            SetSegmentValue(value_buff, value);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, double? value, SegmentValueInfo info)
        {
            info.IsSetValue = true;
            info.Count = 1;
            info.StartPos = value_buff.Count;

            SetSegmentValue(value_buff, value);
        }


        public override void SetSegmentValue(IList<ulong> value_buff, ulong? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (double)value, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, long? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (double)value, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, bool? value, SegmentValueInfo info)
        {
            if (value == null || !(bool)value)
                SetSegmentValue(value_buff, (float)1, info);
            else
                SetSegmentValue(value_buff, (float)0, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, sbyte? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (float)value, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, byte? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (float)value, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, short? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (float)value, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, ushort? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (float)value, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, int? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (float)value, info);
        }

        public override void SetSegmentValue(IList<ulong> value_buff, uint? value, SegmentValueInfo info)
        {
            SetSegmentValue(value_buff, (float)value, info);
        }



        #endregion
    }
}
