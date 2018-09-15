using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIO.Interface
{

    /// <summary>
    /// 帧式数据流接口，用于封装硬件驱动调用的接口
    /// </summary>
    public interface IFrameStream
    {
        /// <summary>
        /// 打开设备，准备读写数据
        /// </summary>
        /// <param name="config">配置项字典</param>
        /// <returns>打开成功返回true，失败返回false</returns>
        bool Open(Dictionary<string, object> config);

        /// <summary>
        /// 关闭设备，释放相关资源
        /// </summary>
        void Close();

        /// <summary>
        /// 清空通道缓存，复位读写缓冲区
        /// </summary>
        void ClearChannel();
    }


}
