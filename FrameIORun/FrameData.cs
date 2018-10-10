using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;

namespace FrameIO.Run
{
    public class FrameData : IFrameData
    {
        private Dictionary<string, SegRunUnpack> _data;
        public FrameData(Dictionary<string, SegRunUnpack> data)
        {
            _data = data;
        }

        public bool GetBool(string segmentname)
        {
            throw new NotImplementedException();
        }

        public bool[] GetBoolArray(string segmentname)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(string segmentname)
        {
            throw new NotImplementedException();
        }

        public byte[] GetByteArray(string segmentname)
        {
            throw new NotImplementedException();
        }

        public int GetInt(string segmentname)
        {
            throw new NotImplementedException();
        }

        public int[] GetIntArray(string segmentname)
        {
            throw new NotImplementedException();
        }

        public long GetLong(string segmentname)
        {
            throw new NotImplementedException();
        }

        public long[] GetLongArray(string segmentname)
        {
            throw new NotImplementedException();
        }

        public sbyte GetSByte(string segmentname)
        {
            throw new NotImplementedException();
        }

        public sbyte[] GetSByteArray(string segmentname)
        {
            throw new NotImplementedException();
        }

        public short GetShort(string segmentname)
        {
            throw new NotImplementedException();
        }

        public short[] GetShortArray(string segmentname)
        {
            throw new NotImplementedException();
        }

        public uint GetUInt(string segmentname)
        {
            throw new NotImplementedException();
        }

        public uint[] GetUIntArray(string segmentname)
        {
            throw new NotImplementedException();
        }

        public ulong GetULong(string segmentname)
        {
            throw new NotImplementedException();
        }

        public ulong[] GetULongArray(string segmentname)
        {
            throw new NotImplementedException();
        }

        public ushort GetUShort(string segmentname)
        {
            throw new NotImplementedException();
        }

        public ushort[] GetUShortArray(string segmentname)
        {
            throw new NotImplementedException();
        }
    }
}
