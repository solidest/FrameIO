using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace FrameIO.Driver
{
    // -----------------------------------------------------------------------------
    // DESCRIPTION: CAN frame use by driver 
    // -----------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct canmsg_t
    {
        public Int32 flags;                                       //Flags, indicating or controlling special message properties 
        public Int32 cob;                                         //CAN Object number, used in Full CAN
        public UInt32 id;                                         //CAN message ID, 4 bytes  
        public Int16 length;                                      //Number of bytes in the CAN message 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Byte[] data;
    }
}
