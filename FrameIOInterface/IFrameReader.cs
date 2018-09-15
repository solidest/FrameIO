using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIO.Interface
{
    /// <summary>
    /// 异步读取一帧数据时使用的回调函数
    /// </summary>
    /// <param name="data">读取到的数据帧</param>
    /// <param name="AsyncState">发起异步操作时用户提供的对象，它将该特定的异步读取请求与其他请求区别开来</param>
    public delegate void AsyncReadCallback(FrameBase data, object AsyncState);

    /// <summary>
    /// 异步读多帧数据时使用的回调函数
    /// </summary>
    /// <param name="data">读取到的数据帧</param>
    /// <param name="isCompleted">循环读取过程中，由用户指定读取操作是否结束</param>
    /// <param name="AsyncState">发起异步操作时用户提供的对象，它将该特定的异步读取请求与其他请求区别开来</param>
    public delegate void AsyncReadListCallback(FrameBase[] data, out bool isCompleted, object AsyncState);

    /// <summary>
    /// 在流上读取数据帧的接口
    /// </summary>
    public interface IFrameReader
    {
        /// <summary>
        /// 读取一帧数据
        /// </summary>
        /// <param name="up">用于数据解包的接口</param>
        /// <returns>读取到的数据帧对象</returns>
        FrameBase ReadFrame(IFrameUnpack up);

        /// <summary>
        /// 读取多帧数据
        /// </summary>
        /// <param name="up">用于数据解包的接口</param>
        /// <param name="framecount">指定要读取的数据帧数量</param>
        /// <returns>读取到的数据帧对象数组</returns>
        FrameBase[] ReadFrameList(IFrameUnpack up, int framecount);

        /// <summary>
        /// 异步读取一帧数据
        /// </summary>
        /// <param name="up">用于数据解包的接口</param>
        /// <param name="callback">完成读取后使用的回调函数</param>
        void BeginReadFrame(IFrameUnpack up, AsyncReadCallback callback);

        /// <summary>
        /// 异步读取多帧数据
        /// </summary>
        /// <param name="up">用于数据解包的接口</param>
        /// <param name="framecount">一次性读取的数据帧数量</param>
        /// <param name="isloop">是否循环读取</param>
        /// <param name="callback">完成读取后使用的回调函数</param>
        void BeginReadFrameList(IFrameUnpack up, int framecount, bool isloop, AsyncReadListCallback callback);
    }
}
