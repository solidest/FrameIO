using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIO.Interface
{
    /// <summary>
    /// 数据帧打包接口，供发送数据帧时将数据帧打包成字节流时调用
    /// </summary>
    public interface IFramePack
    {
        /// <summary>
        /// 需要打包的数据帧数量，有多个数据帧同时打包时值大于1
        /// </summary>
        int FrameCount { get; }

        /// <summary>
        /// 将下一帧数据打包，每调用一次打包一帧数据，并返回打包后的字节流
        /// </summary>
        /// <returns>返回单个数据帧打包后的字节流</returns>
        byte[] NextPack();

        /// <summary>
        /// 将全部数据一次性打包成字节流
        /// </summary>
        /// <returns>返回全部数据帧打包后的字节流</returns>
        byte[] GetAllPack();
    }
}
