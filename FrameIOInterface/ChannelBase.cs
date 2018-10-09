using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Interface
{
    /// <summary>
    /// 驱动封装接口
    /// </summary>
    public interface IChannelBase : IFrameReader, IFrameWriter, IFrameStream
    {

    }
}
