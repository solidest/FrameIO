using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIOInterface
{
    public delegate void AsyncWriteCallback(int completedCount, object AsyncState);

    /// <summary>
    /// 在流上写入数据帧的接口
    /// </summary>
    public interface IFrameWriter
    {
        int WriteFrame(IFramePack p);
        void BeginWriteFrame(IFramePack p, AsyncWriteCallback callback, object AsyncState);
    }
}
