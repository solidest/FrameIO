using FrameIO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameIO.Driver
{
    partial class YH_CAN_Impl
    {
        private string PortName = string.Empty;
        private UInt16 ReadTimeOut = 1;
        private UInt16 WriteTimeOut = 3000;
        private UInt16 WorkMode = 1;
        private UInt16 BaudRate;

        private static Byte[] PVCI_CAN_OBJ_ToBytes(canmsg_t obj)
        {
            Byte[] data;
            Int32 externFlag = (obj.flags & AdvCan.MSG_EXT) > 0 ? 1 : 0;
            Int32 rerFlag = (obj.flags & AdvCan.MSG_RTR) > 0 ? 1 : 0;
            if (externFlag <= 0)
            {
                data = new Byte[11];
                data[0] = (System.Byte)obj.length;
                data[0] |= (System.Byte)(rerFlag << 6);
                data[0] |= (System.Byte)(externFlag << 7);
                Byte[] id = BitConverter.GetBytes((UInt16)obj.id);
                data[1] = id[0];
                data[2] |= id[1];
                for (System.Int32 j = 0; j < 8; j++)
                    data[j + 3] = obj.data[j];
            }
            else
            {
                data = new Byte[13];
                data[0] = (System.Byte)obj.length;
                data[0] |= (System.Byte)(rerFlag << 6);
                data[0] |= (System.Byte)(externFlag << 7);
                Byte[] id = BitConverter.GetBytes(obj.id);
                data[1] = id[0];
                data[2] = id[1];
                data[3] = id[2];
                data[4] |= id[3];
                for (System.Int32 j = 0; j < 8; j++)
                    data[j + 5] = obj.data[j];
            }
            return data;
        }

        #region 辅助函数 Init_Config()
        public void InitConfig(Dictionary<string, object> config)
        {
            PortName = "can"+config["channelind"].ToString();
            BaudRate = (UInt16)BaudRateTypeConverter.ConvertFrom(config["baudrate"]);
            WriteTimeOut = System.Convert.ToUInt16(config["writetimeout"]);
        }
        #endregion
        //设备号
        private AdvCANIO m_DevCan = null;
        [System.ComponentModel.Browsable(false)]
        private AdvCANIO DevCan
        {
            get
            {
                if (m_DevCan == null)
                    m_DevCan = new AdvCANIO();
                return m_DevCan;
            }
        }
        private Boolean OpenCan()
        {
            Int32 nRet = DevCan.acCanOpen(PortName, true, 500, 500);
            if (nRet >= 0) return true;
            Console.WriteLine("打开研华_CAN 失败, 请检查端口名称是否正确!");
            return false;
        }
        private Boolean EnterResetMode()
        {
            var nRet = DevCan.acEnterResetMode();
            if (nRet >= 0) return true;
            Console.WriteLine("研华_CAN 复位失败!");
            DevCan.acCanClose();
            return false;
        }
        private Boolean SetTimeOut()
        {
            var nRet = DevCan.acSetTimeOut(ReadTimeOut, WriteTimeOut);
            if (nRet >= 0) return true;
            Console.WriteLine("研华_CAN 收发超时设置失败!");
            DevCan.acCanClose();
            return false;
        }
        private Boolean SetSelfReception()
        {
            var nRet = DevCan.acSetSelfReception(WorkMode == 1);
            if (nRet >= 0) return true;
            Console.WriteLine("研华_CAN 工作模式设置失败!");
            DevCan.acCanClose();
            return false;
        }

        private Boolean EnterWorkMode()
        {
            var nRet = DevCan.acEnterWorkMode();
            if (nRet >= 0) return true;
            Console.WriteLine("研华_CAN 进入工作状态设置失败!");
            DevCan.acCanClose();
            return false;
        }
    }
}
