// #############################################################################
// *****************************************************************************
//                  Copyright (c) 2011, Advantech Automation Corp.
//      THIS IS AN UNPUBLISHED WORK CONTAINING CONFIDENTIAL AND PROPRIETARY
//               INFORMATION WHICH IS THE PROPERTY OF ADVANTECH AUTOMATION CORP.
//
//    ANY DISCLOSURE, USE, OR REPRODUCTION, WITHOUT WRITTEN AUTHORIZATION FROM
//               ADVANTECH AUTOMATION CORP., IS STRICTLY PROHIBITED.
// *****************************************************************************

// #############################################################################
//
// File:    AdvCANIO.cs
// Created: 4/8/2009
// Revision:6/5/2009
// Version: 1.0
//          - Initial version
//          2.0
//          - Compatible with 64-bit and 32-bit system
//          2.1 (2011-5-19)
//          - Fix bug of  WaitCommEvent
// Description: Implements IO function about how to access CAN WDM&CE driver
//
// -----------------------------------------------------------------------------

using System;
using Microsoft.Win32.SafeHandles;
using System.Threading;
using System.Runtime.InteropServices;

namespace FrameIO.Driver
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    [Serializable]
    public class AdvCANIO
    {

        private IntPtr hDevice;                                                           //Device handle
        private IntPtr orgWriteBuf = IntPtr.Zero;                                         //Unmanaged buffer for write
        private IntPtr orgReadBuf = IntPtr.Zero;                                          //Unmanaged buffer for read
        private IntPtr lpCommandBuffer = IntPtr.Zero;                                     //Unmanaged buffer Command 
        private IntPtr lpConfigBuffer = IntPtr.Zero;                                      //Unmanaged buffer Config
        private IntPtr lpStatusBuffer = IntPtr.Zero;                                      //Unmanaged buffer Status
        private Command_par_t Command = new Command_par_t();                              //Managed buffer for Command
        private Config_par_t Config = new Config_par_t();                                 //Managed buffer for Cofig
        private Int32 OutLen;                                                               //Out data length for DeviceIoControl
        private Int32 OS_TYPE = System.IntPtr.Size;                                         //For judge x86 or x64 
        private UInt32 EventCode = 0;                                                       //Event code for WaitCommEvent
        private IntPtr lpEventCode = IntPtr.Zero;                                         //Unmanaged buffer for Event code
        private IntPtr INVALID_HANDLE_VALUE = IntPtr.Zero;                                //Invalid handle
        public const Int32 SUCCESS = 0;                                                     //Status definition : success
        public const Int32 OPERATION_ERROR = -1;                                            //Status definition : device error or parameter error
        public const Int32 TIME_OUT = -2;                                                   //Status definition : time out
        private UInt32 MaxReadMsgNumber;                                                    //Max number of message in unmanaged buffer for read 
        private UInt32 MaxWriteMsgNumber;                                                   //Max number of message in unmanaged buffer for write

        // Fields
        private Win32Events events;
        private Win32Ovrlap ioctlOvr;
        private ManualResetEvent ioctlEvent;
        private Win32Ovrlap txOvr;
        private ManualResetEvent writeEvent;
        private Win32Ovrlap rxOvr;
        private Win32Ovrlap eventOvr;
        AutoResetEvent readEvent;

        public AdvCANIO()
        {

            hDevice = INVALID_HANDLE_VALUE;

            lpCommandBuffer = Marshal.AllocHGlobal(AdvCan.CAN_COMMAND_LENGTH);
            lpConfigBuffer = Marshal.AllocHGlobal(AdvCan.CAN_CONFIG_LENGTH);
            lpStatusBuffer = Marshal.AllocHGlobal(AdvCan.CAN_CANSTATUS_LENGTH);
            lpEventCode = Marshal.AllocHGlobal(Marshal.SizeOf(EventCode));
            Marshal.StructureToPtr(EventCode, lpEventCode, true);

            this.ioctlEvent = new ManualResetEvent(false);
            this.ioctlOvr = new Win32Ovrlap(this.ioctlEvent.SafeWaitHandle.DangerousGetHandle());

            this.writeEvent = new ManualResetEvent(false);
            this.txOvr = new Win32Ovrlap(this.writeEvent.SafeWaitHandle.DangerousGetHandle());

            readEvent = new AutoResetEvent(false);
            this.rxOvr = new Win32Ovrlap(readEvent.SafeWaitHandle.DangerousGetHandle());

            this.eventOvr = new Win32Ovrlap(readEvent.SafeWaitHandle.DangerousGetHandle());
            this.events = new Win32Events(this.eventOvr.memPtr);
        }

        ~AdvCANIO()
        {
            if (hDevice != INVALID_HANDLE_VALUE)
            {
                AdvCan.CloseHandle(hDevice);
                Thread.Sleep(100);

                Marshal.FreeHGlobal(lpCommandBuffer);
                Marshal.FreeHGlobal(lpConfigBuffer);
                Marshal.FreeHGlobal(lpStatusBuffer);
                Marshal.FreeHGlobal(lpEventCode);
                Marshal.FreeHGlobal(orgWriteBuf);
                Marshal.FreeHGlobal(orgReadBuf);
                this.ioctlEvent = null;
                this.ioctlOvr = null;
                this.writeEvent = null;
                this.txOvr = null;
                this.eventOvr = null;
                this.events = null;
                hDevice = INVALID_HANDLE_VALUE;
            }

        }
        /*****************************************************************************
        *
        *    acCanOpen
        *
        *    Purpose:
        *    open can port by name 
        *		
        *
        *    Arguments:
        *        PortName                - port name
        *        synchronization         - true, synchronization ; false, asynchronous
        *        MsgNumberOfReadBuffer   - message number of read intptr
        *        MsgNumberOfWriteBuffer  - message number of write intptr
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acCanOpen(System.String CanPortName, Boolean synchronization, UInt32 MsgNumberOfReadBuffer, UInt32 MsgNumberOfWriteBuffer)
        {
            CanPortName = "\\\\.\\" + CanPortName;
            if (!synchronization)
                hDevice = AdvCan.CreateFile(CanPortName, AdvCan.GENERIC_READ + AdvCan.GENERIC_WRITE, 0, IntPtr.Zero, AdvCan.OPEN_EXISTING, AdvCan.FILE_ATTRIBUTE_NORMAL + AdvCan.FILE_FLAG_OVERLAPPED, IntPtr.Zero);
            else
                hDevice = AdvCan.CreateFile(CanPortName, AdvCan.GENERIC_READ + AdvCan.GENERIC_WRITE, 0, IntPtr.Zero, AdvCan.OPEN_EXISTING, AdvCan.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
            if (hDevice.ToInt32() == -1)
            {
                hDevice = INVALID_HANDLE_VALUE;
                return OPERATION_ERROR;
            }
            if (hDevice != INVALID_HANDLE_VALUE)
            {
                MaxReadMsgNumber = MsgNumberOfReadBuffer;
                MaxWriteMsgNumber = MsgNumberOfWriteBuffer;
                orgReadBuf = Marshal.AllocHGlobal((int)(AdvCan.CAN_MSG_LENGTH * 10000));
                orgWriteBuf = Marshal.AllocHGlobal((int)(AdvCan.CAN_MSG_LENGTH * 10000));
                return SUCCESS;
            }
            else
                return OPERATION_ERROR;
        }

        /*****************************************************************************
        *
        *    acCanClose
        *
        *    Purpose:
        *        Close can port 
        *		
        *
        *    Arguments:
        *
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acCanClose()
        {
            if (hDevice != INVALID_HANDLE_VALUE)
            {
                AdvCan.CloseHandle(hDevice);
                Thread.Sleep(100);
                Marshal.FreeHGlobal(orgWriteBuf);
                Marshal.FreeHGlobal(orgReadBuf);
                hDevice = INVALID_HANDLE_VALUE;
            }


            return SUCCESS;
        }

        /*****************************************************************************
        *
        *    acEnterResetMode
        *
        *    Purpose:
        *        Enter reset mode.
        *		
        *
        *    Arguments:
        *
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acEnterResetMode()
        {
            Boolean flag;
            Command.cmd = AdvCan.CMD_STOP;
            Marshal.StructureToPtr(Command, lpCommandBuffer, true);
            flag = AdvCan.DeviceIoControl(hDevice, AdvCan.CAN_IOCTL_COMMAND, lpCommandBuffer, AdvCan.CAN_COMMAND_LENGTH, IntPtr.Zero, 0, ref OutLen, this.ioctlOvr.memPtr);
            if (!flag)
            {
                return OPERATION_ERROR;
            }
            return SUCCESS;
        }

        /*****************************************************************************
        *
        *    acEnterWorkMode
        *
        *    Purpose:
        *        Enter work mode 
        *		
        *
        *    Arguments:
        *
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acEnterWorkMode()
        {
            Boolean flag;
            Command.cmd = AdvCan.CMD_START;
            Marshal.StructureToPtr(Command, lpCommandBuffer, true);
            flag = AdvCan.DeviceIoControl(hDevice, AdvCan.CAN_IOCTL_COMMAND, lpCommandBuffer, AdvCan.CAN_COMMAND_LENGTH, IntPtr.Zero, 0, ref OutLen, this.ioctlOvr.memPtr);
            if (!flag)
            {
                return OPERATION_ERROR;
            }
            return SUCCESS;
        }

        /*****************************************************************************
        *
        *    acClearRxFifo
        *
        *    Purpose:
        *        Clear can port receive buffer
        *		
        *
        *    Arguments:
        *
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acClearRxFifo()
        {
            Boolean flag = false;
            Command.cmd = AdvCan.CMD_CLEARBUFFERS;
            Marshal.StructureToPtr(Command, lpCommandBuffer, true);
            flag = AdvCan.DeviceIoControl(hDevice, AdvCan.CAN_IOCTL_COMMAND, lpCommandBuffer, AdvCan.CAN_COMMAND_LENGTH, IntPtr.Zero, 0, ref OutLen, this.ioctlOvr.memPtr);
            if (!flag)
            {
                return OPERATION_ERROR;
            }
            return SUCCESS;
        }

        /*****************************************************************************
        *
        *    acSetBaud
        *
        *    Purpose:
        *	     Set baudrate of the CAN Controller.The two modes of configuring
        *     baud rate are custom mode and standard mode.
        *     -   Custom mode
        *         If Baud Rate value is user defined, driver will write the first 8
        *         bit of low 16 bit in BTR0 of SJA1000.
        *         The lower order 8 bit of low 16 bit will be written in BTR1 of SJA1000.
        *     -   Standard mode
        *         Target value     BTR0      BTR1      Setting value 
        *           10K            0x31      0x1c      10 
        *           20K            0x18      0x1c      20 
        *           50K            0x09      0x1c      50 
        *          100K            0x04      0x1c      100 
        *          125K            0x03      0x1c      125 
        *          250K            0x01      0x1c      250 
        *          500K            0x00      0x1c      500 
        *          800K            0x00      0x16      800 
        *         1000K            0x00      0x14      1000 
        *		
        *
        *    Arguments:
        *        BaudRateValue     - baudrate will be set
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acSetBaud(uint BaudRateValue)
        {
            Boolean flag;
            Config.target = AdvCan.CONF_TIMING;
            Config.val1 = BaudRateValue;
            Marshal.StructureToPtr(Config, lpConfigBuffer, true);
            flag = AdvCan.DeviceIoControl(hDevice, AdvCan.CAN_IOCTL_CONFIG, lpConfigBuffer, AdvCan.CAN_CONFIG_LENGTH, IntPtr.Zero, 0, ref OutLen, this.ioctlOvr.memPtr);
            if (!flag)
            {
                return OPERATION_ERROR;
            }
            return SUCCESS;
        }

        /*****************************************************************************
        *
        *    acSetBaudRegister
        *
        *    Purpose:
        *        Configures baud rate by custom mode.
        *		
        *
        *    Arguments:
        *        Btr0           - BTR0 register value.
        *        Btr1           - BTR1 register value.
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acSetBaudRegister(Byte Btr0, Byte Btr1)
        {
            UInt32 BaudRateValue = (uint)(Btr0 * 256 + Btr1);
            return acSetBaud(BaudRateValue);
        }

        /*****************************************************************************
        *
        *    acSetTimeOut
        *
        *    Purpose:
        *        Set timeout for read and write  
        *		
        *
        *    Arguments:
        *        ReadTimeOutValue                   - ms
        *        WriteTimeOutValue                  - ms
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acSetTimeOut(uint ReadTimeOutValue, UInt32 WriteTimeOutValue)
        {
            Boolean flag;
            Config.target = AdvCan.CONF_TIMEOUT;
            Config.val1 = WriteTimeOutValue;
            Config.val2 = ReadTimeOutValue;
            Marshal.StructureToPtr(Config, lpConfigBuffer, true);
            flag = AdvCan.DeviceIoControl(hDevice, AdvCan.CAN_IOCTL_CONFIG, lpConfigBuffer, AdvCan.CAN_CONFIG_LENGTH, IntPtr.Zero, 0, ref OutLen, this.ioctlOvr.memPtr);
            if (!flag)
            {
                return OPERATION_ERROR;
            }
            return SUCCESS;
        }

        /*****************************************************************************
        *
        *    acSetSelfReception
        *
        *    Purpose:
        *        Set support for self reception 
        *		
        *
        *    Arguments:
        *        SelfFlag      - true, open self reception; false, close self reception
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acSetSelfReception(bool SelfFlag)
        {
            Boolean flag;
            Config.target = AdvCan.CONF_SELF_RECEPTION;
            if (SelfFlag)
                Config.val1 = 1;
            else
                Config.val1 = 0;
            Marshal.StructureToPtr(Config, lpConfigBuffer, true);
            flag = AdvCan.DeviceIoControl(hDevice, AdvCan.CAN_IOCTL_CONFIG, lpConfigBuffer, AdvCan.CAN_CONFIG_LENGTH, IntPtr.Zero, 0, ref OutLen, this.ioctlOvr.memPtr);
            if (!flag)
            {
                return OPERATION_ERROR;
            }
            return SUCCESS;
        }

        /*****************************************************************************
        *
        *    acSetListenOnlyMode
        *
        *    Purpose:
        *        Set listen only mode of the CAN Controller
        *		
        *
        *    Arguments:
        *        ListenOnly        - true, open only listen mode; false, close only listen mode
        *    Returns:
        *        =0 succeeded; or <0 Failed 
        *
        *****************************************************************************/
        public Int32 acSetListenOnlyMode(bool ListenOnly)
        {
            Boolean flag;
            Config.target = AdvCan.CONF_LISTEN_ONLY_MODE;
            if (ListenOnly)
                Config.val1 = 1;
            else
                Config.val1 = 0;
            Marshal.StructureToPtr(Config, lpConfigBuffer, true);
            flag = AdvCan.DeviceIoControl(hDevice, AdvCan.CAN_IOCTL_CONFIG, lpConfigBuffer, AdvCan.CAN_CONFIG_LENGTH, IntPtr.Zero, 0, ref OutLen, this.ioctlOvr.memPtr);
            if (!flag)
            {
                return OPERATION_ERROR;
            }
            return SUCCESS;
        }

        /*****************************************************************************
        *
        *    acSetAcceptanceFilterMode
        *
        *    Purpose:
        *        Set acceptance filter mode of the CAN Controller
        *		
        *
        *    Arguments:
        *        FilterMode     - PELICAN_SINGLE_FILTER, single filter mode; PELICAN_DUAL_FILTER, dule filter mode
        *    Returns:
        *        =0 succeeded; or <0 Failed 
        *
        *****************************************************************************/
        public Int32 acSetAcceptanceFilterMode(uint FilterMode)
        {
            Boolean flag = false;
            Config.target = AdvCan.CONF_ACC_FILTER;
            Config.val1 = FilterMode;
            Marshal.StructureToPtr(Config, lpConfigBuffer, true);
            flag = AdvCan.DeviceIoControl(hDevice, AdvCan.CAN_IOCTL_CONFIG, lpConfigBuffer, AdvCan.CAN_CONFIG_LENGTH, IntPtr.Zero, 0, ref OutLen, this.ioctlOvr.memPtr);
            if (!flag)
            {
                return OPERATION_ERROR;
            }
            return SUCCESS;
        }

        /*****************************************************************************
        *
        *    acSetAcceptanceFilterMask
        *
        *    Purpose:
        *        Set acceptance filter mask of the CAN Controller
        *		
        *
        *    Arguments:
        *        Mask              - acceptance filter mask
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acSetAcceptanceFilterMask(uint Mask)
        {
            Boolean flag = false;
            Config.target = AdvCan.CONF_ACCM;
            Config.val1 = Mask;
            Marshal.StructureToPtr(Config, lpConfigBuffer, true);
            flag = AdvCan.DeviceIoControl(hDevice, AdvCan.CAN_IOCTL_CONFIG, lpConfigBuffer, AdvCan.CAN_CONFIG_LENGTH, IntPtr.Zero, 0, ref OutLen, this.ioctlOvr.memPtr);
            if (!flag)
            {
                return OPERATION_ERROR;
            }
            return SUCCESS;
        }

        /*****************************************************************************
        *
        *    acSetAcceptanceFilterCode
        *
        *    Purpose:
        *        Set acceptance filter code of the CAN Controller
        *		
        *
        *    Arguments:
        *        Code        - acceptance filter code
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acSetAcceptanceFilterCode(uint Code)
        {
            Boolean flag = false;
            Config.target = AdvCan.CONF_ACCC;
            Config.val1 = Code;
            Marshal.StructureToPtr(Config, lpConfigBuffer, true);
            flag = AdvCan.DeviceIoControl(hDevice, AdvCan.CAN_IOCTL_CONFIG, lpConfigBuffer, AdvCan.CAN_CONFIG_LENGTH, IntPtr.Zero, 0, ref OutLen, this.ioctlOvr.memPtr);
            if (!flag)
            {
                return OPERATION_ERROR;
            }
            return SUCCESS;
        }

        /*****************************************************************************
        *
        *    acSetAcceptanceFilter
        *
        *    Purpose:
        *        Set acceptance filter code and mask of the CAN Controller 
        *		
        *
        *    Arguments:
        *        Mask              - acceptance filter mask
        *        Code              - acceptance filter code
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acSetAcceptanceFilter(uint Mask, UInt32 Code)
        {
            Boolean flag = false;
            Config.target = AdvCan.CONF_ACC;
            Config.val1 = Mask;
            Config.val2 = Code;
            Marshal.StructureToPtr(Config, lpConfigBuffer, true);
            flag = AdvCan.DeviceIoControl(hDevice, AdvCan.CAN_IOCTL_CONFIG, lpConfigBuffer, AdvCan.CAN_CONFIG_LENGTH, IntPtr.Zero, 0, ref OutLen, this.ioctlOvr.memPtr);
            if (!flag)
            {
                return OPERATION_ERROR;
            }
            return SUCCESS;
        }

        /*****************************************************************************
        *
        *    acGetStatus
        *
        *    Purpose:
        *        Get the current status of the driver and the CAN Controller
        *		
        *
        *    Arguments:
        *        Status    - status buffer
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acGetStatus(ref CanStatusPar_t Status)
        {
            Boolean flag = false;
            flag = AdvCan.DeviceIoControl(hDevice, AdvCan.CAN_IOCTL_STATUS, IntPtr.Zero, 0, lpStatusBuffer, AdvCan.CAN_CANSTATUS_LENGTH, ref OutLen, this.ioctlOvr.memPtr);
            if (!flag)
            {
                return OPERATION_ERROR;
            }
            Status = (CanStatusPar_t)(Marshal.PtrToStructure(lpStatusBuffer, typeof(CanStatusPar_t)));
            return SUCCESS;
        }


        /*****************************************************************************
        *
        *    acCanWrite
        *
        *    Purpose:
        *        Write can msg
        *		
        *
        *    Arguments:
        *        msgWrite              - managed buffer for write
        *        nWriteCount           - msg number for write
        *        pulNumberofWritten    - real msgs have written
        *
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/

        public Int32 acCanWrite(canmsg_t[] msgWrite, UInt32 nWriteCount, ref UInt32 pulNumberofWritten)
        {
            Boolean flag;
            Int32 nRet;
            UInt32 dwErr;
            if (nWriteCount > MaxWriteMsgNumber)
                nWriteCount = MaxWriteMsgNumber;
            pulNumberofWritten = 0;
            flag = AdvCan.WriteFile(hDevice, msgWrite, nWriteCount, out pulNumberofWritten, this.txOvr.memPtr);
            if (flag)
            {
                if (nWriteCount > pulNumberofWritten)
                    nRet = TIME_OUT;                          //Sending data timeout
                else
                    nRet = SUCCESS;                               //Sending data ok
            }
            else
            {
                dwErr = (uint)Marshal.GetLastWin32Error();
                if (dwErr == AdvCan.ERROR_IO_PENDING)
                {
                    if (AdvCan.GetOverlappedResult(hDevice, this.txOvr.memPtr, out pulNumberofWritten, true))
                    {
                        if (nWriteCount > pulNumberofWritten)
                            nRet = TIME_OUT;                    //Sending data timeout
                        else
                            nRet = SUCCESS;                         //Sending data ok
                    }
                    else
                        nRet = OPERATION_ERROR;                         //Sending data error
                }
                else
                    nRet = OPERATION_ERROR;                            //Sending data error
            }
            return nRet;
        }


        /*****************************************************************************
        *
        *    acCanRead
        *
        *    Purpose:
        *        Read can message.
        *		
        *
        *    Arguments:
        *        msgRead           - managed buffer for read
        *        nReadCount        - msg number that unmanaged buffer can preserve
        *        pulNumberofRead   - real msgs have read
        *		
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acCanRead(canmsg_t[] msgRead, UInt32 nReadCount, ref UInt32 pulNumberofRead)
        {
            Boolean flag;
            Int32 nRet;
            UInt32 i;
            if (nReadCount > MaxReadMsgNumber)
                nReadCount = MaxReadMsgNumber;
            pulNumberofRead = 0;
            flag = AdvCan.ReadFile(hDevice, orgReadBuf, nReadCount, out pulNumberofRead, this.rxOvr.memPtr);
            if (flag)
            {
                if (pulNumberofRead == 0)
                {
                    nRet = TIME_OUT;
                }
                else
                {
                    for (i = 0; i < pulNumberofRead; i++)
                    {
                        if (OS_TYPE == 8)
                            msgRead[i] = (canmsg_t)(Marshal.PtrToStructure(new IntPtr(orgReadBuf.ToInt64() + AdvCan.CAN_MSG_LENGTH * i), typeof(canmsg_t)));
                        else
                            msgRead[i] = (canmsg_t)(Marshal.PtrToStructure(new IntPtr(orgReadBuf.ToInt32() + AdvCan.CAN_MSG_LENGTH * i), typeof(canmsg_t)));

                    }
                    nRet = SUCCESS;
                }
            }
            else
            {
                if (AdvCan.GetOverlappedResult(hDevice, this.rxOvr.memPtr, out pulNumberofRead, true))
                {
                    if (pulNumberofRead == 0)
                    {
                        nRet = TIME_OUT;                               //Package receiving timeout
                    }
                    else
                    {
                        for (i = 0; i < pulNumberofRead; i++)
                        {
                            if (OS_TYPE == 8)
                                msgRead[i] = (canmsg_t)(Marshal.PtrToStructure(new IntPtr(orgReadBuf.ToInt64() + AdvCan.CAN_MSG_LENGTH * i), typeof(canmsg_t)));
                            else
                                msgRead[i] = (canmsg_t)(Marshal.PtrToStructure(new IntPtr(orgReadBuf.ToInt32() + AdvCan.CAN_MSG_LENGTH * i), typeof(canmsg_t)));
                        }
                        nRet = SUCCESS;
                    }
                }
                else
                    nRet = OPERATION_ERROR;                                    //Package receiving error
            }
            return nRet;
        }
        /*****************************************************************************
        *
        *    acClearCommError
        *
        *    Purpose:
        *        Execute ClearCommError of AdvCan.
        *		
        *
        *    Arguments:
        *        ErrorCode      - error code if the CAN Controller occur error
        * 
        * 
        *    Returns:
        *        true SUCCESS; or false failure 
        *
        *****************************************************************************/
        public Boolean acClearCommError(ref UInt32 ErrorCode)
        {
            COMSTAT lpState = new COMSTAT();
            return AdvCan.ClearCommError(hDevice, out ErrorCode, out lpState);
        }

        /*****************************************************************************
        *
        *    acSetCommMask
        *
        *    Purpose:
        *        Execute SetCommMask of AdvCan.
        *		
        *
        *    Arguments:
        *        EvtMask    - event type
        * 
        * 
        *    Returns:
        *        true SUCCESS; or false failure 
        *
        *****************************************************************************/
        public Boolean acSetCommMask(uint EvtMask)
        {
            if (!AdvCan.SetCommMask(hDevice, EvtMask))
            {
                Int32 num1 = Marshal.GetLastWin32Error();
                return false;
            }
            Marshal.WriteInt32(this.events.evPtr, 0);
            return true;

        }

        /*****************************************************************************
        *
        *    acGetCommMask
        *
        *    Purpose:
        *        Execute GetCommMask of AdvCan.
        *		
        *
        *    Arguments:
        *        EvtMask     - event type
        * 
        * 
        *    Returns:
        *        true SUCCESS; or false failure 
        *
        *****************************************************************************/
        public Boolean acGetCommMask(ref UInt32 EvtMask)
        {
            return AdvCan.GetCommMask(hDevice, ref EvtMask);
        }

        /*****************************************************************************
        *
        *    acWaitEvent
        *
        *    Purpose:
        *        Wait can message or error of the CAN Controller.
        *		
        *
        *    Arguments:
        *        msgRead           - managed buffer for read
        *        nReadCount        - msg number that unmanaged buffer can preserve
        *        pulNumberofRead   - real msgs have read
        *        ErrorCode         - return error code when the CAN Controller has error
        * 
        *    Returns:
        *        =0 SUCCESS; or <0 failure 
        *
        *****************************************************************************/
        public Int32 acWaitEvent(canmsg_t[] msgRead, UInt32 nReadCount, ref UInt32 pulNumberofRead, ref UInt32 ErrorCode)
        {
            Int32 nRet = OPERATION_ERROR;
            ErrorCode = 0;
            pulNumberofRead = 0;
            if (AdvCan.WaitCommEvent(hDevice, this.events.evPtr, this.events.olPtr) == true)
            {
                EventCode = (uint)Marshal.ReadInt32(this.events.evPtr, 0);
                if ((EventCode & AdvCan.EV_RXCHAR) != 0)
                {
                    nRet = acCanRead(msgRead, nReadCount, ref pulNumberofRead);
                }
                if ((EventCode & AdvCan.EV_ERR) != 0)
                {
                    nRet = OPERATION_ERROR;
                    acClearCommError(ref ErrorCode);
                }
            }
            else
            {
                Int32 err = Marshal.GetLastWin32Error();
                if (AdvCan.ERROR_IO_PENDING == err)
                {
                    if (AdvCan.GetOverlappedResult(hDevice, this.eventOvr.memPtr, out pulNumberofRead, true))
                    {
                        EventCode = (uint)Marshal.ReadInt32(this.events.evPtr, 0);
                        if ((EventCode & AdvCan.EV_RXCHAR) != 0)
                        {
                            nRet = acCanRead(msgRead, nReadCount, ref pulNumberofRead);
                        }
                        if ((EventCode & AdvCan.EV_ERR) != 0)
                        {
                            nRet = OPERATION_ERROR;
                            acClearCommError(ref ErrorCode);
                        }
                    }
                    else
                        nRet = OPERATION_ERROR;
                }
                else
                    nRet = OPERATION_ERROR;
            }

            return nRet;
        }
    }

    internal class Win32Events
    {
        // Methods
        internal Win32Events(IntPtr ol)
        {
            this.olPtr = ol;
            this.evPtr = Marshal.AllocHGlobal(sizeof(uint));
            Marshal.WriteInt32(this.evPtr, 0);
        }

        ~Win32Events()
        {
            Marshal.FreeHGlobal(this.evPtr);
        }

        public IntPtr evPtr;
        public IntPtr olPtr;
    }

    internal class Win32Ovrlap
    {
        // Methods
        internal Win32Ovrlap(IntPtr evHandle)
        {
            this.ol = new OVERLAPPED();
            this.ol.offset = 0;
            this.ol.offsetHigh = 0;
            this.ol.hEvent = evHandle;
            if (evHandle != IntPtr.Zero)
            {
                this.memPtr = Marshal.AllocHGlobal(Marshal.SizeOf(this.ol));
                Marshal.StructureToPtr(this.ol, this.memPtr, false);
            }
        }

        ~Win32Ovrlap()
        {
            if (this.memPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.memPtr);
            }
        }

        public IntPtr memPtr;
        public OVERLAPPED ol;

    }
}