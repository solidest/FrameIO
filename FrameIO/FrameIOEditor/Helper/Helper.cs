using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FrameIO.Main
{
    public partial class Helper
    {


        public static IList<string> GetFrameSegmentsName(string name, ICollection<Frame> pjfrms)
        {
            var ret = new List<string>();
            var frms = pjfrms.Where(p => p.Name == name);
            if (frms == null || frms.Count() == 0) return ret;
            var frm = frms.First();
            foreach (var seg in frm.Segments)
            {
                AddSegName(ret, "", seg, pjfrms);
            }
            return ret;
        }

        private static void AddSegName(List<string> segnames, string pre, FrameSegmentBase seg, ICollection<Frame> pjfrms)
        {
            if (segnames.Count > 1000) return;
            var ty = seg.GetType();
            if (ty == typeof(FrameSegmentInteger))
                segnames.Add((pre == "" ? "" : (pre + ".")) + seg.Name);
            else if (ty == typeof(FrameSegmentReal))
                segnames.Add((pre == "" ? "" : (pre + ".")) + seg.Name);
            else if (ty == typeof(FrameSegmentBlock))
            {
                var mypre = (pre == "" ? "" : (pre + ".")) + seg.Name;
                var bseg = (FrameSegmentBlock)seg;
                switch (bseg.UsedType)
                {
                    case BlockSegType.RefFrame:
                        {
                            var mylist = GetFrameSegmentsName(bseg.RefFrameName, pjfrms);
                            segnames.AddRange(mylist.Select(p => mypre + "." + p));
                            return;
                        }
                    case BlockSegType.DefFrame:
                        foreach (var myseg in bseg.DefineSegments)
                            AddSegName(segnames, mypre, myseg, pjfrms);
                        return;
                    case BlockSegType.OneOf:
                        foreach (var item in bseg.OneOfCaseList)
                        {
                            var itempre = mypre + "." + item.EnumItem;
                            var mylist = GetFrameSegmentsName(item.FrameName, pjfrms);
                            segnames.AddRange(mylist.Select(p => itempre + "." + p));
                        }
                        return;
                }
            }
        }

        //验证标识名称是否有效
        static public string ValidId(string s)
        {
            var reg = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*$");
            var match = reg.Match(s);
            if(match.Success) return "";
            return "请输入有效的名称";
        }

        //验证整数输入是否有效
        static public string ValidInteger(string s)
        {
            if (ValidateIsInt(s)) return "";
            return "请输入有效的整数";
        }

        //验证小数输入是否有效
        static public string ValidReal(string s)
        {
            if (ValidateIsReal(s)) return "";
            return "请输入有效的小数";
        }

        //验证常量数值输入是否有效
        static public string ValidConstNumber(string s)
        {
            if (ValidateIsReal(s)) return "";
            if (ValidateIsInt(s)) return "";
            return "请输入有效的数值";
        }

        static public bool ValidateIsInt(string s)
        {
            var reg = new Regex(@"^0[0-7]*$");
            if (reg.Match(s).Success) return true;

            reg = new Regex(@"^-?[1-9][0-9]*$");
            if (reg.Match(s).Success) return true;

            reg = new Regex(@"^0[Xx][0-9a-fA-F]+$");
            if (reg.Match(s).Success) return true;

            return false;
        }

        static public bool ValidateIsReal(string s)
        {
            var reg = new Regex(@"^-?([0-9]*\.[0-9]+|[0-9]+\.)([Ee][-+]?[0-9]+)?$");
            if (reg.Match(s).Success) return true;

            reg = new Regex(@"^[0-9]+([Ee][-+]?[0-9]+)$");
            if (reg.Match(s).Success) return true;

            return false;
        }


        //取任意位的字节
        static public UInt64 GetUInt64FromByte(byte[] buff, uint bitStart)
        {
            uint word_index = bitStart >> 6;
            uint word_offset = bitStart & 63;
            ulong result = BitConverter.ToUInt64(buff,(int)word_index*8) >> (UInt16)word_offset;
            uint bits_taken = 64 - word_offset;
            if (word_offset > 0 && bitStart + bits_taken < (uint)(8*buff.Length))
            {
                result |= BitConverter.ToUInt64(buff, (int)(word_index+1)*8) << (UInt16)(64 - word_offset);
            }
            return result;
        }

        //取任意位的指定长度字节
        static public UInt64 GetUIntxFromByte(byte[] buff, uint bitStart, int x)
        {
            return GetUInt64FromByte(buff, bitStart) & ((x!=0) ? (~(ulong)0>>(sizeof(ulong)*8-x)):(ulong)0);
        }

    }
}
