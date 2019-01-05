using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //表达式运行时
    internal class ExpRun
    {
        private ExpType _optype;
        private ExpRun _left;
        private ExpRun _right;
        private string _value;
    }

    internal enum ExpType
    {
        EXP_NUMBER = 1,
        EXP_ADD = 2,
        EXP_SUB = 3,
        EXP_MUL = 4,
        EXP_DIV = 5,
        EXP_REF_SEGMENT = 6,
        EXP_FUN_BYTESIZEOF = 7
    }

}
