using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Interface
{
    /// <summary>
    /// FrameIO库抛出的异常类型
    /// </summary>
    public class FrameIOException:Exception
    {
        /// <summary>
        /// 异常构造函数
        /// </summary>
        /// <param name="errtype">错误类型</param>
        /// <param name="position">出错位置</param>
        /// <param name="errinfo">错误描述信息</param>
        public FrameIOException(FrameIOErrorType errtype, string position, string errinfo)
        {
            ErrType = errtype;
            Position = position;
            ErrInfo = errinfo;
        }
        /// <summary>
        /// 错误类型
        /// </summary>
        public FrameIOErrorType ErrType { get; set; }
        /// <summary>
        /// 出现错误的位置信息描述 驱动类型 ID号/数据帧与字段信息等
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 错误信息描述
        /// </summary>
        public string ErrInfo { get; set; }
        
    }

    /// <summary>
    /// FrameIO库错误分类
    /// </summary>
    public enum FrameIOErrorType
    {
        /// <summary>
        /// 通道类型错误
        /// </summary>
        ChannelErr,
        /// <summary>
        /// 发送数据帧错误
        /// </summary>
        SendErr,
        /// <summary>
        /// 接收数据帧错误
        /// </summary>
        RecvErr,
        /// <summary>
        /// 接收数据帧后验证出错
        /// </summary>
        CheckDtaErr
    }
}
