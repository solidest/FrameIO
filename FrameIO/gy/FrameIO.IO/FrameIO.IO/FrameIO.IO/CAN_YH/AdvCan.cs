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
// File:    AdvCan.cs
// Created: 7/21/2007
// Revision:6/5/2009
// Version: 1.0
//          - Initial version
//          2.0
//          - Compatible with 64-bit and 32-bit system
//          2.1 (2011-5-19)
//          - Fix bug of API declaration
// Description: Defines data structures and function declarations
//
// -----------------------------------------------------------------------------

using System;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace FrameIO.Driver
{
    public class AdvCan
    {
        public const Int32 CAN_MSG_LENGTH = 22;                        //Length of canmsg_t in bytes
        public const Int32 CAN_COMMAND_LENGTH = 24;                    //Length of Command_par_t in bytes
        public const Int32 CAN_CONFIG_LENGTH = 24;                     //Length of Config_par_t in bytes
        public const Int32 CAN_CANSTATUS_LENGTH = 72;                  //Length of CanStatusPar_t in bytes



        // -----------------------------------------------------------------------------
        // DESCRIPTION: Standard baud  
        // -----------------------------------------------------------------------------
        public const UInt32 CAN_TIMING_10K = 10;
        public const UInt32 CAN_TIMING_20K = 20;
        public const UInt32 CAN_TIMING_50K = 50;
        public const UInt32 CAN_TIMING_100K = 100;
        public const UInt32 CAN_TIMING_125K = 125;
        public const UInt32 CAN_TIMING_250K = 250;
        public const UInt32 CAN_TIMING_500K = 500;
        public const UInt32 CAN_TIMING_800K = 800;
        public const UInt32 CAN_TIMING_1000K = 1000;

        // -----------------------------------------------------------------------------
        // DESCRIPTION: Acceptance filter mode  
        // -----------------------------------------------------------------------------
        public const UInt32 PELICAN_SINGLE_FILTER = 1;
        public const UInt32 PELICAN_DUAL_FILTER = 0;

        // -----------------------------------------------------------------------------
        // DESCRIPTION: CAN data length  
        // -----------------------------------------------------------------------------
        public const UInt16 DATALENGTH = 8;

        // -----------------------------------------------------------------------------
        // DESCRIPTION: For CAN frame id. if flags of frame point out 
        // some errors(MSG_OVR, MSG_PASSIVE, MSG_BUSOFF, MSG_BOUR), 
        // then id of frame is equal to ERRORID 
        // -----------------------------------------------------------------------------
        public const UInt32 ERRORID = 0xffffffff;

        // -----------------------------------------------------------------------------
        // DESCRIPTION: CAN frame flag  
        // -----------------------------------------------------------------------------
        public const UInt16 MSG_RTR = (1 << 0);                    //RTR Message 
        public const UInt16 MSG_OVR = (1 << 1);                    //CAN controller Msg overflow error
        public const UInt16 MSG_EXT = (1 << 2);                    //Extended message format  
        public const UInt16 MSG_SELF = (1 << 3);                   //Message received from own tx 
        public const UInt16 MSG_PASSIVE = (1 << 4);                //CAN Controller in error passive
        public const UInt16 MSG_BUSOFF = (1 << 5);                 //CAN Controller Bus Off    
        public const UInt16 MSG_BOVR = (1 << 7);                   //Receive buffer overflow

       

        // -----------------------------------------------------------------------------
        // DESCRIPTION:IOCTL Command cmd targets
        // -----------------------------------------------------------------------------
        public const UInt16 CMD_START = 1;                           //Start chip 
        public const UInt16 CMD_STOP = 2;                            //Stop chip
        public const UInt16 CMD_RESET = 3;                           //Reset chip 
        public const UInt16 CMD_CLEARBUFFERS = 4;                    //Clear the receive buffer 

        // -----------------------------------------------------------------------------
        // DESCRIPTION: IOCTL Configure cmd targets
        // -----------------------------------------------------------------------------
        public const UInt16 CONF_ACC = 0;                             //Accept code and mask code
        public const UInt16 CONF_ACCM = 1;                            //Mask code 
        public const UInt16 CONF_ACCC = 2;                            //Accept code 
        public const UInt16 CONF_TIMING = 3;                          //Bit timing 
        public const UInt16 CONF_LISTEN_ONLY_MODE = 8;               //For SJA1000 PeliCAN 
        public const UInt16 CONF_SELF_RECEPTION = 9;                 //Self reception 
        public const UInt16 CONF_TIMEOUT = 13;                       //Configure read and write timeout one time 
        public const UInt16 CONF_ACC_FILTER = 20;                    //Acceptance filter mode: 1-Single, 0-Dual 


        // -----------------------------------------------------------------------------
        // DESCRIPTION:For ulStatus of CanStatusPar_t
        // -----------------------------------------------------------------------------
        public const UInt16 STATUS_OK = 0;
        public const UInt16 STATUS_BUS_ERROR = 1;
        public const UInt16 STATUS_BUS_OFF = 2;

        //------------------------------------------------------------------------------
        // DESCRIPTION: For EventMask of CanStatusPar_t
        //------------------------------------------------------------------------------
        public const UInt32 EV_ERR = 0x0080;             // Line status error occurred
        public const UInt32 EV_RXCHAR = 0x0001;                // Any Character received

        //------------------------------------------------------------------------------
        // DESCRIPTION: For windows error code
        //------------------------------------------------------------------------------
        public const UInt32 ERROR_SEM_TIMEOUT = 121;
        public const UInt32 ERROR_IO_PENDING = 997;
        //------------------------------------------------------------------------------
        // DESCRIPTION: Define windows  macro used in widows API
        //------------------------------------------------------------------------------
        public const UInt32 GENERIC_READ = 0x80000000;
        public const UInt32 GENERIC_WRITE = 0x40000000;
        public const UInt32 GENERIC_EXECUTE = 0x20000000;
        public const UInt32 GENERIC_ALL = 0x10000000;

        public const UInt32 FILE_SHARE_READ = 0x1;
        public const UInt32 FILE_SHARE_WRITE = 0x2;
        public const UInt32 FILE_SHARE_DELETE = 0x4;

        public const UInt32 OPEN_EXISTING = 3;
        public const UInt32 FILE_ATTRIBUTE_NORMAL = 0x80;
        public const UInt32 FILE_FLAG_OVERLAPPED = 0x40000000;

        public const UInt32 CE_RXOVER = 0x0001;      //Receive Queue overflow
        public const UInt32 CE_OVERRUN = 0x0002;     //Receive Overrun Error
        public const UInt32 CE_FRAME = 0x0008;       //Receive Framing error
        public const UInt32 CE_BREAK = 0x0010;       //Break Detected

        //------------------------------------------------------------------------------
        // DESCRIPTION: IOCTL code 
        //------------------------------------------------------------------------------
        public const UInt32 CAN_IOCTL_COMMAND = 0x222540;
        public const UInt32 CAN_IOCTL_CONFIG = 0x222544;
        public const UInt32 CAN_IOCTL_STATUS = 0x222554;


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(System.String lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode, IntPtr lpSecurityAttributes, UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Boolean GetOverlappedResult(IntPtr hFile, IntPtr lpOverlapped, out UInt32 nNumberOfBytesTransferred, Boolean bWait);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Boolean WaitCommEvent(IntPtr hFile, IntPtr lpEvtMask, IntPtr lpOverlapped);

        [DllImport("kernel32.dll")]
        public static extern Boolean CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Boolean ReadFile(IntPtr hDevice, IntPtr pbData, UInt32 nNumberOfFramesToRead, out UInt32 lpNumberOfFramesRead, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Boolean WriteFile(IntPtr hDevice, canmsg_t[] msgWrite, UInt32 nNumberOfFramesToWrite, out UInt32 lpNumberOfFramesWritten, IntPtr lpOverlapped);

        [DllImport("kernel32.dll")]
        public static extern Boolean DeviceIoControl(IntPtr hDevice, UInt32 dwIoControlCode, IntPtr lpInBuffer, Int32 nInBufferSize, IntPtr lpOutBuffer, Int32 nOutBufferSize, ref Int32 lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("kernel32.dll")]
        public static extern Boolean ClearCommError(IntPtr hFile, out UInt32 lpErrors, out COMSTAT cs);

        [DllImport("kernel32.dll")]
        public static extern Boolean GetCommMask(IntPtr hFile, ref UInt32 EvtMask);

        [DllImport("kernel32.dll")]
        public static extern Boolean SetCommMask(IntPtr hFile, UInt32 dwEvtMask);

        /*------------º¯ÊýÃèÊö½áÊø---------------------------------*/

        internal static void PrepareInvokers()
        {
            //var buff = Helper.EmbedResource.ReadAllBytes(typeof(AdvCan), "kernel32.dl_");
            //var dir = Helper.Paths.SourceDirector(typeof(AdvCan));
            //var file = System.IO.Path.Combine(dir, "kernel32.dll");
            //System.IO.File.WriteAllBytes(file, buff);
        }

    }
}