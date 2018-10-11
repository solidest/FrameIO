using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameIO.Driver
{
    //----------------------------------------------------------------------------
    //DESCRIPTION: IOCTL configuration request parameter structure 
    //----------------------------------------------------------------------------
    [Serializable]
    public struct Config_par_t
    {
        public Int32 cmd;                          //special driver command
        public Int32 target;                       //special configuration target 
        public UInt32 val1;                        //parameter 1
        public UInt32 val2;                        //parameter 2 
        public Int32 errorv;                       //return value
        public Int32 retval;                       //return value
    }
}
