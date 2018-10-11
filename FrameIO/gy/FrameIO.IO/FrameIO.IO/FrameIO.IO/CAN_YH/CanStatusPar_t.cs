using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameIO.Driver
{
    // -----------------------------------------------------------------------------
    //DESCRIPTION:IOCTL Generic CAN controller status request parameter structure 
    // -----------------------------------------------------------------------------
    [Serializable]
    public struct CanStatusPar_t
    {
        public UInt32 baud;                      //Actual bit rate 
        public UInt32 status;                    //CAN controller status register 
        public UInt32 error_warning_limit;       //The error warning limit 
        public UInt32 rx_errors;                 //Content of RX error counter
        public UInt32 tx_errors;                 //Content of TX error counter 
        public UInt32 error_code;                //Content of error code register 
        public UInt32 rx_buffer_size;            //Size of rx buffer
        public UInt32 rx_buffer_used;            //number of messages
        public UInt32 tx_buffer_size;            //Size of tx buffer for wince, windows not use tx buffer
        public UInt32 tx_buffer_used;            //Number of message for wince, windows not use tx buffer s
        public UInt32 retval;                    //Return value
        public UInt32 type;                      //CAN controller/driver type
        public UInt32 acceptancecode;            //Acceptance code 
        public UInt32 acceptancemask;            //Acceptance mask
        public UInt32 acceptancemode;             //Acceptance Filter Mode: 1:Single 0:Dual
        public UInt32 selfreception;             //Self reception 
        public UInt32 readtimeout;               //Read timeout 
        public UInt32 writetimeout;              //Write timeout 
    }
}
