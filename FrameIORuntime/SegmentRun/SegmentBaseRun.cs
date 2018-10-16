using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{

    public abstract class SegmentBaseRun
    {
        protected FrameInfo Parent { get; private set; }
        public SegmentBaseRun(ulong token, IRunInitial ir)
        {

        }

        #region --Pack--

        public abstract ushort Pack(IList<ulong> value_buff, MemoryStream pack, ref ulong cach, ref int pos, SegmentValueInfo info, IPackRunExp ir);

        //取字段值
        public virtual double GetValue(IList<ulong> value_buff, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        //取字段的位大小
        public abstract ushort GetBitLen(ref int bitlen, SegmentValueInfo info, IPackRunExp ir);

        #endregion

        #region --Unpack

        public abstract ushort Unpack(byte[] buff, ref int pos_bit, SegmentUnpackInfo info, IUnpackRunExp ir);

        //尝试取字段值
        public virtual bool TryGetValue(ref double value, byte[] buff,  SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        //尝试取字段的位大小
        public abstract bool TryGetBitLen(ref int bitlen, ref ushort nextseg, SegmentUnpackInfo info, IUnpackRunExp ir);

        #endregion

        #region --SetSegmentValue--


        public virtual void SetSegmentValue(IList<ulong> value_buff, bool? value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, byte? value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, sbyte? value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, ushort? value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, short? value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, uint? value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, int? value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, ulong? value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, long? value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, float? value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, double? value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, bool?[] value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, byte?[] value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, sbyte?[] value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, ushort?[] value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, short?[] value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, uint?[] value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, int?[] value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, ulong?[] value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, long?[] value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, float?[] value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual void SetSegmentValue(IList<ulong> value_buff, double?[] value, SegmentValueInfo info)
        {
            throw new Exception("runtime");
        }

        #endregion

        #region --GetSegmentValue--


        public virtual bool? GetBool(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual bool?[] GetBoolArray(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual byte? GetByte(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual byte?[] GetByteArray(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual double? GetDouble(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual double?[] GetDoubleArray(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual float? GetFloat(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual float?[] GetFloatArray(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual int? GetInt(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual int?[] GetIntArray(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual long? GetLong(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual long?[] GetLongArray(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual sbyte? GetSByte(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual sbyte?[] GetSByteArray(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual short? GetShort(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual short?[] GetShortArray(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual uint? GetUInt(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual uint?[] GetUIntArray(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual ulong? GetULong(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual ulong?[] GetULongArray(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual ushort? GetUShort(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual ushort?[] GetUShortArray(byte[] buff, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        #endregion

        #region --Helper--

        //取任意位的字节
        static public ulong GetUInt64FromByte(byte[] buff, uint bitStart)
        {
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
        static public ulong GetUIntxFromByte(byte[] buff, uint bitStart, int x)
        {
            return GetUInt64FromByte(buff, bitStart) & ((x != 0) ? (~(ulong)0 >> (sizeof(ulong) * 8 - x)) : (ulong)0);
        }

        //解包一个long数值
        protected static long UnpackToLong(ulong v, byte bitlen, EncodedType et, bool isbigorder)
        {
            ulong nv = isbigorder ? GetBigOrder(v, bitlen) : v;
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

        //解包一个double数值
        protected static double UnpackToDouble(ulong v, EncodedType et, bool isbigorder)
        {
            ulong nv = isbigorder ? GetBigOrder(v, 64) : v;
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
        protected static float UnpackToFloat(ulong v, EncodedType et, bool isbigorder)
        {
            ulong nv = isbigorder ? GetBigOrder(v, 32) : v;
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
        protected static void CommitValue(ulong value, Stream pack, ref ulong cach, ref int cach_pos, byte bitcount)
        {
            int newpos = cach_pos + bitcount;
            if (newpos < 64)
            {
                cach |= (value << cach_pos);
                cach &= (~(ulong)0) >> (64 - newpos);
                cach_pos = newpos;
            }
            else
            {
                ulong cv = cach | (value << cach_pos);
                pack.Write(BitConverter.GetBytes(cv), 0, 8);
                cach = value >> (64 - cach_pos);
                cach_pos = newpos - 64;
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
            for (int i=0; i< bcount; i++)
            {
                newv[i] = oldv[oldi-1];
                oldi -= 1;
            }
            return BitConverter.ToUInt64(newv,0);
        }

        public static byte GetTokenByte(ulong token, byte pos_tart, byte len)
        {
            return (byte)((token & (((~(ulong)0) << (64 - len)) >> (64 - len - pos_tart))) >> pos_tart);
        }

        public static ushort GetTokenUShort(ulong token, byte pos_tart)
        {
            return (ushort)((token & (((~(ulong)0) << (64 - 16)) >> (64 - 16 - pos_tart))) >> pos_tart);
        }

        public static bool GetTokenBool(ulong token, byte pos_tart)
        {
            return (token & ((ulong)1 << pos_tart)) != 0;
        }

        #endregion


    }
}
