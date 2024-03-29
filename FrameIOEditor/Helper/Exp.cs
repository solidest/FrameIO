﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //计算表达式
    public class Exp
    {
        
        public exptype Op { get; set; }
        public Exp LeftExp { get; set; }
        public Exp RightExp { get; set; }
        public string ConstStr { get; set; }

        //是否为常量1
        public bool IsIntOne()
        {
            return (Op == exptype.EXP_INT && ConstStr == "1");
        }

        //是否位常量0
        public bool IsIntZero()
        {
            return (Op == exptype.EXP_INT && ConstStr == "0");
        }

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

        //是否可以计算
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
                    return varlist.Contains(ConstStr) || ConstStr=="this";
            }
            Debug.Assert(false);
            return false;
        }

        //计算常量值
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

        public override string ToString()
        {
            switch (Op)
            {
                case exptype.EXP_INT:
                    return ConstStr;
                case exptype.EXP_REAL:
                    return ConstStr;
                case exptype.EXP_ID:
                    return ConstStr;
                case exptype.EXP_BYTESIZEOF:
                    return "bytesizeof(" + ConstStr + ")";
                case exptype.EXP_ADD:
                    return"(" + LeftExp.ToString() + "+" + RightExp.ToString() + ")";
                case exptype.EXP_SUB:
                    return "(" + LeftExp.ToString() + "-" + RightExp.ToString() + ")";
                case exptype.EXP_MUL:
                    return  LeftExp.ToString() + "*" + RightExp.ToString();
                case exptype.EXP_DIV:
                    return LeftExp.ToString() + "/" + RightExp.ToString();
            }

            return "";
        }

    }
}
