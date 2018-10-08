using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //计算表达式
    [Serializable]
    public class Exp
    {
        public exptype Op { get; set; }
        public Exp LeftExp { get; set; }
        public Exp RightExp { get; set; }
        public string ConstStr { get; set; }

        //是否为常量
        public bool IsConst()
        {
            switch(Op)
            {
                case exptype.EXP_INT:
                case exptype.EXP_REAL:
                    return true;
                case exptype.EXP_ADD:
                case exptype.EXP_SUB:
                case exptype.EXP_MUL:
                case exptype.EXP_DIV:
                    return LeftExp.IsConst() && RightExp.IsConst();
                case exptype.EXP_BYTESIZEOF:
                case exptype.EXP_ID:
                    return false;
            }
            Debug.Assert(false);
            return false;
        }

        public bool CanEval(IList<string> varlist)
        {
            switch (Op)
            {
                case exptype.EXP_INT:
                    return true;
                case exptype.EXP_REAL:
                    return true;
                case exptype.EXP_ADD:
                case exptype.EXP_SUB:
                case exptype.EXP_MUL:
                case exptype.EXP_DIV:
                    return LeftExp.CanEval(varlist) && RightExp.CanEval(varlist);
                case exptype.EXP_BYTESIZEOF:
                case exptype.EXP_ID:
                    return varlist.Contains(ConstStr);
            }
            Debug.Assert(false);
            return false;
        }


        public double GetConstValue()
        {
            switch (Op)
            {
                case exptype.EXP_INT:
                    return Convert.ToInt32(ConstStr);
                case exptype.EXP_REAL:
                    return Convert.ToDouble(ConstStr);
                case exptype.EXP_ADD:
                    return LeftExp.GetConstValue() + RightExp.GetConstValue();
                case exptype.EXP_SUB:
                    return LeftExp.GetConstValue() - RightExp.GetConstValue();
                case exptype.EXP_MUL:
                    return LeftExp.GetConstValue() * RightExp.GetConstValue();
                case exptype.EXP_DIV:
                    return LeftExp.GetConstValue() / RightExp.GetConstValue();
            }
            Debug.Assert(false);
            return -1;
        }
    }
}
