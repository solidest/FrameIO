using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameIO.Driver
{
    partial class CAN_ZLG_Impl
    {
        private VCI_CAN_OBJ ParseFrame(byte[] data)
        {
            var ExternFlag = (System.Byte)((data[0] & 128) >> 7);
            if (ExternFlag == 0) return ParseStandFrame(data);
            if (ExternFlag == 1) return ParseExtendFrame(data);
            return new VCI_CAN_OBJ(); ;
        }
        private VCI_CAN_OBJ ParseStandFrame(Byte[] data)
        {
            VCI_CAN_OBJ sendbuf = new VCI_CAN_OBJ();

            if (data.Length != 11) throw new Exception("数据格式不正确!");
            var DataLen = (System.Byte)(data[0] & 15);
            var RemoteFlag = ((data[0] & 64) >> 6) == 0 ? false : true;
            var id = BitConverter.ToUInt16(data.Skip(1).Take(2).ToArray(), 0);

            sendbuf.DataLen = DataLen;
            sendbuf.RemoteFlag = Convert.ToByte(RemoteFlag);
            sendbuf.ExternFlag = 0;
            sendbuf.ID = id;

            unsafe
            {
                for (System.Int32 i = 0; i < 8; ++i)
                    sendbuf.Data[i] = data.Skip(3).ToArray()[i];
            }

            return sendbuf;
        }
        private VCI_CAN_OBJ ParseExtendFrame(Byte[] data)
        {
            VCI_CAN_OBJ sendbuf = new VCI_CAN_OBJ();
            if (data.Length != 13) throw new Exception("数据格式不正确!");
            var DataLen = (System.Byte)(data[0] & 15);
            var RemoteFlag = ((data[0] & 64) >> 6) == 0 ? false : true;
            var id = BitConverter.ToUInt32(data.Skip(1).Take(4).ToArray(), 0);

            unsafe
            {
                for (System.Int32 i = 0; i < 8; ++i)
                    sendbuf.Data[i] = data.Skip(5).ToArray()[i];
            }

            return sendbuf;
        }
    }
}
