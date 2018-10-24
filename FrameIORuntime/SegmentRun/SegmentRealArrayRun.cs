using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    internal class SegmentRealArrayRun : SegmentRealRun
    {
        private ushort _repeated_idx;
        private int _repeated_const = -1;

        internal SegmentRealArrayRun(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte pos_repeated = 16;
            _repeated_idx = GetTokenUShort(token, pos_repeated);
            if (_repeated_idx == 0) throw new Exception("runtime");
            if (ir.IsConst(_repeated_idx)) _repeated_const = (int)ir.GetConstValue(_repeated_idx);

        }

        #region --Unpack--


        internal override ushort Unpack(byte[] buff, ref int pos_bit, int end_bit_pos, UnpackInfo info, IUnpackRunExp ir)
        {
            if (info.IsUnpack) return 0;
            info.IsUnpack = true;
            info.BitStart = pos_bit;
            int count = 0;
            if (TryGetRepeated(buff, ref count, ir))
            {
                info.BitLen = count *(IsDouble?64:32);
                pos_bit += info.BitLen;
                return 0;
            }
            else
                throw new Exception("unkonw");


        }

        //尝试取字段的位所需大小
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
                bitlen += ((IsDouble ? 64 : 32) * count);
                nextseg = 0;
                return true;
            }
            else
                return false;
        }

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

        #region --Pack--

        internal override ushort Pack(MemoryStream value_buff, MemoryStream pack, ref byte odd, ref byte odd_pos, SetValueInfo info, IPackRunExp ir)
        {
            if (!info.IsSetValue) SetAutoValue(value_buff, info, ir);

            int istart = info.StartPos;
            var buff = value_buff.GetBuffer();
            for (int i = 0; i < info.Count; i++)
            {
                CommitValue(buff, istart, IsDouble ? 64 : 32, pack, ref odd, ref odd_pos);
                istart += IsDouble ? 8 : 4;
            }
            return 0;

        }


        //取字段的字节大小
        internal override ushort GetBitLen(MemoryStream value_buff, ref int bitlen, SetValueInfo info, IPackRunExp ir)
        {
            if (_repeated_const > 0)
            { 
                bitlen += (IsDouble ? 64 : 32) * _repeated_const;
                return 0;
            }
            if (info.IsSetValue) 
            {
                bitlen += (IsDouble? 64 : 32) * info.Count;
                return 0;
            }
            return 0;
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
                SetSegmentValue(value_buff, (float?)0);
            }
        }


        internal override void SetSegmentValue(MemoryStream value_buff, bool?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float)(value[i] == null ? 0 : ((bool)value[i] ? 1 : 0)));
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (float?)0);
            }
        }

        internal override void SetSegmentValue(MemoryStream value_buff, byte?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (float?)0);
            }
        }

        internal override void SetSegmentValue(MemoryStream value_buff, sbyte?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (float?)0);
            }
        }

        internal override void SetSegmentValue(MemoryStream value_buff, ushort?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (float?)0);
            }
        }

        internal override void SetSegmentValue(MemoryStream value_buff, short?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (float?)0);
            }
        }

        internal override void SetSegmentValue(MemoryStream value_buff, uint?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (float?)0);
            }
        }

        internal override void SetSegmentValue(MemoryStream value_buff, int?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (float?)0);
            }
        }

        internal override void SetSegmentValue(MemoryStream value_buff, ulong?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (double?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (double?)0);
            }
        }

        internal override void SetSegmentValue(MemoryStream value_buff, long?[] value, SetValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = (int)value_buff.Position;
            info.Count = value == null ? 0 : value.Length;
            if (_repeated_const > 0 && _repeated_const < info.Count) info.Count = _repeated_const;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (double?)value[i]);
            }

            if (_repeated_const > info.Count)
            {
                info.Count = _repeated_const;
                for (int i = 0; i < _repeated_const - info.Count; i++)
                    SetSegmentValue(value_buff, (double?)0);
            }
        }

        internal override void SetSegmentValue(MemoryStream value_buff, float?[] value, SetValueInfo info)
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
                    SetSegmentValue(value_buff, (float?)0);
            }
        }

        internal override void SetSegmentValue(MemoryStream value_buff, double?[] value, SetValueInfo info)
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
                    SetSegmentValue(value_buff, (double?)0);
            }
        }

        #endregion

        #region --GetSegmentValue--

        internal override bool?[] GetBoolArray(byte[] buff, UnpackInfo info)
        {
            var bitcount = (IsDouble ? 64 : 32);
            int count = info.BitLen / bitcount;
            var ret = new bool?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = 0!=(IsDouble ? UnpackToDouble(buff, (uint)bitstart, Encoded, IsBigOrder) : (double)UnpackToFloat(buff, (uint)bitstart, Encoded, IsBigOrder));
                bitstart += bitcount;
            }
            return ret;
        }

        internal override byte?[] GetByteArray(byte[] buff, UnpackInfo info)
        {
            var bitcount = (IsDouble ? 64 : 32);
            int count = info.BitLen / bitcount;
            var ret = new byte?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsDouble ? (byte)UnpackToDouble(buff, (uint)bitstart, Encoded, IsBigOrder) : (byte)UnpackToFloat(buff, (uint)bitstart, Encoded, IsBigOrder);
                bitstart += bitcount;
            }
            return ret;
        }

        internal override double?[] GetDoubleArray(byte[] buff, UnpackInfo info)
        {
            var bitcount = (IsDouble ? 64 : 32);
            int count = info.BitLen / bitcount;
            var ret = new double?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsDouble ? UnpackToDouble(buff, (uint)bitstart,  Encoded, IsBigOrder): (double)UnpackToFloat(buff, (uint)bitstart, Encoded, IsBigOrder);
                bitstart += bitcount;
            }
            return ret;
        }

        internal override float?[] GetFloatArray(byte[] buff, UnpackInfo info)
        {
            var bitcount = (IsDouble ? 64 : 32);
            int count = info.BitLen / bitcount;
            var ret = new float?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsDouble ? (float)UnpackToDouble(buff, (uint)bitstart, Encoded, IsBigOrder) : UnpackToFloat(buff, (uint)bitstart, Encoded, IsBigOrder);
                bitstart += bitcount;
            }
            return ret;
        }

        internal override int?[] GetIntArray(byte[] buff, UnpackInfo info)
        {
            var bitcount = (IsDouble ? 64 : 32);
            int count = info.BitLen / bitcount;
            var ret = new int?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsDouble ? (int)UnpackToDouble(buff, (uint)bitstart, Encoded, IsBigOrder) : (int)UnpackToFloat(buff, (uint)bitstart, Encoded, IsBigOrder);
                bitstart += bitcount;
            }
            return ret;
        }

        internal override long?[] GetLongArray(byte[] buff, UnpackInfo info)
        {
            var bitcount = (IsDouble ? 64 : 32);
            int count = info.BitLen / bitcount;
            var ret = new long?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsDouble ? (long)UnpackToDouble(buff, (uint)bitstart, Encoded, IsBigOrder) : (long)UnpackToFloat(buff, (uint)bitstart, Encoded, IsBigOrder);
                bitstart += bitcount;
            }
            return ret;
        }

        internal override sbyte?[] GetSByteArray(byte[] buff, UnpackInfo info)
        {
            var bitcount = (IsDouble ? 64 : 32);
            int count = info.BitLen / bitcount;
            var ret = new sbyte?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsDouble ? (sbyte)UnpackToDouble(buff, (uint)bitstart, Encoded, IsBigOrder) : (sbyte)UnpackToFloat(buff, (uint)bitstart, Encoded, IsBigOrder);
                bitstart += bitcount;
            }
            return ret;
        }

        internal override short?[] GetShortArray(byte[] buff, UnpackInfo info)
        {
            var bitcount = (IsDouble ? 64 : 32);
            int count = info.BitLen / bitcount;
            var ret = new short?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsDouble ? (short)UnpackToDouble(buff, (uint)bitstart, Encoded, IsBigOrder) : (short)UnpackToFloat(buff, (uint)bitstart, Encoded, IsBigOrder);
                bitstart += bitcount;
            }
            return ret;
        }

        internal override uint?[] GetUIntArray(byte[] buff, UnpackInfo info)
        {
            var bitcount = (IsDouble ? 64 : 32);
            int count = info.BitLen / bitcount;
            var ret = new uint?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsDouble ? (uint)UnpackToDouble(buff, (uint)bitstart, Encoded, IsBigOrder) : (uint)UnpackToFloat(buff, (uint)bitstart, Encoded, IsBigOrder);
                bitstart += bitcount;
            }
            return ret;
        }

        internal override ulong?[] GetULongArray(byte[] buff, UnpackInfo info)
        {
            var bitcount = (IsDouble ? 64 : 32);
            int count = info.BitLen / bitcount;
            var ret = new ulong?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsDouble ? (ulong)UnpackToDouble(buff, (uint)bitstart, Encoded, IsBigOrder) : (ulong)UnpackToFloat(buff, (uint)bitstart, Encoded, IsBigOrder);
                bitstart += bitcount;
            }
            return ret;
        }

        internal override ushort?[] GetUShortArray(byte[] buff, UnpackInfo info)
        {
            var bitcount = (IsDouble ? 64 : 32);
            int count = info.BitLen / bitcount;
            var ret = new ushort?[count];
            var bitstart = info.BitStart;
            for (int i = 0; i < count; i++)
            {
                ret[i] = IsDouble ? (ushort)UnpackToDouble(buff, (uint)bitstart, Encoded, IsBigOrder) : (ushort)UnpackToFloat(buff, (uint)bitstart, Encoded, IsBigOrder);
                bitstart += bitcount;
            }
            return ret;
        }

        #endregion

    }
}
