using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameIO.Driver
{
    //----------------------------------------------------------------------------
    //DESCRIPTION: COMSTAT  structure
    //----------------------------------------------------------------------------
    [Serializable]
    public struct COMSTAT
    {
        public Int32 fCtsHold;
        public Int32 fDsrHold;
        public Int32 fRlsdHold;
        public Int32 fXoffHold;
        public Int32 fXoffSent;
        public Int32 fEof;
        public Int32 fTxim;
        public Int32 fReserved;
        public Int32 cbInQue;
        public Int32 cbOutQue;
    }
}
