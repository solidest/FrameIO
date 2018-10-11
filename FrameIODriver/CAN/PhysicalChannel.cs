using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameIO.Driver
{
    public abstract partial class PhysicalChannel
    {
        public  System.Boolean DoWrite(Byte[] data)
        {
            var ExternFlag = (System.Byte)((data[0] & 128) >> 7);
            if (ExternFlag == 0) return WriteStandardFreame(data);
            if (ExternFlag == 1) return WriteExternFreame(data);
            return true;
        }
        private Boolean WriteExternFreame(Byte[] data)
        {
            if (data.Length != 13) return false;
            var DataLen = (System.Byte)(data[0] & 15);
            var RemoteFlag = ((data[0] & 64) >> 6) == 0 ? false : true;
            var id = BitConverter.ToUInt32(data.Skip(1).Take(4).ToArray(), 0);
            return DoWriteExternFrame(DataLen, RemoteFlag, id, data.Skip(5).ToArray());
        }
        private Boolean WriteStandardFreame(Byte[] data)
        {
            if (data.Length != 11) return false;
            var DataLen = (System.Byte)(data[0] & 15);
            var RemoteFlag = ((data[0] & 64) >> 6) == 0 ? false : true;
            var id = BitConverter.ToUInt16(data.Skip(1).Take(2).ToArray(), 0);
            return DoWriteStandardFrame(DataLen, RemoteFlag, id, data.Skip(3).ToArray());
        }
        protected abstract Boolean DoWriteExternFrame(Byte DataLen, Boolean RemoteFlag, UInt32 id, Byte[] userdata);
        protected abstract Boolean DoWriteStandardFrame(Byte DataLen, Boolean RemoteFlag, UInt16 id, Byte[] userdata);
    }
}
