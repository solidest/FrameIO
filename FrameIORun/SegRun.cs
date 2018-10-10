using FrameIO.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    public class SegRun
    {
        public SegBlockType ValueType { get; set; }
        public bool IsArray { get; set; }
        public ulong NumberValue { get; protected set; }
        public byte[] TextValue { get; protected set; }
        public ulong[] NumberArrayValue { get; protected set; }
        public byte[][] TextArrayValue { get; protected set; }
        public SegBlockInfo RefSegBlock { get; set; }
        public ExpRun BitSize { get; protected set; }
        public ExpRun Repeated { get; protected set; }

        public int BitStart { get; protected set; } = -1; //内存中的开始比特位置


        //取任意位的字节
        static public ulong GetUInt64FromByte(byte[] buff, uint bitStart)
        {
            uint word_index = bitStart >> 6;
            uint word_offset = bitStart & 63;
            ulong result = BitConverter.ToUInt64(buff, (int)word_index * 8) >> (UInt16)word_offset;
            uint bits_taken = 64 - word_offset;
            if (word_offset > 0 && bitStart + bits_taken < (uint)(8 * buff.Length))
            {
                result |= BitConverter.ToUInt64(buff, (int)(word_index + 1) * 8) << (UInt16)(64 - word_offset);
            }
            return result;
        }

        //取任意位的指定长度字节
        static public ulong GetUIntxFromByte(byte[] buff, uint bitStart, int x, EncodedType et, ByteOrderType ot)
        {
            return GetUInt64FromByte(buff, bitStart) & ((x != 0) ? (~(ulong)0 >> (sizeof(ulong) * 8 - x)) : (ulong)0);
            //TODO 处理反码与补码及大小端序
        }


        //取字段前缀名
        static protected string GetSegPreName(SegTreeInfo segi)
        {
            segi = segi.Parent;
            if (segi == null) return "";
            return GetSegFullName(segi);
        }

        //取字段全名
        static protected string GetSegFullName(SegTreeInfo segi)
        {
            var ret = segi.Name;
            while (segi.Parent != null)
            {
                ret = segi.Parent.Name + "." + ret;
                segi = segi.Parent;
            }
            return ret;
        }

        //转换为有符号数
        static public long ConvertToLong(ulong v, int bitsize)
        {
            if (bitsize == 64 || bitsize == 1)
                return (long)v;
            else
            {
                if ((v & ((ulong)1 << (bitsize - 1))) != 0)
                {
                    return (long)(~(ulong)0 <<bitsize | v);
                }
                else
                    return (long)v;
            }
        }
    }
}
