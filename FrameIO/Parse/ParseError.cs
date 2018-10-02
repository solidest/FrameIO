using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class ParseError
    {
        public int ErrorCode { get; set; }
        public string FirstSymbol { get; set; }
        public string LastSymbol { get; set; }
        public int FirstLine { get; set; }
        public int FirstCol { get; set; }
        public int LastLine { get; set; }
        public int LastCol { get; set; }
        public string ErrorTip
        {
            get
            {
                var str = GetErrorStr();
                //if (str == "UNKNOW")
                {
                    return "Error(" + ErrorCode.ToString() + "):" + str;
                }
                //else
                //  return str;
            }
        }

        private string GetErrorStr()
        {
            switch (ErrorCode)
            {
                case -1:
                    return "拼写错误";
                case -2:
                    return "语法错误";
                case 1:
                    return "属性重复设置";
                case 101:
                    return "协议名称重复";
                case 102:
                    return "字段名称重复";
                case 2:
                    return "当前字段不支持该属性";
                case 3:
                    return "字段缺少必要的属性";
                case 4:
                    return "属性的错误赋值";
                case 5:
                    return "属性赋值类型匹配错误";
                case 6:
                    return "赋值类型与字段类型不匹配";
                case 7:
                    return "属性赋值与字段类型不匹配";
                default:
                    return "UNKNOW";
            }
        }
    }
}
