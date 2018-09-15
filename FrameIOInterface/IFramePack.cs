using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIO.Interface
{
    /// <summary>
    /// 数据帧打包接口
    /// </summary>
    public interface IFramePack
    {
        /// <summary>
        /// 需要打包的数据帧数量
        /// </summary>
        int FrameCount { get; }

        /// <summary>
        /// 将下一帧数据打包
        /// </summary>
        /// <returns>返回单个数据帧打包后的字节流</returns>
        byte[] NextPack();

        /// <summary>
        /// 将全部数据一次性打包
        /// </summary>
        /// <returns>返回全部数据帧打包后的字节流</returns>
        byte[] GetAllPack();
    }
}
