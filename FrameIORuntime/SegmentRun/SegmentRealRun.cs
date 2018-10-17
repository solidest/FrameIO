using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    internal class SegmentRealRun : SegmentBaseRun
    {
        internal bool IsDouble { get; private set; }
        internal EncodedType Encoded { get; private set; }
        internal bool IsBigOrder { get; private set; }
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

        internal override ushort Pack(MemoryStream value_buff, MemoryStream pack, ref byte odd, ref byte odd_pos, SetValueInfo info, IPackRunExp ir)
        {
            if (!info.IsSetValue) SetAutoValue(value_buff, info, ir);

            CommitValue(value_buff, info.StartPos, IsDouble ? 64 : 32, pack, ref odd, ref odd_pos);
            return 0;
        }

        //解包用于计算
        internal override double GetValue(MemoryStream value_buff, SetValueInfo info, IPackRunExp ir)
        {
            if (!info.IsSetValue) SetAutoValue(value_buff, info, ir);

            return IsDouble? UnpackToDouble(value_buff.GetBuffer(), (uint)info.StartPos*8, Encoded, IsBigOrder) : UnpackToFloat(value_buff.GetBuffer(),(uint)info.StartPos*8, Encoded, IsBigOrder);
        }


        //取字段的字节大小
        internal override ushort GetBitLen(ref int bitlen, SetValueInfo info, IPackRunExp ir)
        {
            bitlen += ( IsDouble ? 64 : 32);
            return 0;
        }

        #endregion

        #region --Unpack

        internal override ushort Unpack(byte[] buff, ref int pos_bit, UnpackInfo info, IUnpackRunExp ir)
        {
            info.IsUnpack = true;
            info.BitStart = pos_bit;
            info.BitLen = IsDouble?64:32;

            //HACK 验证规则处理

            pos_bit += IsDouble ? 64 : 32;
            return 0;
        }


        //尝试取字段值
        internal override bool TryGetValue(ref double value, byte[] buff, UnpackInfo info)
        {
            if (info.IsUnpack)
            {
                if (IsDouble)
                    value = UnpackToDouble(buff, (uint)info.BitStart, Encoded, IsBigOrder);
                else
                    value = UnpackToFloat(buff, (uint)info.BitStart, Encoded, IsBigOrder);
                return true;
            }
            else
                return false;
        }

        //尝试取字段的位大小
        internal override bool TryGetBitLen(ref int bitlen, ref ushort nextseg, UnpackInfo info, IUnpackRunExp ir)
        {
            bitlen += IsDouble ? 64 : 32;
            return true;
        }



        #endregion


        #region --SetValue--

        private void SetAutoValue(MemoryStream value_buff, SetValueInfo info, IPackRunExp ir)
        {
            if (_value == 0)
                SetSegmentValue(value_buff, (double)0, info);
            else
            {
                double v = ir.GetExpValue(_value);
                SetSegmentValue(value_buff, v, info);
            }
        }


        internal void SetSegmentValue(MemoryStream value_buff, double? value)
        {
            double v = (value == null ? 0 : (double)value);
            value_buff.Write(BitConverter.GetBytes(PackToULong(v, Encoded, IsBigOrder)), 0, IsDouble ? 8 : 4);
        }

        internal void SetSegmentValue(MemoryStream value_buff, float? value)
        {
            float v = (value == null ? 0 : (float)value);
            value_buff.Write(BitConverter.GetBytes(PackToULong(v, Encoded, IsBigOrder)), 0, IsDouble ? 8 : 4);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, float? value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.Count = 1;
            info.StartPos = (int)value_buff.Position;
            info.BitLen = 64;

            SetSegmentValue(value_buff, value);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, double? value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.Count = 1;
            info.StartPos = (int)value_buff.Position;
            info.BitLen = 64;

            SetSegmentValue(value_buff, value);
        }


        internal override void SetSegmentValue(MemoryStream value_buff, ulong? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (double)value, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, long? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (double)value, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, bool? value, SetValueInfo info)
        {
            if (value == null || !(bool)value)
                SetSegmentValue(value_buff, (float)1, info);
            else
                SetSegmentValue(value_buff, (float)0, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, sbyte? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (float)value, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, byte? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (float)value, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, short? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (float)value, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, ushort? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (float)value, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, int? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (float)value, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, uint? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (float)value, info);
        }


        #endregion
    }
}
