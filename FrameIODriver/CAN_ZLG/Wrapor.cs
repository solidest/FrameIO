using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIO.Driver
{
    public static class Wrapor
    {
        public static UInt32 VCI_OpenDevice(UInt32 DeviceType, UInt32 DeviceInd, UInt32 Reserved)
        {
            try
            {
                return Api.VCI_OpenDevice(DeviceType, DeviceInd, Reserved);

            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public static UInt32 VCI_CloseDevice(UInt32 DeviceType, UInt32 DeviceInd)
        {
            try
            {
                return Api.VCI_CloseDevice(DeviceType, DeviceInd);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  UInt32 VCI_InitCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_INIT_CONFIG pInitConfig)
        {
            try
            {
                return Api.VCI_InitCAN(DeviceType, DeviceInd, CANInd, ref pInitConfig);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  UInt32 VCI_ReadBoardInfo(UInt32 DeviceType, UInt32 DeviceInd, ref VCI_BOARD_INFO pInfo)
        {
            try
            {
                return Api.VCI_ReadBoardInfo(DeviceType, DeviceInd, ref pInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  UInt32 VCI_ReadErrInfo(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_ERR_INFO pErrInfo)
        {
            try
            {
                return Api.VCI_ReadErrInfo(DeviceType, DeviceInd, CANInd,ref pErrInfo);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  UInt32 VCI_ReadCANStatus(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_STATUS pCANStatus)
        {
            try
            {
                return Api.VCI_ReadCANStatus(DeviceType, DeviceInd, CANInd, ref pCANStatus);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static  UInt32 VCI_GetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData)
        {
            try
            {
                return Api.VCI_GetReference(DeviceType, DeviceInd, CANInd, RefType, ref pData);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  UInt32 VCI_SetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData)
        {
            try
            {
                return Api.VCI_SetReference(DeviceType, DeviceInd, CANInd, RefType, ref pData);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  UInt32 VCI_GetReceiveNum(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd)
        {
            try
            {
                return Api.VCI_GetReceiveNum(DeviceType, DeviceInd, CANInd);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  UInt32 VCI_ClearBuffer(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd)
        {
            try
            {
                return Api.VCI_ClearBuffer(DeviceType, DeviceInd, CANInd);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  UInt32 VCI_StartCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd)
        {
            try
            {
                return Api.VCI_StartCAN(DeviceType, DeviceInd, CANInd);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  UInt32 VCI_ResetCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd)
        {
            try
            {

                return Api.VCI_ResetCAN(DeviceType, DeviceInd, CANInd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  UInt32 VCI_Transmit(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pSend, UInt32 Len)
        {
            try
            {
                return Api.VCI_Transmit(DeviceType, DeviceInd, CANInd, ref pSend, Len);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static  UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, IntPtr pReceive, UInt32 Len, Int32 WaitTime)
        {
            try
            {
                return Api.VCI_Receive(DeviceType, DeviceInd, CANInd, pReceive, Len, WaitTime);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
