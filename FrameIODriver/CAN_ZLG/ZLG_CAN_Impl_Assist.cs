using System;
using System.Collections.Generic;
using FrameIO.Interface;

namespace FrameIO.Driver
{
    partial class CAN_ZLG_Impl
    {

        private UInt32 m_bOpen = 0;
        private UInt32 m_devind = 0;
        private UInt32 m_canind = 0;
        private UInt32 m_devtype = 4;
        private int m_waittime = 0;

        private UInt32 AccCode = 0;
        private UInt32 AccMask = 0;
        private UInt32 Baudrate = 500_000;

        private Byte Timing0 = 0;
        private Byte Timing1 = 0;
        private Byte Filter = 0;
        private Byte Mode = 0;

        private Byte SendType = 0;
        private Byte RemoteFlag = 0;
        private Byte ExternFlag = 0;




        VCI_CAN_OBJ[] m_recobj = new VCI_CAN_OBJ[50];

        #region 辅助函数 CAN_RX_MSG_ToBytes
        private static Byte[] CAN_RX_MSG_ToBytes(VCI_CAN_OBJ obj)
        {
            var data = (Byte[])null;

            if (obj.ExternFlag == 0)
            {
                data = new byte[11];
                data[0] = obj.DataLen;
                data[0] |= (byte)(0);
                data[0] |= (byte)(obj.RemoteFlag << 7);
                data[1] = (byte)(obj.ID);
                data[2] |= (byte)(obj.ID >> 8);
                unsafe
                {
                    for (System.Int32 j = 0; j < 8; j++)
                        data[j + 3] = obj.Data[j];
                }
                return data;
            }
            if (obj.ExternFlag == 1)
            {
                data = new byte[13];
                data[0] = obj.DataLen;
                data[0] |= (byte)(0);
                data[0] |= (byte)(obj.RemoteFlag << 7);
                data[1] = (byte)(obj.ID);
                data[2] = (byte)(obj.ID >> 8);
                data[3] = (byte)(obj.ID >> 16);
                data[4] |= (byte)(obj.ID >> 24);
                unsafe
                {
                    for (System.Int32 j = 0; j < 8; j++)
                        data[j + 5] = obj.Data[j];
                }
                return data;
            }
            if (obj.RemoteFlag == 1)
            {
                data = new byte[3];
                data[0] = obj.DataLen;
                data[0] |= (byte)(1);
                data[0] |= (byte)(obj.RemoteFlag << 7);
                data[1] = (byte)(obj.ID);
                data[2] |= (byte)(obj.ID >> 8);

                return data;
            }
            return new Byte[0];
        }

        #endregion
        
        #region 辅助函数 Init_Config()
        private  void InitConfig(Dictionary<string, object> config)
        {
            m_devtype = (uint)config["DevType"];
            m_devind = (UInt32)config["DevInd"];
            m_canind = (UInt32)config["ChannelInd"];
            m_waittime = (int)config["WaitTime"];

            AccCode = System.Convert.ToUInt32("0x" + config["AccCode"], 16);
            AccMask = System.Convert.ToUInt32("0x" + config["AccMask"], 16);
            
            Baudrate = System.Convert.ToUInt32(config["Baudrate"]);
            CANBaudrate canBaudrate = new CANBaudrate(Baudrate);
            Timing0 = System.Convert.ToByte(""+canBaudrate.BTR0,16);
            Timing1 = System.Convert.ToByte("" + canBaudrate.BTR1, 16);

            Filter = (Byte)config["Filter"];
            Mode = (Byte)config["Mode"];

//             SendType= (byte)config["SendType"];
//             RemoteFlag=(byte)config["RemoteFlag"];
//             ExternFlag= (byte)config["ExternFlag"];
        }
        #endregion
       
        #region 初始化CAN对象
        private static VCI_CAN_OBJ Create_VCI_CAN_OBJ()
        {
            unsafe
            {
                var sendbuf = new VCI_CAN_OBJ();
                sendbuf.ID = 0;

                return sendbuf;
            }
        }
        #endregion
    }
}
