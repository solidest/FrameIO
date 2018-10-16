using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;

namespace FrameIO.Runtime
{
    public class SegmentGettor : ISegmentGettor
    {
        private FrameInfo _fi;
        private SegmentUnpackInfo[] _segi;
        private byte[] _data;

        public SegmentGettor(FrameInfo fi,byte[] data, SegmentUnpackInfo[] segi)
        {
            _fi = fi;
            _segi = segi;
            _data = data;
        }

        #region --GetValue--

        public bool? GetBool(ushort segidx)
        {
            return _fi[segidx].GetBool(_data, _segi[segidx]);
        }

        public bool?[] GetBoolArray(ushort segidx)
        {
            return _fi[segidx].GetBoolArray(_data, _segi[segidx]);
        }

        public byte? GetByte(ushort segidx)
        {
            return _fi[segidx].GetByte(_data, _segi[segidx]);
        }

        public byte?[] GetByteArray(ushort segidx)
        {
            return _fi[segidx].GetByteArray(_data, _segi[segidx]);
        }

        public double? GetDouble(ushort segidx)
        {
            return _fi[segidx].GetDouble(_data, _segi[segidx]);
        }

        public double?[] GetDoubleArray(ushort segidx)
        {
            return _fi[segidx].GetDoubleArray(_data, _segi[segidx]);
        }

        public float? GetFloat(ushort segidx)
        {
            return _fi[segidx].GetFloat(_data, _segi[segidx]);
        }

        public float?[] GetFloatArray(ushort segidx)
        {
            return _fi[segidx].GetFloatArray(_data, _segi[segidx]);
        }

        public int? GetInt(ushort segidx)
        {
            return _fi[segidx].GetInt(_data, _segi[segidx]);
        }

        public int?[] GetIntArray(ushort segidx)
        {
            return _fi[segidx].GetIntArray(_data, _segi[segidx]);
        }

        public long? GetLong(ushort segidx)
        {
            return _fi[segidx].GetLong(_data, _segi[segidx]);
        }

        public long?[] GetLongArray(ushort segidx)
        {
            return _fi[segidx].GetLongArray(_data, _segi[segidx]);
        }

        public sbyte? GetSByte(ushort segidx)
        {
            return _fi[segidx].GetSByte(_data, _segi[segidx]);
        }

        public sbyte?[] GetSByteArray(ushort segidx)
        {
            return _fi[segidx].GetSByteArray(_data, _segi[segidx]);
        }

        public short? GetShort(ushort segidx)
        {
            return _fi[segidx].GetShort(_data, _segi[segidx]);
        }

        public short?[] GetShortArray(ushort segidx)
        {
            return _fi[segidx].GetShortArray(_data, _segi[segidx]);
        }

        public uint? GetUInt(ushort segidx)
        {
            return _fi[segidx].GetUInt(_data, _segi[segidx]);
        }

        public uint?[] GetUIntArray(ushort segidx)
        {
            return _fi[segidx].GetUIntArray(_data, _segi[segidx]);
        }

        public ulong? GetULong(ushort segidx)
        {
            return _fi[segidx].GetULong(_data, _segi[segidx]);
        }

        public ulong?[] GetULongArray(ushort segidx)
        {
            return _fi[segidx].GetULongArray(_data, _segi[segidx]);
        }

        public ushort? GetUShort(ushort segidx)
        {
            return _fi[segidx].GetUShort(_data, _segi[segidx]);
        }

        public ushort?[] GetUShortArray(ushort segidx)
        {
            return _fi[segidx].GetUShortArray(_data, _segi[segidx]);
        }

        #endregion

    }
}
