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
    public class Helper
    {
        //取嵌入资源中的图片
        static public BitmapImage GetImage(string imgName)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("FrameIO.Main.img." + imgName);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = myStream;
            image.EndInit();
            myStream.Dispose();
            myStream.Close();
            return image;
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
            if (ValidateInt(s)) return "";
            return "请输入有效的整数";
        }

        //验证小数输入是否有效
        static public string ValidReal(string s)
        {
            if (ValidateRl(s)) return "";
            return "请输入有效的小数";
        }

        //验证常量数值输入是否有效
        static public string ValidConstNumber(string s)
        {
            if (ValidateRl(s)) return "";
            if (ValidateInt(s)) return "";
            return "请输入有效的数值";
        }

        static private bool ValidateInt(string s)
        {
            var reg = new Regex(@"^0[0-7]*$");
            if (reg.Match(s).Success) return true;

            reg = new Regex(@"^[1-9][0-9]*$");
            if (reg.Match(s).Success) return true;

            reg = new Regex(@"^0[Xx][0-9a-fA-F]+$");
            if (reg.Match(s).Success) return true;

            return false;
        }

        static private bool ValidateRl(string s)
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
