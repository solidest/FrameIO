using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIOInterface
{
    /// <summary>
    /// 数据帧打包接口
    /// </summary>
    public interface IFramePack
    {
        int FrameCount { get; }
        byte[] NextPack();
        byte[] GetAllPack();
    }
}
