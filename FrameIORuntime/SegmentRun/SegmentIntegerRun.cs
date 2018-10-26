using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    internal class SegmentIntegerRun : SegmentBaseRun
    {
        public EncodedType Encoded { get; private set; }
        public bool IsBigOrder { get; private set; }
        public bool IsSigned { get; private set; }
        public byte BitCount { get; private set; }

        private SegmentMaxValidator _vlidmax;
        private SegmentMinValidator _vlidmin;
        private SegmentCheckValidator _vlidcheck;

        private ushort _value;

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
            if (BitCount == 0) BitCount = 64;
            _value = GetTokenUShort(token, pos_value);
            var validator = GetTokenUShort(token, pos_validate);
            if(validator!=0)
            {
                _vlidmax = (SegmentMaxValidator)ir.GetValidator(validator, ValidateType.Max);
                _vlidmin = (SegmentMinValidator)ir.GetValidator(validator, ValidateType.Min);
                _vlidcheck = (SegmentCheckValidator)ir.GetValidator(validator, ValidateType.Check);
            }
        }

        #region --Pack--

        //打包到数据帧
        internal override ushort Pack(MemoryStream value_buff, MemoryStream pack, ref byte odd, ref byte odd_pos, SetValueInfo info, IPackRunExp ir)
        {
            if(!info.IsSetValue) SetAutoValue(value_buff, pack, info, ir);
            
            CommitValue(value_buff.GetBuffer(), info.StartPos, BitCount, pack, ref odd, ref odd_pos);
            return 0;
        }

        //解包一个数值 用于计算
        internal override double GetValue(MemoryStream value_buff, SetValueInfo info, IPackRunExp ir)
        {
            if (!info.IsSetValue) SetAutoValue(value_buff, null,info, ir);

            if (IsSigned)
                return UnpackToLong(value_buff.GetBuffer(), (uint)info.StartPos*8, BitCount, Encoded, IsBigOrder);
            else
                return UnpackToULong(value_buff.GetBuffer(), (uint)info.StartPos*8, BitCount, IsBigOrder);

        }

        //取字段位长度
        internal override ushort GetBitLen(MemoryStream value_buff, ref int bitlen, SetValueInfo info, IPackRunExp ir)
        {
            bitlen += BitCount;
            return 0;
        }

        #endregion

        #region --Unpack--

        internal override ushort Unpack(byte[] buff, ref int pos_bit, int end_bit_pos, UnpackInfo info, IUnpackRunExp ir)
        {
            if (info.IsUnpack) return 0;

            info.IsUnpack = true;
            info.BitStart = pos_bit;
            info.BitLen = BitCount;

            if(_vlidmax!=null || _vlidmin!=null || _vlidcheck!=null)
            {
                double v;
                if (IsSigned)
                    v = UnpackToLong(buff, (uint)info.BitStart, BitCount, Encoded, IsBigOrder);
                else
                    v = UnpackToULong(buff, (uint)info.BitStart, BitCount, IsBigOrder);
                if (_vlidmax != null) if (!_vlidmax.Valid(v)) ir.AddErrorInfo("超出最大值设置", this);
                if (_vlidmin != null) if (!_vlidmin.Valid(v)) ir.AddErrorInfo("低于最小值设置", this);
                if (_vlidcheck != null)
                {
                    if (v != (_vlidcheck.GetCheckValue(buff, pos_bit / 8)& (~(ulong)0)>>(64-BitCount))) ir.AddErrorInfo("校验值错误", this); ;
                }
            }
            pos_bit += BitCount;
            return 0;
        }


        //尝试取字段值
        internal override bool TryGetValue(ref double value, byte[] buff, UnpackInfo info)
        {
            if (info.IsUnpack)
            {
                if (IsSigned)
                    value = UnpackToLong(buff, (uint)info.BitStart, BitCount, Encoded, IsBigOrder);
                else
                    value = UnpackToULong(buff, (uint)info.BitStart, BitCount, IsBigOrder);
                return true;
            }
            else
                return false;
        }

        //尝试取字段的位大小
        internal override bool TryGetNeedBitLen(byte[] buff, ref int bitlen, ref ushort nextseg, UnpackInfo info, IUnpackRunExp ir)
        {
            if (info.IsUnpack)
            {
                nextseg = 0;
                return true;
            }
            bitlen += BitCount;
            nextseg = 0;
            return true;
        }


        #endregion

        #region --SetValue--

        private void SetAutoValue(MemoryStream value_buff, MemoryStream pack, SetValueInfo info, IPackRunExp ir)
        {
            if (_vlidcheck != null && pack != null)
            {
                SetSegmentValue(value_buff, _vlidcheck.GetCheckValue(pack.GetBuffer(), (int)pack.Position),info);
                return;
            }

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

        public void SetSegmentValue(MemoryStream value_buff, ulong? value)
        {
            ulong v = (value == null ? 0 : (ulong)value);
            var buf = BitConverter.GetBytes(v);
            if (IsBigOrder) buf = GetBigOrder(buf, BitCount);
            int count = (BitCount % 8 == 0) ? (BitCount / 8 ): (BitCount / 8 + 1);
            value_buff.Write(buf, 0, count);
        }

        public void SetSegmentValue(MemoryStream value_buff, long? value)
        {
            long v = (value == null ? 0 : (long)value);
            if (v < 0)
                if (IsSigned)
                    value_buff.Write(BitConverter.GetBytes(PackToULong(v, BitCount, Encoded, IsBigOrder)), 0, ((BitCount % 8 == 0) ? (BitCount / 8) : (BitCount / 8 + 1)));
                else
                    SetSegmentValue(value_buff, (ulong)v);
            else
                SetSegmentValue(value_buff, (ulong)v);
        }


        internal override void SetSegmentValue(MemoryStream value_buff, ulong? value, SetValueInfo info)
        {
            info.StartPos = (int)value_buff.Position;
            info.Count = 1;
            info.BitLen = BitCount;
            info.IsSetValue = true ;

            SetSegmentValue(value_buff, value);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, long? value, SetValueInfo info)
        {
            info.StartPos = (int)value_buff.Position;
            info.Count = 1;
            info.BitLen = BitCount;
            info.IsSetValue = true;

            SetSegmentValue(value_buff, value);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, bool? value, SetValueInfo info)
        {
            if (value == null || !(bool)value)
                SetSegmentValue(value_buff, (ulong)0, info);
            else
                SetSegmentValue(value_buff, (ulong)1, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, sbyte? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (long?)value, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, byte? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (ulong?)value, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, short? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (long?)value, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, ushort? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (ulong?)value, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, int? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (long?)value, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, uint? value, SetValueInfo info)
        {
            SetSegmentValue(value_buff, (ulong?)value, info);
        }


        internal override void SetSegmentValue(MemoryStream value_buff, float? value, SetValueInfo info)
        {
            if (IsSigned)
                SetSegmentValue(value_buff, (long?)value, info);
            else
                SetSegmentValue(value_buff, (ulong?)value, info);
        }

        internal override void SetSegmentValue(MemoryStream value_buff, double? value, SetValueInfo info)
        {
            if (IsSigned)
                SetSegmentValue(value_buff, (long?)value, info);
            else
                SetSegmentValue(value_buff, (ulong?)value, info);
        }



        #endregion

        #region --GetValue--


        internal override bool? GetBool(byte[] buff, UnpackInfo info)
        {
            if (!info.IsUnpack) return null;
            var v = GetUIntxFromByte(buff, (uint)info.BitStart, BitCount);
            return (v != 0);
        }


        internal override byte? GetByte(byte[] buff, UnpackInfo info)
        {
            return (byte?)GetULong(buff, info);
        }


        internal override double? GetDouble(byte[] buff, UnpackInfo info)
        {
            return (double?)GetLong(buff, info);
        }

        internal override float? GetFloat(byte[] buff, UnpackInfo info)
        {
            return (float?)GetLong(buff, info);
        }


        internal override int? GetInt(byte[] buff, UnpackInfo info)
        {
            return (int?)GetLong(buff, info);
        }

        internal override long? GetLong(byte[] buff, UnpackInfo info)
        {
            if (!info.IsUnpack) return null;
            
            if (IsSigned)
                return UnpackToLong(buff,(uint)info.BitStart, BitCount, Encoded, IsBigOrder);
            else
                return (long)GetUIntxFromByte(buff, (uint)info.BitStart, BitCount);
        }

        internal override sbyte? GetSByte(byte[] buff, UnpackInfo info)
        {
            return (sbyte?)GetLong(buff, info);
        }

        internal override short? GetShort(byte[] buff, UnpackInfo info)
        {
            return (short?)GetLong(buff, info);
        }

        internal override uint? GetUInt(byte[] buff, UnpackInfo info)
        {
            return (uint?)GetULong(buff, info);
        }

        internal override ulong? GetULong(byte[] buff, UnpackInfo info)
        {
            if (!info.IsUnpack) return null;
            if (IsSigned)
                return (ulong)UnpackToLong(buff, (uint)info.BitStart, BitCount, Encoded, IsBigOrder);
            else
                return GetUIntxFromByte(buff, (uint)info.BitStart, BitCount);
        }


        internal override ushort? GetUShort(byte[] buff, UnpackInfo info)
        {
            return (ushort?)GetULong(buff, info);
        }

        #endregion

    }
}
