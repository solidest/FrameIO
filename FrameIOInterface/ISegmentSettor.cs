using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Interface
{
    /// <summary>
    /// 用于设置数据帧字段值的接口
    /// </summary>
    public interface ISegmentSettor
    {
        /// <summary>
        /// 获取数据帧打包接口
        /// </summary>
        /// <returns>数据帧打包接口</returns>
        IFramePack GetPack();

        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, bool? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, byte? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, sbyte? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, ushort? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, short? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, uint? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, int? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, ulong? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, long? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, float? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, double? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, bool?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, byte?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, sbyte?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, ushort?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, short?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, uint?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, int?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, ulong?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, long?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, float?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segidx">字段编号</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(ushort segidx, double?[] value);
    }
}
