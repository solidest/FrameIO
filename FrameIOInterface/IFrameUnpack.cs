using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIOInterface
{
    /// <summary>
    /// 数据帧打包接口
    /// </summary>
    public interface IFrameUnpack
    {
        int FirstBlockSize { get; }
        int BlockCount { get; }
        int AppendBlock(byte[] buffer);
        FrameBase Unpack();
        void Reset();
    }
}
