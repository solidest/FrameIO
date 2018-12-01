using FrameIO.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    internal class FramePacker : ISegmentSettor, IPackRunExp
    {
        private static FrameRuntime _fi;
        static FramePacker()
        {
            _fi = FrameRuntime.Run;
        }

        internal FramePacker(ushort startidx, ushort endidxi)
        {
            Info = new FramePackerInfo(startidx, endidxi);
        }

        internal FramePackerInfo Info { get; private set; }


        public IFramePack GetPack()
        {
            IFramePack ret = null;
            try
            {
                using (var pdata = new MemoryStream())
                {
                    ushort pos = (ushort)(Info.StartIdx + 1);
                    byte buff = 0;
                    byte oddlen = 0;
                    while (pos != Info.EndIdx)
                    {
                        var result = _fi[pos].Pack(Info.Cach, pdata, ref buff, ref oddlen, Info[pos], this);
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
                Info.Reset();
            }

            return ret;
        }

        public ISegmentSettor GetSubFrame(ushort idx)
        {
            return ((SegmentFrameRef)_fi[idx]).GetSegmentSettor(Info[idx]);
        }

        #region --SegSegmentValue--

        //设置字段的数据内容

        public void SetSegmentValue(ushort idx, bool? value)
        {
            _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, byte? value)
        {
            _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, sbyte? value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, ushort? value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, short? value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, uint? value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, int? value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, ulong? value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, long? value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, float? value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, double? value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, bool?[] value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, byte?[] value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, sbyte?[] value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, ushort?[] value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, short?[] value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, uint?[] value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, int?[] value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, ulong?[] value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, long?[] value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, float?[] value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }

        public void SetSegmentValue(ushort idx, double?[] value)
        {
             _fi[idx].SetSegmentValue(Info.Cach, value, Info[idx]);
        }


        #endregion

        #region --IRunExp--

        public int GetSegmentByteSize(ushort idx)
        {
            int len = 0;
            _fi[idx].GetBitLen(Info.Cach, ref len, Info[idx], this);
            if (len % 8 != 0)
                throw new Exception("runtime 数据帧字段未能整字节对齐");
            else
                return len / 8;
        }

        public ushort GetBitLen(MemoryStream value_buff, ref int bitlen, ushort idx)
        {
            return _fi[idx].GetBitLen(Info.Cach, ref bitlen, Info[idx], this);
        }

        public double GetSegmentValue(ushort idx)
        {
            return _fi[idx].GetValue(Info.Cach, Info[idx], this);
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

        public SetValueInfo GetSetValueInfo(ushort idx)
        {
            return Info[idx];
        }

        public double GetConstValue(ushort exp_idx)
        {
            return _fi.GetConstValue(exp_idx);
        }

        #endregion

    }

}
