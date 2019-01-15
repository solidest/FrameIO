using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal abstract class SegRunValue : SegRunBase, ISegRun
    {
        public override bool IsArray => ArrayLen != null;

        protected IExpRun ArrayLen { get; set; }


        public abstract JValue UnpackValue(IFrameReadBuffer buff, JContainer pc);

        //按位计算的长度
        public abstract int BitLen { get; }


        //自下而上查找
        public bool LookUpNextValueSeg(out SegRunValue nextSeg, out JContainer pc, out int repeated, JContainer thePc, SegRunValue theSeg)
        {
            var p = IsArray ? ((JObject)thePc.Parent.Parent) : (JObject)thePc;
            if (Next != null)
                return Next.LookUpFirstValueSeg(out nextSeg, out pc, out repeated, p, p[Next.Name]);
            else
                return Parent.LookUpNextValueSeg(out nextSeg, out pc, out repeated, p);
        }

        //自上而下查找
        public override bool LookUpFirstValueSeg(out SegRunValue firstSeg, out JContainer pc, out int repeated, JObject ctx, JToken theValue)
        {
            firstSeg = this;
            if (IsArray && !ArrayLen.CanCalc(ctx, this))
            {
                pc = null;
                repeated = 0;
                return false;
            }

            if (IsArray)
            {
                repeated = ArrayLen.GetInt(ctx, this);
                var mypc = new JArray();
                ctx.Add(Name, mypc);
                pc = mypc;
            }
            else
            {
                repeated = 1;
                pc = ctx;
            }

            return true;
        }
    }
}
