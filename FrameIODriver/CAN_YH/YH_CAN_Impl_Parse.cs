using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameIO.Driver
{
    partial class YH_CAN_Impl
    {
        private canmsg_t ParseFrame(byte[] data)
        {
            var ExternFlag = (System.Byte)((data[0] & 128) >> 7);
            if (ExternFlag == 0) return ParseStandFrame(data);
            if (ExternFlag == 1) return ParseExtendFrame(data);
            return new canmsg_t();
        }
        private canmsg_t ParseStandFrame(Byte[] data)
        {
            if (data.Length != 11) throw new Exception("数据格式错误!");
            var DataLen = (System.Byte)(data[0] & 15);
            var RemoteFlag = ((data[0] & 64) >> 6) == 0 ? false : true;
            var id = BitConverter.ToUInt16(data.Skip(1).Take(2).ToArray(), 0);

            canmsg_t sendbuf = InitStandeFrameFlags(DataLen, RemoteFlag, id, data.Skip(3).ToArray());
            return sendbuf;
        }
        private canmsg_t ParseExtendFrame(Byte[] data)
        {
            if (data.Length != 13) throw new Exception("数据格式错误!");
            var DataLen = (System.Byte)(data[0] & 15);
            var RemoteFlag = ((data[0] & 64) >> 6) == 0 ? false : true;
            var id = BitConverter.ToUInt32(data.Skip(1).Take(4).ToArray(), 0);

            canmsg_t sendbuf = InitExtendeFrameFlags(DataLen, RemoteFlag, id, data.Skip(5).ToArray());
            return sendbuf;
        }
    }
}
