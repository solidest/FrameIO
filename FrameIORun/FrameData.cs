using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;

namespace FrameIO.Run
{
    public class FrameData : ISegmentGettor
    {
        private Dictionary<string, InnerData> _data;

        private class InnerData
        {
            public ulong NumberValue { get;  set; }
            public ulong[] NumberArrayValue { get;  set; }
            public int BitSizeNumber { get;  set; }
        }

        public FrameData(Dictionary<string, SegRunUnpack> data)
        {
            _data = new Dictionary<string, InnerData>();
            foreach (var src in data)
            {
                var ind = new InnerData()
                {
                    NumberValue = src.Value.NumberValue,
                    BitSizeNumber = src.Value.BitSizeNumber
                };
                if(src.Value.NumberArrayValue!=null)
                {
                    var vs = src.Value.NumberArrayValue;
                    ind.NumberArrayValue = new ulong[vs.Length];
                    for (int i = 0; i < vs.Length; i++)
                        ind.NumberArrayValue[i] = vs[i];
                }
                _data.Add(src.Key, ind);
            }
        }

        public bool GetBool(string segmentname)
        {
            var seg = _data[segmentname];
            if (seg.NumberValue == 0)
                return false;
            else
                return true;
        }

        public byte GetByte(string segmentname)
        {
            return (byte)_data[segmentname].NumberValue;
        }

        public sbyte GetSByte(string segmentname)
        {
            var seg = _data[segmentname];
            return (sbyte)SegRun.ConvertToLong(seg.NumberValue, seg.BitSizeNumber);
        }

        public short GetShort(string segmentname)
        {
            var seg = _data[segmentname];
            return (short)SegRun.ConvertToLong(seg.NumberValue, seg.BitSizeNumber);
        }

        public ushort GetUShort(string segmentname)
        {
            return (ushort)_data[segmentname].NumberValue;
        }

        public int GetInt(string segmentname)
        {
            var seg = _data[segmentname];
            return (int)SegRun.ConvertToLong(seg.NumberValue, seg.BitSizeNumber);
        }

        public uint GetUInt(string segmentname)
        {
            return (uint)_data[segmentname].NumberValue;
        }

        public long GetLong(string segmentname)
        {
            var seg = _data[segmentname];
            return SegRun.ConvertToLong(seg.NumberValue, seg.BitSizeNumber);
        }
        public ulong GetULong(string segmentname)
        {
            return _data[segmentname].NumberValue;
        }


        public float GetFloat(string segmentname)
        {
            var seg = _data[segmentname];
            return BitConverter.ToSingle(BitConverter.GetBytes((uint)seg.NumberValue), 0);
        }

        public double GetDouble(string segmentname)
        {
            var seg = _data[segmentname];
            return BitConverter.ToDouble(BitConverter.GetBytes(seg.NumberValue), 0);
        }

        public bool[] GetBoolArray(string segmentname)
        {
            var vs = _data[segmentname].NumberArrayValue;
            if (vs == null) return null;
            var ret = new bool[vs.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                if (vs[i] == 0)
                    ret[i] = false;
                else
                    ret[i] = true;
            }
            return ret;
        }

        public sbyte[] GetSByteArray(string segmentname)
        {
            var seg = _data[segmentname];
            var vs = seg.NumberArrayValue;
            if (vs == null) return null;
            var ret = new sbyte[vs.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (sbyte)SegRun.ConvertToLong(vs[i], seg.BitSizeNumber);
            }
            return ret;
        }

        public byte[] GetByteArray(string segmentname)
        {
            var vs = _data[segmentname].NumberArrayValue;
            if (vs == null) return null;
            var ret = new byte[vs.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (byte)vs[i];
            }
            return ret;
        }
        public ushort[] GetUShortArray(string segmentname)
        {
            var vs = _data[segmentname].NumberArrayValue;
            if (vs == null) return null;
            var ret = new ushort[vs.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (ushort)vs[i];
            }
            return ret;
        }

        public short[] GetShortArray(string segmentname)
        {
            var seg = _data[segmentname];
            var vs = seg.NumberArrayValue;
            if (vs == null) return null;
            var ret = new short[vs.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (short)SegRun.ConvertToLong(vs[i], seg.BitSizeNumber);
            }
            return ret;
        }
        public int[] GetIntArray(string segmentname)
        {
            var seg = _data[segmentname];
            var vs = seg.NumberArrayValue;
            if (vs == null) return null;
            var ret = new int[vs.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (int)SegRun.ConvertToLong(vs[i], seg.BitSizeNumber);
            }
            return ret;
        }
        public uint[] GetUIntArray(string segmentname)
        {
            var vs = _data[segmentname].NumberArrayValue;
            if (vs == null) return null;
            var ret = new uint[vs.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (uint)vs[i];
            }
            return ret;
        }

        public long[] GetLongArray(string segmentname)
        {
            var seg = _data[segmentname];
            var vs = seg.NumberArrayValue;
            if (vs == null) return null;
            var ret = new long[vs.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = SegRun.ConvertToLong(vs[i], seg.BitSizeNumber);
            }
            return ret;
        }

        public ulong[] GetULongArray(string segmentname)
        {
            var vs = _data[segmentname].NumberArrayValue;
            if (vs == null) return null;
            var ret = new ulong[vs.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = vs[i];
            }
            return ret;
        }


        public float[] GetFloatArray(string segmentname)
        {
            var vs = _data[segmentname].NumberArrayValue;
            if (vs == null) return null;
            var ret = new float[vs.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = BitConverter.ToSingle(BitConverter.GetBytes((uint)vs[i]), 0);
            }
            return ret;
        }

        public double[] GetDoubleArray(string segmentname)
        {
            var vs = _data[segmentname].NumberArrayValue;
            if (vs == null) return null;
            var ret = new double[vs.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = BitConverter.ToDouble(BitConverter.GetBytes(vs[i]), 0);
            }
            return ret;
        }
    }
}
