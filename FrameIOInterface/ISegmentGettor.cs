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
        /// 获取字段的bool值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        bool GetBool(ushort segidx);

        /// <summary>
        /// 获取字段的byte值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        byte GetByte(ushort segidx);


        /// <summary>
        /// 获取字段的sbyte值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        sbyte GetSByte(ushort segidx);

        /// <summary>
        /// 获取字段的short值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        short GetShort(ushort segidx);

        /// <summary>
        /// 获取字段的ushort值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        ushort GetUShort(ushort segidx);

        /// <summary>
        /// 获取字段的int值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        int GetInt(ushort segidx);

        /// <summary>
        /// 获取字段的uint值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        uint GetUInt(ushort segidx);

        /// <summary>
        /// 获取字段的long值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        long GetLong(ushort segidx);

        /// <summary>
        /// 获取字段的ulong值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        ulong GetULong(ushort segidx);

        /// <summary>
        /// 获取字段的float值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        float GetFloat(ushort segidx);


        /// <summary>
        /// 获取字段的double值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        double GetDouble(ushort segidx);

        /// <summary>
        /// 获取字段的float数组值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        float[] GetFloatArray(ushort segidx);


        /// <summary>
        /// 获取字段的double数组值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        double[] GetDoubleArray(ushort segidx);

        /// <summary>
        /// 获取字段的bool数组值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        bool[] GetBoolArray(ushort segidx);

        /// <summary>
        /// 获取字段的byte数组值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        byte[] GetByteArray(ushort segidx);


        /// <summary>
        /// 获取字段的sbyte数组值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        sbyte[] GetSByteArray(ushort segidx);

        /// <summary>
        /// 获取字段的short数组值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        short[] GetShortArray(ushort segidx);

        /// <summary>
        /// 获取字段的ushort数组值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        ushort[] GetUShortArray(ushort segidx);

        /// <summary>
        /// 获取字段的int数组值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        int[] GetIntArray(ushort segidx);

        /// <summary>
        /// 获取字段的uint数组值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        uint[] GetUIntArray(ushort segidx);

        /// <summary>
        /// 获取字段的long数组值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        long[] GetLongArray(ushort segidx);

        /// <summary>
        /// 获取字段的ulong数组值
        /// </summary>
        /// <param name="segidx">字段的号</param>
        /// <returns>返回的字段值</returns>
        ulong[] GetULongArray(ushort segidx);
    }

}
