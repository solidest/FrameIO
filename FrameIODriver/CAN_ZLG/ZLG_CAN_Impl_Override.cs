using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameIO.Driver
{
    partial class CAN_ZLG_Impl
    {
        protected override bool DoWriteExternFrame(byte DataLen, bool RemoteFlag, uint id, byte[] userdata)
        {
            VCI_CAN_OBJ sendbuf = Create_VCI_CAN_OBJ();
            sendbuf.DataLen = DataLen;
            sendbuf.RemoteFlag = Convert.ToByte(RemoteFlag);
            sendbuf.ExternFlag = 1;
            sendbuf.ID = id;
            unsafe
            {
                for (System.Int32 i = 0; i < 8; ++i)
                    sendbuf.Data[i] = userdata[i];
            }
            UInt32 flag = Api.VCI_Transmit(m_devtype, m_devind, m_canind, ref sendbuf, 1);
            if (flag == 0)
            {
                Console.WriteLine("设备不存在或掉线");
                return false;
            }
            if (flag != 1) return false;
            return true;
        }

        protected override bool DoWriteStandardFrame(byte DataLen, bool RemoteFlag, ushort id, byte[] userdata)
        {
            VCI_CAN_OBJ sendbuf = Create_VCI_CAN_OBJ();
            sendbuf.DataLen = DataLen;
            sendbuf.RemoteFlag = Convert.ToByte(RemoteFlag);
            sendbuf.ExternFlag = 0;
            sendbuf.ID = id;
            unsafe
            {
                //sendbuf.Data = userdata;
                for (System.Int32 i = 0; i < 8; ++i)
                    sendbuf.Data[i] = userdata[i];
            }
            UInt32 flag = Api.VCI_Transmit(m_devtype, m_devind, m_canind, ref sendbuf, 1);
            if (flag == 0)
            {
                Console.WriteLine("设备不存在或掉线");
                return false;
            }
            if (flag != 1) return false;
            return true;
        }
    }
}
