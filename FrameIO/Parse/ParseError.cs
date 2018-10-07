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
        public int FirstLine { get; set; }
        public int FirstCol { get; set; }
        public int LastLine { get; set; }
        public int LastCol { get; set; }
        public string ErrorInfo { get; set; }
        public string ErrorTip
        {
            get
            {
                var str = GetErrorStr();
                //if (str == "UNKNOW")
                {
                    return "---ErrorCode" + ErrorCode.ToString() + "--- " + str;
                }
                //else
                //  return str;
            }
        }

        private string GetErrorStr()
        {
            switch (ErrorCode)
            {
                case 0:
                    return "开始位置出现错误";
                case -1:
                    return "拼写错误";
                case -2:
                    return "语法错误";
                case 1:
                    return "枚举名称重复";
                case 2:
                    return "枚举组成项名称重复";
                case 3:
                    return "分系统名称重复";
                case 4:
                    return "通道名称重复";
                case 5:
                    return "通道参数重复设置";
                case 6:
                    return "操作名称重复";
                case 7:
                    return "字段值重复设置";
                case 8:
                    return "属性名称重复";
                case 9:
                    return "数据帧名称重复";
                case 10:
                    return "字段名称重复";
                case 11:
                    return "字段属性重复设置";
                case 12:
                    return "字段属性与字段类型不匹配";
                case 13:
                    return "OneOf选择项名称重复";

                case 100:
                    return ErrorInfo;
                
                default:
                    return "UNKNOW";
            }
        }
    }
}
