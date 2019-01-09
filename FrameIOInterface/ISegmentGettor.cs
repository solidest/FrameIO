using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIO.Interface
{
    /// <summary>
    /// 数据帧的字段内容读取接口
    /// </summary>
    public interface ISegmentGettor
    {

        /// <summary>
        /// 获取子数据帧的数据读取接口
        /// </summary>
        /// <returns>解析后的数据帧对象</returns>
        object GetFrameObject();
    }

}
