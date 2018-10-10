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

        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, bool? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, byte? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, sbyte? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, ushort? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, short? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, uint? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, int? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, ulong? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, long? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, float? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, double? value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, bool?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, byte?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, sbyte?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, ushort?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, short?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, uint?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, int?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, ulong?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, long?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, float?[] value);


        /// <summary>
        /// 设置数据帧字段的值
        /// </summary>
        /// <param name="segmentname">字段名称</param>
        /// <param name="value">字段值</param>
        void SetSegmentValue(string segmentname, double?[] value);

    }
}
