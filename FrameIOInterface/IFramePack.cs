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
        /// 将数据帧打包成字节流
        /// </summary>
        /// <returns>返回数据帧打包后的字节流</returns>
        byte[] Pack();


    }
}
