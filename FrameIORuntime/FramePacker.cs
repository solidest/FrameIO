using FrameIO.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    public class FramePacker
    {
        private FrameInfo _fi;
        private SegmentPackInfo[] _segpis;
        private MemoryStream _data;
        public FramePacker(FrameInfo fi)
        {
            _fi = fi;
            _segpis = new SegmentPackInfo[fi.SegmentsCount];
            _data = new MemoryStream();
        }

        public byte[] Pack()
        {
            byte[] ret = null;
            try
            {
                using (var pdata = new MemoryStream())
                {
                    int pos = 1;
                    while(pos != _fi.SegmentsCount-1)
                    {
                        var result = _fi[pos].Pack(_data, pdata, _segpis[pos]);
                        if (result == 0)
                            pos += 1;
                        else
                            pos = result;
                    }
                    //打包数值
                    pdata.Close();
                    ret = pdata.ToArray();
                }

            }
            finally
            {
                _data.Seek(0, SeekOrigin.Begin);
                _segpis = new SegmentPackInfo[_fi.SegmentsCount];
            }
            return ret;
        }

        #region --SegSegmentValue--

        //设置字段的数据内容

        public void SetSegmentValue(int idx, bool? value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, byte? value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, sbyte? value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, ushort? value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, short? value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, uint? value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, int? value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, ulong? value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, long? value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, float? value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, double? value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, bool?[] value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, byte?[] value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, sbyte?[] value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, ushort?[] value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, short?[] value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, uint?[] value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, int?[] value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, ulong?[] value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, long?[] value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, float?[] value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        public void SetSegmentValue(int idx, double?[] value)
        {
            _segpis[idx] = _fi[idx].SetSegmentValue(_data, value);
        }

        #endregion

    }

    public struct SegmentPackInfo
    {
        public bool HaveSetValue;
        public int StartPos;
        public int ByteSize;
        public int OddBitCount;
    }
}
