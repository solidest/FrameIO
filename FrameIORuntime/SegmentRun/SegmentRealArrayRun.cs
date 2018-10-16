﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    class SegmentRealArrayRun : SegmentRealRun
    {
        private ushort _repeated_idx;
        private int _repeated_const = -1;

        public SegmentRealArrayRun(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte pos_repeated = 16;
            _repeated_idx = GetTokenUShort(token, pos_repeated);
            if (_repeated_idx == 0) throw new Exception("runtime");
            if (ir.IsConst(_repeated_idx)) _repeated_const = (int)ir.GetConst(_repeated_idx);

        }


        #region --Pack--

        public override ushort Pack(IList<ulong> value_buff, MemoryStream pack, ref ulong cach, ref int pos, SegmentValueInfo info, IRunExp ir)
        {
            if (!info.IsSetValue)
            {
                info.IsSetValue = true;
                info.Count = 0;
            }

            int icount = info.Count;
            if (_repeated_const > 0 && _repeated_const < icount) icount = _repeated_const;
            for (int i = 0; i < icount; i++)
                CommitValue(value_buff[info.StartPos + i], pack, ref cach, ref pos, (byte)(IsDouble ? 64 : 32));

            if (_repeated_const > icount)
                for (int i = 0; i < _repeated_const - icount; i++)
                    CommitValue(0, pack, ref cach, ref pos, (byte)(IsDouble ? 64 : 32));

            return 0;
        }


        //取字段的字节大小
        public override ushort GetBitLen(ref int bitlen, SegmentValueInfo info, IRunExp ir)
        {
            if (_repeated_const > 0)
            { 
                bitlen += (IsDouble ? 64 : 32) * _repeated_const;
                return 0;
            }
            if (info.IsSetValue) 
            {
                bitlen += (IsDouble? 64 : 32) * info.Count;
                return 0;
            }
            return 0;
        }


        #endregion


        #region --SetValue--


        public override void SetSegmentValue(IList<ulong> value_buff, bool?[] value, SegmentValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = value_buff.Count;
            info.Count = value == null ? 0 : value.Length;
            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float)(value[i] == null ? 0 : ((bool)value[i] ? 1 : 0)));
            }
        }

        public override void SetSegmentValue(IList<ulong> value_buff, byte?[] value, SegmentValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = value_buff.Count;
            info.Count = value == null ? 0 : value.Length;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float?)value[i]);
            }
        }

        public override void SetSegmentValue(IList<ulong> value_buff, sbyte?[] value, SegmentValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = value_buff.Count;
            info.Count = value == null ? 0 : value.Length;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float?)value[i]);
            }
        }

        public override void SetSegmentValue(IList<ulong> value_buff, ushort?[] value, SegmentValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = value_buff.Count;
            info.Count = value == null ? 0 : value.Length;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float?)value[i]);
            }
        }

        public override void SetSegmentValue(IList<ulong> value_buff, short?[] value, SegmentValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = value_buff.Count;
            info.Count = value == null ? 0 : value.Length;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float?)value[i]);
            }
        }

        public override void SetSegmentValue(IList<ulong> value_buff, uint?[] value, SegmentValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = value_buff.Count;
            info.Count = value == null ? 0 : value.Length;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float?)value[i]);
            }
        }

        public override void SetSegmentValue(IList<ulong> value_buff, int?[] value, SegmentValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = value_buff.Count;
            info.Count = value == null ? 0 : value.Length;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (float?)value[i]);
            }
        }

        public override void SetSegmentValue(IList<ulong> value_buff, ulong?[] value, SegmentValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = value_buff.Count;
            info.Count = value == null ? 0 : value.Length;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (double?)value[i]);
            }
        }

        public override void SetSegmentValue(IList<ulong> value_buff, long?[] value, SegmentValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = value_buff.Count;
            info.Count = value == null ? 0 : value.Length;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, (double?)value[i]);
            }
        }

        public override void SetSegmentValue(IList<ulong> value_buff, float?[] value, SegmentValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = value_buff.Count;
            info.Count = value == null ? 0 : value.Length;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, value[i]);
            }
        }

        public override void SetSegmentValue(IList<ulong> value_buff, double?[] value, SegmentValueInfo info)
        {
            info.IsSetValue = true;
            info.StartPos = value_buff.Count;
            info.Count = value == null ? 0 : value.Length;

            for (int i = 0; i < info.Count; i++)
            {
                SetSegmentValue(value_buff, value[i]);
            }
        }

        #endregion


    }
}
