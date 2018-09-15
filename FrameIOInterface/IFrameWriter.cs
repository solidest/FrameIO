using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIO.Interface
{
    /// <summary>
    /// 异步写入完成后的回调函数
    /// </summary>
    /// <param name="completedCount">完成写入的数据帧数量</param>
    /// <param name="AsyncState">用户发起异步操作时提供的对象，它将该特定的异步读取请求与其他请求区别开来</param>
    public delegate void AsyncWriteCallback(int completedCount, object AsyncState);

    /// <summary>
    /// 在流上写入数据帧的接口
    /// </summary>
    public interface IFrameWriter
    {
        /// <summary>
        /// 在帧式流上写入数据帧
        /// </summary>
        /// <param name="p">数据帧打包接口</param>
        /// <returns>返回成功写入的数据帧数量</returns>
        int WriteFrame(IFramePack p);

        /// <summary>
        /// 在帧式流上异步写入数据帧
        /// </summary>
        /// <param name="p">数据帧打包接口</param>
        /// <param name="callback">写入完成后的回调函数</param>
        /// <param name="AsyncState">一个用户提供的对象，它将该特定的异步读取请求与其他请求区别开来</param>
        void BeginWriteFrame(IFramePack p, AsyncWriteCallback callback, object AsyncState);
    }
}
