using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIOInterface
{
    public delegate void AsyncReadCallback(FrameBase data, object AsyncState);
    public delegate void AsyncReadListCallback(FrameBase[] data, out bool isCompleted, object AsyncState);

    /// <summary>
    /// 在流上读取数据帧的接口
    /// </summary>
    public interface IFrameReader
    {
        FrameBase ReadFrame(IFrameUnpack p);
        FrameBase[] ReadFrameList(IFrameUnpack p, int framecount);

        void BeginReadFrame(IFrameUnpack p, AsyncReadCallback callback);
        void BeginReadFrameList(IFrameUnpack p, int framecount, AsyncReadListCallback callback);
    }
}
