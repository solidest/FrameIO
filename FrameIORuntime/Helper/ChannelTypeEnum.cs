using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    public enum ChannelTypeEnum
    {
        COM = 1,
        CAN,
        TCPSERVER,
        TCPCLIENT,
        UDP,
        DIO
    }
}
