using System;
using System.Collections.Generic;
using System.Text;

namespace FrameIO.Interface
{
    /// <summary>
    /// 数据帧的字段内容读取接口
    /// </summary>
    public interface IFrameData
    {

        /// <summary>
        /// 获取字段的bool值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        bool GetBool(string segmentname);

        /// <summary>
        /// 获取字段的byte值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        byte GetByte(string segmentname);


        /// <summary>
        /// 获取字段的sbyte值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        sbyte GetSByte(string segmentname);

        /// <summary>
        /// 获取字段的short值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        short GetShort(string segmentname);

        /// <summary>
        /// 获取字段的ushort值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        ushort GetUShort(string segmentname);

        /// <summary>
        /// 获取字段的int值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        int GetInt(string segmentname);

        /// <summary>
        /// 获取字段的uint值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        uint GetUInt(string segmentname);

        /// <summary>
        /// 获取字段的long值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        long GetLong(string segmentname);

        /// <summary>
        /// 获取字段的ulong值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        ulong GetULong(string segmentname);

        /// <summary>
        /// 获取字段的float值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        float GetFloat(string segmentname);


        /// <summary>
        /// 获取字段的double值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        double GetDouble(string segmentname);

        /// <summary>
        /// 获取字段的float数组值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        float[] GetFloatArray(string segmentname);


        /// <summary>
        /// 获取字段的double数组值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        double[] GetDoubleArray(string segmentname);

        /// <summary>
        /// 获取字段的bool数组值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        bool[] GetBoolArray(string segmentname);

        /// <summary>
        /// 获取字段的byte数组值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        byte[] GetByteArray(string segmentname);


        /// <summary>
        /// 获取字段的sbyte数组值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        sbyte[] GetSByteArray(string segmentname);

        /// <summary>
        /// 获取字段的short数组值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        short[] GetShortArray(string segmentname);

        /// <summary>
        /// 获取字段的ushort数组值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        ushort[] GetUShortArray(string segmentname);

        /// <summary>
        /// 获取字段的int数组值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        int[] GetIntArray(string segmentname);

        /// <summary>
        /// 获取字段的uint数组值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        uint[] GetUIntArray(string segmentname);

        /// <summary>
        /// 获取字段的long数组值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        long[] GetLongArray(string segmentname);

        /// <summary>
        /// 获取字段的ulong数组值
        /// </summary>
        /// <param name="segmentname">字段的全路径名称</param>
        /// <returns>返回的字段值</returns>
        ulong[] GetULongArray(string segmentname);
    }

}
