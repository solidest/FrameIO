using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIO.Interface
{
    /// <summary>
    /// 数据帧基类，所有数据帧类型均继承自该类型
    /// </summary>
    public class FrameBase
    {
        /// <summary>
        /// 获取一个ushort类型值
        /// </summary>
        /// <param name="segname">指定字段名称</param>
        /// <returns>返回字段的值</returns>
        public ushort GetUShort(string segname)
        {
            return 0;
        }

    }
}
