using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIO.Interface
{
    /// <summary>
    /// 数据帧解包接口，用于在流上读取数据帧的操作
    /// </summary>
    public interface IFrameUnpack
    {
        /// <summary>
        /// 数据帧解包时所需要的第一块字节流大小
        /// </summary>
        int FirstBlockSize { get; }

        /// <summary>
        /// 数据帧解包所需要字节流的块数
        /// </summary>
        int BlockCount { get; }

        /// <summary>
        /// 将读取到的字节流追加到解包缓冲区，返回解包所需下一块字节流大小
        /// </summary>
        /// <param name="buffer">读取到的字节流内容</param>
        /// <returns>解析所需要的下一块字节流大小，0表示已具备完整数据帧字节流</returns>
        int AppendBlock(byte[] buffer);

        /// <summary>
        /// 执行解包操作，返回解包后得到的对象
        /// </summary>
        /// <returns>解包后的数据帧对象</returns>
        FrameBase Unpack();

        /// <summary>
        /// 重置解包缓冲区
        /// </summary>
        void Reset();
    }
}
