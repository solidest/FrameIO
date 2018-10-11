using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace FrameIO.Driver
{

    //----------------------------------------------------------------------------
    //DESCRIPTION: Asynchronous OVERLAPPED structure
    //----------------------------------------------------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct OVERLAPPED
    {
        public UIntPtr internalLow;
        public UIntPtr internalHigh;
        public UInt32 offset;
        public UInt32 offsetHigh;
        public IntPtr hEvent;
    }
}
