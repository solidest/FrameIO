using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace FrameIO.Main
{
    public static class Helper
    {


        //转大端序
        private static byte[] RevOrder(byte[] data)
        {
            var newv = new byte[data.Length];

            var oldi = data.Length;
            for (int i = 0; i < data.Length; i++)
            {
                newv[i] = data[oldi - 1];
                oldi -= 1;
            }
            return newv;
        }

        public static long ToLong(string str)
        {
            if (str.StartsWith("0x") || str.StartsWith("0X"))
            {
                return Convert.ToInt64(str, 16);
            }

            var ret= Convert.ToInt64(str);
            return ret;
        }

        public static ulong ToULong(string str)
        {
            if (str.StartsWith("0x") || str.StartsWith("0X"))
            {
                var ret = Convert.ToUInt64(str, 16); 
                return ret;
            }

            return Convert.ToUInt64(str);
        }

        //取数据帧基础字段名
        public static IList<string> GetFrameSegmentsName(string name, ICollection<Frame> pjfrms, bool notIntoSubsys = false)
        {
            var ret = new List<string>();
            var frms = pjfrms.Where(p => p.Name == name);
            if (frms == null || frms.Count() == 0) return ret;
            var frm = frms.First();
            foreach (var seg in frm.Segments)
            {
                AddSegName(frm, ret, "", seg, pjfrms,  notIntoSubsys);
            }
            return ret;
        }

        //转换整数字段类型到属性类型
        public static string ConvertISegType2ProType(int bitcount, bool isSigned)
        {
            if (bitcount == 1)
                return "bool";
            else if (bitcount > 1 && bitcount <= 8)
                return isSigned ? "sbyte" : "byte";
            else if (bitcount > 8 && bitcount <= 16)
                return isSigned ? "short" : "ushort";
            else if (bitcount > 16 && bitcount <= 32)
                return isSigned ? "int" : "uint";
            else if (bitcount > 32 && bitcount <= 64)
                return isSigned ? "long" : "ulong";
            return "";
        }

        //转换字段类型到属性类型
        public static string ConvertSegType2ProType(FrameSegmentBase seg)
        {
            var ret = "";
            var t = seg.GetType();
            if(t == typeof(FrameSegmentInteger))
            {
                var iseg = (FrameSegmentInteger)seg;
                ret = ConvertISegType2ProType(iseg.BitCount, iseg.Signed);
            }
            else if(t ==typeof(FrameSegmentReal))
            {
                var rseg = (FrameSegmentReal)seg;
                if (rseg.IsDouble)
                    ret = "double";
                else
                    ret = "float";
            }
            return ret;
        }

        private static void AddSegName(Frame frm, List<string> segnames, string pre, FrameSegmentBase seg, ICollection<Frame> pjfrms, bool notIntoSubsys = false)
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
                            var subname = GetSubSysNameFrom(bseg.RefFrameName, pjfrms);
                            if(notIntoSubsys && subname != "")
                            {
                                segnames.Add((pre == "" ? "" : (pre + ".")) + seg.Name);
                                return;
                            }
                            var mylist = GetFrameSegmentsName(bseg.RefFrameName, pjfrms, notIntoSubsys);
                            segnames.AddRange(mylist.Select(p => mypre + "." + p));
                            return;
                        }
                    case BlockSegType.DefFrame:
                        if(notIntoSubsys && bseg.SubSysName != null && bseg.SubSysName.Length > 0)
                        {
                            segnames.Add((pre == "" ? "" : (pre + ".")) + seg.Name);
                            return;
                        }
                        foreach (var myseg in bseg.DefineSegments)
                            AddSegName(frm, segnames, mypre, myseg, pjfrms, notIntoSubsys);
                        return;
                    case BlockSegType.OneOf:
                        foreach (var item in bseg.OneOfCaseList)
                        {
                            var itempre = mypre + "." + item.EnumItem;
                            var subname = GetSubSysNameFrom(item.FrameName, pjfrms);
                            if (notIntoSubsys && subname != "")
                            {
                                segnames.Add(itempre);
                            }
                            else
                            {
                                var mylist = GetFrameSegmentsName(item.FrameName, pjfrms, notIntoSubsys);
                                segnames.AddRange(mylist.Select(p => itempre + "." + p));
                            }

                        }
                        return;

                }
            }
        }

        public class OneOfHelper
        {
            public string ByEnum { get; set; } = "";
            public string BySegname { get; set; } = "";
            public string ByProperty { get; set; } = "";

            public List<OneOfItemHelper> Items { get; private set; } = new List<OneOfItemHelper>();
        }
        public class OneOfItemHelper
        {
            public string ItemName { get; set; }
            public List<string> Segmens { get; private set; } = new List<string>();
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


        //取数据帧对应的子系统名称
        public static string GetSubSysNameFrom(string name, ICollection<Frame> pjfrms)
        {
            var f = pjfrms.Where(p => p.Name == name);
            if (f.Count() == 0) return "";
            var fr = f.First();
            if (fr.SubSysName == null || fr.SubSysName.Length == 0) return "";
            return fr.SubSysName;
        }
    }
}
