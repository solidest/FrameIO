using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    public static class FioNetRunner
    {
        public static FioChannel GetChannel(ChannelOption chops)
        {
            return IORunner.GetChannel((ChannelTypeEnum)chops.GetOption("$channeltype"), chops);
        }
    }
}
