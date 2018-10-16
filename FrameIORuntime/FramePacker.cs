using FrameIO.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    public class FramePacker : ISegmentSettor, IPackRunExp
    {
        private FrameInfo _fi;
        private SegmentValueInfo[] _segpi;
        private List<ulong> _data;
        public FramePacker(FrameInfo fi)
        {
            _fi = fi;
            _segpi = new SegmentValueInfo[fi.SegmentsCount];
            for(int i=0; i<_segpi.Length; i++) _segpi[i] = new SegmentValueInfo();
            _data = new List<ulong>();
        }


        public IFramePack GetPack()
        {
            IFramePack ret = null;
            try
            {
                using (var pdata = new MemoryStream())
                {
                    int pos = 1;
                    ulong buff = 0;
                    int oddlen = 0;
                    while(pos != _fi.SegmentsCount)
                    {
                        var result = _fi[pos].Pack(_data, pdata, ref buff, ref oddlen, _segpi[pos], this);
                        if (result == 0)
                            pos += 1;
                        else
                            pos = result;
                    }
                    pdata.Close();
                    ret = new DataPacker(pdata.ToArray());
                }
            }
            finally
            {
                _data.Clear();
                for (int i = 0; i < _segpi.Length; i++) _segpi[i].Reset();
            }

            return ret;
        }

        #region --SegSegmentValue--

        //设置字段的数据内容

        public void SetSegmentValue(ushort idx, bool? value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, byte? value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, sbyte? value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, ushort? value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, short? value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, uint? value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, int? value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, ulong? value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, long? value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, float? value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, double? value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, bool?[] value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, byte?[] value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, sbyte?[] value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, ushort?[] value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, short?[] value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, uint?[] value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, int?[] value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, ulong?[] value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, long?[] value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, float?[] value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }

        public void SetSegmentValue(ushort idx, double?[] value)
        {
            _fi[idx].SetSegmentValue(_data, value, _segpi[idx]);
        }


        #endregion

        #region --IRunExp--

        public int GetSegmentByteSize(ushort idx)
        {
            int len = 0;
            _fi[idx].GetBitLen(ref len, _segpi[idx], this);
            if (len % 8 != 0)
                throw new Exception("runtime");
            else
                return len / 8;
        }

        public ushort GetBitLen(ref int bitlen, ushort idx)
        {
            return _fi[idx].GetBitLen(ref bitlen, _segpi[idx], this);
        }

        public double GetSegmentValue(ushort idx)
        {
            return _fi[idx].GetValue(_data, _segpi[idx]);
        }

        public double GetExpValue(ushort idx)
        {
            return _fi.GetExp(idx).GetExpValue(this);
        }


        public double GetConst(ushort idx)
        {
            return _fi.GetConst(idx);
        }

        public bool IsConst(ushort idx)
        {
            return _fi.IsConst(idx);
        }

        public bool IsConstOne(ushort idx)
        {
            return _fi.IsConstOne(idx);
        }

        public SegmentValidator GetValidator(ushort idx, ValidateType type)
        {
            return _fi.GetValidator(idx,type);
        }

        #endregion

    }

    public struct SegmentValueInfo
    {
        public bool IsSetValue;
        public int StartPos;
        public int Count;
        public object Tag;
        public void Reset()
        {
            IsSetValue = false;
            StartPos = 0;
            Count = 0;
            Tag = null;
        }
    }
}
