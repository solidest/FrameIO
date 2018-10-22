using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;

namespace FrameIO.Runtime
{
    internal class SegmentGettor : ISegmentGettor
    {
        private static FrameRuntime _fi;
        private FrameUnpackerInfo _segupi;
        private byte[] _data;

        static SegmentGettor()
        {
            _fi = FrameRuntime.Run;
        }
        internal SegmentGettor(byte[] data, FrameUnpackerInfo upi)
        {
            _segupi = upi;
            _data = data;
        }

        public ISegmentGettor GetSubFrame(ushort idx)
        {
            return new SegmentGettor(_data, ((FrameUnpacker)_segupi[idx].Tag).Info);
        }

        #region --GetValue--

        public bool? GetBool(ushort segidx)
        {
            return _fi[segidx].GetBool(_data, _segupi[segidx]);
        }

        public bool?[] GetBoolArray(ushort segidx)
        {
            return _fi[segidx].GetBoolArray(_data, _segupi[segidx]);
        }

        public byte? GetByte(ushort segidx)
        {
            return _fi[segidx].GetByte(_data, _segupi[segidx]);
        }

        public byte?[] GetByteArray(ushort segidx)
        {
            return _fi[segidx].GetByteArray(_data, _segupi[segidx]);
        }

        public double? GetDouble(ushort segidx)
        {
            return _fi[segidx].GetDouble(_data, _segupi[segidx]);
        }

        public double?[] GetDoubleArray(ushort segidx)
        {
            return _fi[segidx].GetDoubleArray(_data, _segupi[segidx]);
        }

        public float? GetFloat(ushort segidx)
        {
            return _fi[segidx].GetFloat(_data, _segupi[segidx]);
        }

        public float?[] GetFloatArray(ushort segidx)
        {
            return _fi[segidx].GetFloatArray(_data, _segupi[segidx]);
        }

        public int? GetInt(ushort segidx)
        {
            return _fi[segidx].GetInt(_data, _segupi[segidx]);
        }

        public int?[] GetIntArray(ushort segidx)
        {
            return _fi[segidx].GetIntArray(_data, _segupi[segidx]);
        }

        public long? GetLong(ushort segidx)
        {
            return _fi[segidx].GetLong(_data, _segupi[segidx]);
        }

        public long?[] GetLongArray(ushort segidx)
        {
            return _fi[segidx].GetLongArray(_data, _segupi[segidx]);
        }

        public sbyte? GetSByte(ushort segidx)
        {
            return _fi[segidx].GetSByte(_data, _segupi[segidx]);
        }

        public sbyte?[] GetSByteArray(ushort segidx)
        {
            return _fi[segidx].GetSByteArray(_data, _segupi[segidx]);
        }

        public short? GetShort(ushort segidx)
        {
            return _fi[segidx].GetShort(_data, _segupi[segidx]);
        }

        public short?[] GetShortArray(ushort segidx)
        {
            return _fi[segidx].GetShortArray(_data, _segupi[segidx]);
        }

        public uint? GetUInt(ushort segidx)
        {
            return _fi[segidx].GetUInt(_data, _segupi[segidx]);
        }

        public uint?[] GetUIntArray(ushort segidx)
        {
            return _fi[segidx].GetUIntArray(_data, _segupi[segidx]);
        }

        public ulong? GetULong(ushort segidx)
        {
            return _fi[segidx].GetULong(_data, _segupi[segidx]);
        }

        public ulong?[] GetULongArray(ushort segidx)
        {
            return _fi[segidx].GetULongArray(_data, _segupi[segidx]);
        }

        public ushort? GetUShort(ushort segidx)
        {
            return _fi[segidx].GetUShort(_data, _segupi[segidx]);
        }

        public ushort?[] GetUShortArray(ushort segidx)
        {
            return _fi[segidx].GetUShortArray(_data, _segupi[segidx]);
        }

        #endregion

    }
}
