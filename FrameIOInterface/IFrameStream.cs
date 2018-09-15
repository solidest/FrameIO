using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIO.Interface
{

    /// <summary>
    /// 数据帧流
    /// </summary>
    public interface IFrameStream
    {
        bool Open(Dictionary<string, object> config);
        void Close();
        void ClearChannel();
    }


}
