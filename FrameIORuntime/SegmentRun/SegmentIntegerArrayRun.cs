using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    internal class SegmentIntegerArrayRun : SegmentIntegerRun
    {
        private ushort _repeated_idx;
        private int _repeated_const = -1;

        public SegmentIntegerArrayRun(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte pos_repeated = 16;
            _repeated_idx = GetTokenUShort(token, pos_repeated);
            if (_repeated_idx == 0) throw new Exception("runtime 空数组引用");
            if (ir.IsConst(_repeated_idx)) _repeated_const = (int)ir.GetConstValue(_repeated_idx);
        }

        #region --Pack--

        //打包
        internal override ushort Pack(MemoryStream value_buff, MemoryStream pack, ref byte odd, ref byte odd_pos, SetValueInfo info, IPackRunExp ir)
        {
            info.PackBitPos = (int)pack.Position * 8 + odd_pos;
            if (!info.IsSetValue) SetAutoValue(value_buff, info, ir);
            int istart = info.StartPos;
            int bytelen = (BitCount % 8 == 0) ? (BitCount / 8) : (BitCount / 8 + 1);
            var buff = value_buff.GetBuffer();
            for (int i = 0; i < info.Count; i++)
            {
                CommitValue(buff, istart, BitCount, pack, ref odd, ref odd_pos);
                istart += bytelen;
            }
            return 0;
        }

        //取字段的字节大小
        internal override ushort GetBitLen(MemoryStream value_buff, ref int bitlen, SetValueInfo info, IPackRunExp ir)
        {
            if (_repeated_const > 0)
            {
                bitlen += BitCount * _repeated_const;
                return 0;
            }

            if (info.IsSetValue)
            {
                bitlen += BitCount * info.Count;
                return 0;
            }
            else if(_repeated_idx > 0)
            {
                bitlen += (int)ir.GetExpValue(_repeated_idx)*BitCount;
            }
            return 0;
        }

        #endregion

        #region --Unpack--

        //解包
        internal override ushort Unpack(byte[] buff, ref int pos_bit, int end_bit_pos, UnpackInfo info, IUnpackRunExp ir)
        {
            if (info.IsUnpack) return 0;
            info.IsUnpack = true;
            info.BitStart = pos_bit;
            int count = 0;
            if (TryGetRepeated(buff, ref count, ir))
            {
                info.BitLen = BitCount * count;
                pos_bit += info.BitLen;
                return 0;
            }
            else
                throw new Exception("unkonw");
        }

        //尝试取字段所需的位大小
        internal override bool TryGetNeedBitLen(byte[] buff, ref int bitlen, ref ushort nextseg, UnpackInfo info, IUnpackRunExp ir)
        {
            if(info.IsUnpack)
            {
                nextseg = 0;
                return true;
            }

            int count = 0;

            if (TryGetRepeated(buff, ref count, ir))
            {
                bitlen += (BitCount * count);
                return true;
            }
            else
                return false;

        }

        //尝试取重复次数
        private bool TryGetRepeated(byte[] buff, ref int count, IUnpackRunExp ir)
        {
            if (_repeated_const > 0)
            {
                count = _repeated_const;
                return true;
            }
            else
            {
                double dres = 0;
                if (ir.TryGetExpValue(buff, ref dres, _repeated_idx))
                {
                    count = (int)dres;
                    return true;
                }
                else
                    return false;
            }
        }

        #endregion

        #region --SetValue--

        private void SetAutoValue(MemoryStream value_buff, SetValueInfo info, IPackRunExp ir)
        {
            info.IsSetValue = true;
            if (_repeated_const > 0)
                info.Count = _repeated_const;
            else if (_repeated_idx > 0)
                info.Count = (int)ir.GetExpValue(_repeated_idx);


            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (ulong?)0);
            }
            info.BitLen = BitCount * info.Count;

        }


        internal override void SetSegmentValue(MemoryStream value_buff, bool?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for(int i=0; i<info.Count; i++)
            {
                SetSegmentValue(value_buff, (ulong)(value[i]==null?0:((bool)value[i]?1:0)));
            }

            if(_repeated_const> info.Count)
            {
                info.Count = _repeated_const;
                for (int i=0; i<_repeated_const-info.Count; i++)
                    SetSegmentValue(value_buff, (ulong?)0);
            }
            info.BitLen = BitCount * info.Count;

        }

        internal override void SetSegmentValue(MemoryStream value_buff, byte?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (ulong?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (ulong?)0);
            }
            info.BitLen = BitCount * info.Count;

        }

        internal override void SetSegmentValue(MemoryStream value_buff, sbyte?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (long?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (ulong?)0);
            }
            info.BitLen = BitCount * info.Count;

        }

        internal override void SetSegmentValue(MemoryStream value_buff, ushort?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (ulong?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (ulong?)0);
            }
            info.BitLen = BitCount * info.Count;

        }

        internal override void SetSegmentValue(MemoryStream value_buff, short?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (long?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (ulong?)0);
            }
            info.BitLen = BitCount * info.Count;

        }

        internal override void SetSegmentValue(MemoryStream value_buff, uint?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (ulong?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (ulong?)0);
            }
            info.BitLen = BitCount * info.Count;

        }

        internal override void SetSegmentValue(MemoryStream value_buff, int?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (long?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (long?)0);
            }
            info.BitLen = BitCount * info.Count;

        }

        internal override void SetSegmentValue(MemoryStream value_buff, ulong?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (ulong?)0);
            }
            info.BitLen = BitCount * info.Count;

        }

        internal override void SetSegmentValue(MemoryStream value_buff, long?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (ulong?)0);
            }
            info.BitLen = BitCount * info.Count;

        }

        internal override void SetSegmentValue(MemoryStream value_buff, float?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                if (IsSigned)
                    SetSegmentValue(value_buff, (long?)value[i]);
                else
                    SetSegmentValue(value_buff, (ulong?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (ulong?)0);
            }
            info.BitLen = BitCount * info.Count;

        }

        internal override void SetSegmentValue(MemoryStream value_buff, double?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                if (IsSigned)
                    SetSegmentValue(value_buff, (long?)value[i]);
                else
                    SetSegmentValue(value_buff, (ulong?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (ulong?)0);
            }
            info.BitLen = BitCount * info.Count;
        }

        #endregion

        #region --GetSegmentValue--

        internal override bool?[] GetBoolArray(byte[] buff, UnpackInfo info)
        {
            int count = info.BitLen / BitCount;
            var ret = new bool?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = UnpackToULong(buff, (uint)bitstart, BitCount, IsBigOrder) == 0 ? false : true;
                bitstart += BitCount;
            }
            return ret;
        }

        internal override byte?[] GetByteArray(byte[] buff, UnpackInfo info)
        {
            int count = info.BitLen / BitCount;
            var ret = new byte?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsSigned ? (byte)UnpackToLong(buff, (uint)bitstart, BitCount,Encoded, IsBigOrder) :(byte)UnpackToULong(buff, (uint)bitstart, BitCount, IsBigOrder);
                bitstart += BitCount;
            }
            return ret;
        }

        internal override double?[] GetDoubleArray(byte[] buff, UnpackInfo info)
        {
            int count = info.BitLen / BitCount;
            var ret = new double?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsSigned ? (double)UnpackToLong(buff, (uint)bitstart, BitCount, Encoded, IsBigOrder) : (double)UnpackToULong(buff, (uint)bitstart, BitCount, IsBigOrder);
                bitstart += BitCount;
            }
            return ret;
        }

        internal override float?[] GetFloatArray(byte[] buff, UnpackInfo info)
        {
            int count = info.BitLen / BitCount;
            var ret = new float?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsSigned ? (float)UnpackToLong(buff, (uint)bitstart, BitCount, Encoded, IsBigOrder) : (float)UnpackToULong(buff, (uint)bitstart, BitCount, IsBigOrder);
                bitstart += BitCount;
            }
            return ret;
        }

        internal override int?[] GetIntArray(byte[] buff, UnpackInfo info)
        {
            int count = info.BitLen / BitCount;
            var ret = new int?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsSigned ? (int)UnpackToLong(buff, (uint)bitstart, BitCount, Encoded, IsBigOrder) : (int)UnpackToULong(buff, (uint)bitstart, BitCount, IsBigOrder);
                bitstart += BitCount;
            }
            return ret;
        }
        
        internal override long?[] GetLongArray(byte[] buff, UnpackInfo info)
        {
            int count = info.BitLen / BitCount;
            var ret = new long?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsSigned ? (long)UnpackToLong(buff, (uint)bitstart, BitCount, Encoded, IsBigOrder) : (long)UnpackToULong(buff, (uint)bitstart, BitCount, IsBigOrder);
                bitstart += BitCount;
            }
            return ret;
        }
        
        internal override sbyte?[] GetSByteArray(byte[] buff, UnpackInfo info)
        {
            int count = info.BitLen / BitCount;
            var ret = new sbyte?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsSigned ? (sbyte)UnpackToLong(buff, (uint)bitstart, BitCount, Encoded, IsBigOrder) : (sbyte)UnpackToULong(buff, (uint)bitstart, BitCount, IsBigOrder);
                bitstart += BitCount;
            }
            return ret;
        }
        
        internal override short?[] GetShortArray(byte[] buff, UnpackInfo info)
        {
            int count = info.BitLen / BitCount;
            var ret = new short?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsSigned ? (short)UnpackToLong(buff, (uint)bitstart, BitCount, Encoded, IsBigOrder) : (short)UnpackToULong(buff, (uint)bitstart, BitCount, IsBigOrder);
                bitstart += BitCount;
            }
            return ret;
        }

        internal override uint?[] GetUIntArray(byte[] buff, UnpackInfo info)
        {
            int count = info.BitLen / BitCount;
            var ret = new uint?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsSigned ? (uint)UnpackToLong(buff, (uint)bitstart, BitCount, Encoded, IsBigOrder) : (uint)UnpackToULong(buff, (uint)bitstart, BitCount, IsBigOrder);
                bitstart += BitCount;
            }
            return ret;
        }

        internal override ulong?[] GetULongArray(byte[] buff, UnpackInfo info)
        {
            int count = info.BitLen / BitCount;
            var ret = new ulong?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsSigned ? (ulong)UnpackToLong(buff, (uint)bitstart, BitCount, Encoded, IsBigOrder) : (ulong)UnpackToULong(buff, (uint)bitstart, BitCount, IsBigOrder);
                bitstart += BitCount;
            }
            return ret;
        }

        internal override ushort?[] GetUShortArray(byte[] buff, UnpackInfo info)
        {
            int count = info.BitLen / BitCount;
            var ret = new ushort?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsSigned ? (ushort)UnpackToLong(buff, (uint)bitstart, BitCount, Encoded, IsBigOrder) : (ushort)UnpackToULong(buff, (uint)bitstart, BitCount, IsBigOrder);
                bitstart += BitCount;
            }
            return ret;
        }

        #endregion

    }
}
