using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{

    public abstract class SegmentBaseRun
    {
        protected FrameInfo Parent { get; private set; }
        public SegmentBaseRun(ulong token)
        {

        }
        public virtual FrameInfo Initial(FrameInfo parent)
        {
            return null;
        }

        public virtual ushort Pack(Stream vst, Stream pst, SegmentPackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual ushort Unpack(byte[] buff, ref int pos_bit, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        public virtual ushort TryUnpack(ushort next_fill_seg, SegmentUnpackInfo info)
        {
            throw new Exception("runtime");
        }

        #region --SetSegmentValue--


        public virtual SegmentPackInfo SetSegmentValue(Stream st, bool? value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, byte? value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, sbyte? value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, ushort? value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, short? value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, uint? value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, int? value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, ulong? value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, long? value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, float? value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, double? value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, bool?[] value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, byte?[] value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, sbyte?[] value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, ushort?[] value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, short?[] value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, uint?[] value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, int?[] value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, ulong?[] value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, long?[] value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, float?[] value)
        {
            throw new Exception("runtime");
        }

        public virtual SegmentPackInfo SetSegmentValue(Stream st, double?[] value)
        {
            throw new Exception("runtime");
        }

        #endregion

        #region --helper--

        public static byte GetTokenByte(ulong token, byte pos_tart, byte len)
        {
            return (byte)((token & (((~(ulong)0) << (64 - len)) >> (64 - len - pos_tart))) >> pos_tart);
        }

        public static ushort GetTokenUShort(ulong token, byte pos_tart)
        {
            return (ushort)((token & (((~(ulong)0) << (64 - 16)) >> (64 - 16 - pos_tart))) >> pos_tart);
        }

        public static bool GetTokenBool(ulong token, byte pos_tart)
        {
            return (token & ((ulong)1 << pos_tart)) != 0;
        }

        #endregion


    }
}
