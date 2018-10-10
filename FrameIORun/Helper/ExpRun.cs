using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Main;

namespace FrameIO.Run
{
    public class ExpRun : Exp
    {

        public ExpRun LeftExpRun { get; set; }
        public ExpRun RightExpRun { get; set; }
        public string FullId { get; private set; }

        //运行时id
        private ExpRun()
        {
            
        }

        public static ExpRun CreateExpRun(Exp ep, string preid)
        {
            if (ep == null) return null;
            var ret = new ExpRun();
            ret.Op = ep.Op;
            ret.ConstStr = ep.ConstStr;
            ret.LeftExpRun = CreateExpRun(ep.LeftExp, preid);
            ret.RightExpRun = CreateExpRun(ep.RightExp, preid);

            switch (ret.Op)
            {
                case exptype.EXP_BYTESIZEOF:
                    if (ret.ConstStr == "this")
                        ret.FullId = preid;
                    else
                        ret.FullId = (preid == "" ? "" : (preid + ".")) + ret.ConstStr;
                    break;
                case exptype.EXP_ID:
                    ret.FullId = (preid == "" ? "" : (preid + ".")) + ret.ConstStr;
                    break;
            }
            return ret;
        }


        //计算数值
        public double GetRealValue(IFrameRun ib)
        {
            switch (Op)
            {
                case exptype.EXP_INT:
                    return Convert.ToInt32(ConstStr);
                case exptype.EXP_REAL:
                    return Convert.ToDouble(ConstStr);
                case exptype.EXP_ADD:
                    return LeftExpRun.GetRealValue(ib) + RightExpRun.GetRealValue(ib);
                case exptype.EXP_SUB:
                    return LeftExpRun.GetRealValue(ib) - RightExpRun.GetRealValue(ib);
                case exptype.EXP_MUL:
                    return LeftExpRun.GetRealValue(ib) * RightExpRun.GetRealValue(ib);
                case exptype.EXP_DIV:
                    return LeftExpRun.GetRealValue(ib) / RightExpRun.GetRealValue(ib);
                case exptype.EXP_ID:
                    return ib.GetSegValue(FullId);
                case exptype.EXP_BYTESIZEOF:
                    return ib.ByteSizeOf(FullId);

            }
            Debug.Assert(false);
            return -1;
        }
    }
}
