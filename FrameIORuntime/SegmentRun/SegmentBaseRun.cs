using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{

    internal abstract class SegmentBaseRun
    {
        internal SegmentBaseRun(ulong token, IRunInitial ir)
        {

        }

        #region --Pack--

        internal abstract ushort Pack(MemoryStream value_buff, MemoryStream pack, ref byte odd, ref byte odd_pos, SetValueInfo info, IPackRunExp ir);

        //取字段值
        internal virtual double GetValue(MemoryStream value_buff, SetValueInfo info, IPackRunExp ir)
        {
            throw new Exception("runtime");
        }

        //取字段的位大小
        internal abstract ushort GetBitLen(MemoryStream value_buff, ref int bitlen, SetValueInfo info, IPackRunExp ir);

        #endregion

        #region --Unpack

        internal abstract ushort Unpack(byte[] buff, ref int pos_bit, int end_bit_pos, UnpackInfo info, IUnpackRunExp ir);

        //尝试取字段值
        internal virtual bool TryGetValue(ref double value, byte[] buff,  UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        //尝试取字段的位大小
        internal abstract bool TryGetBitLen(byte[] buff, ref int bitlen, ref ushort nextseg, UnpackInfo info, IUnpackRunExp ir);

        #endregion

        #region --SetSegmentValue--


        internal virtual void SetSegmentValue(MemoryStream value_buff, bool? value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, byte? value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, sbyte? value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, ushort? value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, short? value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, uint? value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, int? value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, ulong? value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, long? value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, float? value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, double? value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, bool?[] value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, byte?[] value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, sbyte?[] value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, ushort?[] value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, short?[] value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, uint?[] value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, int?[] value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, ulong?[] value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, long?[] value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, float?[] value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual void SetSegmentValue(MemoryStream value_buff, double?[] value, SetValueInfo info)
        {
            throw new Exception("runtime");
        }

        #endregion

        #region --GetSegmentValue--


        internal virtual bool? GetBool(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual bool?[] GetBoolArray(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual byte? GetByte(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual byte?[] GetByteArray(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual double? GetDouble(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual double?[] GetDoubleArray(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual float? GetFloat(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual float?[] GetFloatArray(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual int? GetInt(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual int?[] GetIntArray(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual long? GetLong(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual long?[] GetLongArray(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual sbyte? GetSByte(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual sbyte?[] GetSByteArray(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual short? GetShort(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual short?[] GetShortArray(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual uint? GetUInt(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual uint?[] GetUIntArray(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual ulong? GetULong(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual ulong?[] GetULongArray(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual ushort? GetUShort(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        internal virtual ushort?[] GetUShortArray(byte[] buff, UnpackInfo info)
        {
            throw new Exception("runtime");
        }

        #endregion

        #region --Helper--

        //取任意位的字节
        static internal ulong GetUInt64FromByte(byte[] buff, uint bitStart)
        {
            if (buff.Length < 8)
            {
                var old = buff;
                buff = new byte[8];
                for (int i = 0; i < old.Length; i++)
                    buff[i] = old[i];
            }
            uint word_index = bitStart >> 6;
            uint word_offset = bitStart & 63;
            ulong result = BitConverter.ToUInt64(buff, (int)word_index * 8) >> (UInt16)word_offset;
            uint bits_taken = 64 - word_offset;
            if (word_offset > 0 && bitStart + bits_taken < (uint)(8 * buff.Length))
            {
                result |= BitConverter.ToUInt64(buff, (int)(word_index + 1) * 8) << (UInt16)(64 - word_offset);
            }
            return result;
        }

        //取任意位的指定长度字节
        static internal ulong GetUIntxFromByte(byte[] buff, uint bitStart, int x)
        {
            return GetUInt64FromByte(buff, bitStart) & ((x != 0) ? (~(ulong)0 >> (sizeof(ulong) * 8 - x)) : (ulong)0);
        }

        //解包一个long数值
        protected static long UnpackToLong(byte[] buff, uint bit_start, byte bitlen, EncodedType et, bool isbigorder)
        {
            ulong nv = GetUIntxFromByte(buff, bit_start, bitlen);
            if (isbigorder) nv = GetBigOrder(nv, bitlen);
            switch (et)
            {
                case EncodedType.Primitive:
                    return BitConverter.ToInt64(BitConverter.GetBytes(nv), bitlen);
                case EncodedType.Inversion:
                    return BitConverter.ToInt64(BitConverter.GetBytes(GetInversion(nv, bitlen)), 0);
                case EncodedType.Complement:
                    return BitConverter.ToInt64(BitConverter.GetBytes(GetComplement(nv, bitlen)), 0);
            }
            throw new Exception("runtime");
        }

        //解包一个ulong值
        protected static ulong UnpackToULong(byte[] buff, uint bitstart, byte bitlen, bool isbigorder)
        {
            ulong nv = GetUIntxFromByte(buff, bitstart, bitlen);
            if (isbigorder) nv = GetBigOrder(nv, bitlen);
            return nv;
        }

        //解包一个double数值
        protected static double UnpackToDouble(byte[] buff, uint bit_start,  EncodedType et, bool isbigorder)
        {
            ulong nv = GetUIntxFromByte(buff, bit_start, 64);
            if (isbigorder) nv = GetBigOrder(nv, 64);
            switch (et)
            {
                case EncodedType.Primitive:
                    return BitConverter.ToDouble(BitConverter.GetBytes(nv),0);
                case EncodedType.Inversion:
                    return BitConverter.ToDouble(BitConverter.GetBytes(GetInversion(nv,64)), 0);
                case EncodedType.Complement:
                    return BitConverter.ToDouble(BitConverter.GetBytes(GetComplement(nv, 64)), 0);
            }
            throw new Exception("runtime");
        }

        //解包一个float数值
        protected static float UnpackToFloat(byte[] buff, uint bit_start, EncodedType et, bool isbigorder)
        {
            ulong nv = GetUIntxFromByte(buff, bit_start, 32);
            if (isbigorder) nv = GetBigOrder(nv, 32);
            switch (et)
            {
                case EncodedType.Primitive:
                    return BitConverter.ToSingle(BitConverter.GetBytes((uint)nv), 0);
                case EncodedType.Inversion:
                    return BitConverter.ToSingle(BitConverter.GetBytes((uint)GetInversion(nv, 32)), 0);
                case EncodedType.Complement:
                    return BitConverter.ToSingle(BitConverter.GetBytes((uint)GetComplement(nv, 32)), 0);
            }
            throw new Exception("runtime");
        }


        //将数值写入打包cach
        protected static void CommitValue(MemoryStream cach, int start, int bitlen, Stream pack, ref byte odd, ref byte odd_pos)
        {
            
            var _cach = cach.GetBuffer();
            int count = bitlen / 8;
            if(odd_pos==0)
            {
                if (count > 0) pack.Write(_cach, start, count);
                odd_pos = (byte)(bitlen % 8);
                if (odd_pos!= 0)  odd = _cach[start + count];
            }
            else
            {
                for(int i=0; i<count; i++)
                {
                    pack.WriteByte((byte)((_cach[start + i] << odd_pos) | odd));
                    odd = (byte)(_cach[start + i] >> (8 - odd_pos));
                }
                byte new_odd_pos = (byte)(bitlen % 8);
                if(new_odd_pos!=0)
                {
                    var v = _cach[start + count];
                    if(new_odd_pos+odd_pos<8)
                    {
                        odd |= (byte)(v << odd_pos);
                        odd_pos = (byte)(new_odd_pos + odd_pos);
                    }
                    else
                    {
                        pack.WriteByte((byte)((v << odd_pos) | odd));
                        odd = (byte)(v >> (8 - odd_pos));
                        odd_pos = (byte)(new_odd_pos + odd_pos - 8);
                    }
                }
            }
        }


        //打包一个long数值
        protected static ulong PackToULong(long v, byte bitlen, EncodedType et, bool isbigorder)
        {
            ulong unewv = BitConverter.ToUInt64(BitConverter.GetBytes(v), 0);
            switch (et)
            {
                case EncodedType.Primitive:
                    break;
                case EncodedType.Inversion:
                    unewv = GetInversion(unewv, bitlen);
                    break;
                case EncodedType.Complement:
                    unewv = GetComplement(unewv, bitlen);
                    break;
                default:
                    break;
            }
            return isbigorder ? GetBigOrder(unewv, bitlen) : unewv;
        }

        //打包一个double数值
        protected static ulong PackToULong(double v, EncodedType et, bool isbigorder)
        {
            ulong unewv = BitConverter.ToUInt64(BitConverter.GetBytes(v), 0);
            if(v<0)
            {
                switch (et)
                {
                    case EncodedType.Primitive:
                        break;
                    case EncodedType.Inversion:
                        unewv = GetInversion(unewv, 64);
                        break;
                    case EncodedType.Complement:
                        unewv = GetComplement(unewv, 64);
                        break;
                    default:
                        break;
                }
            }

            return isbigorder ? GetBigOrder(unewv, 64) : unewv;
        }

        //打包一个float数值
        protected ulong PackToULong(float v, EncodedType et, bool isbigorder)
        {
            ulong unewv = BitConverter.ToUInt32(BitConverter.GetBytes(v), 0);
            if (v < 0)
            {
                switch (et)
                {
                    case EncodedType.Primitive:
                        break;
                    case EncodedType.Inversion:
                        unewv = GetInversion(unewv, 32);
                        break;
                    case EncodedType.Complement:
                        unewv = GetComplement(unewv, 32);
                        break;
                    default:
                        break;
                }
            }
            return isbigorder ? GetBigOrder(unewv, 32) : unewv;
        }

        //取负数的反码
        protected static ulong GetInversion(ulong value, byte bitlen)
        {
            value |= (~(ulong)0 << (bitlen - 1));
            value ^= (~(ulong)0 >> (64 - bitlen - 1));
            return value;
        }

        //取负数的补码
        protected static ulong GetComplement(ulong value, byte bitlen)
        {
            return GetInversion(value, bitlen) + 1;
        }

        //转大端序
        protected static ulong GetBigOrder(ulong value, byte bitlen)
        {
            var oldv = BitConverter.GetBytes(value);
            var newv = new byte[8];

            int bcount = bitlen / 8;
            if (bitlen % 8 != 0) bcount += 1;
            var oldi = bcount;
            for (int i = 0; i < bcount; i++)
            {
                newv[i] = oldv[oldi - 1];
                oldi -= 1;
            }
            return BitConverter.ToUInt64(newv, 0);
        }

        protected static byte[] GetBigOrder(byte[] oldv, byte bitlen)
        {
            var newv = new byte[8];

            int bcount = bitlen / 8;
            if (bitlen % 8 != 0) bcount += 1;
            var oldi = bcount;
            for (int i = 0; i < bcount; i++)
            {
                newv[i] = oldv[oldi - 1];
                oldi -= 1;
            }
            return newv;
        }



        internal static byte GetTokenByte(ulong token, byte pos_tart, byte len)
        {
            return (byte)((token & (((~(ulong)0) << (64 - len)) >> (64 - len - pos_tart))) >> pos_tart);
        }

        internal static ushort GetTokenUShort(ulong token, byte pos_tart)
        {
            return (ushort)((token & (((~(ulong)0) << (64 - 16)) >> (64 - 16 - pos_tart))) >> pos_tart);
        }

        internal static bool GetTokenBool(ulong token, byte pos_tart)
        {
            return (token & ((ulong)1 << pos_tart)) != 0;
        }

        #endregion


    }
}
