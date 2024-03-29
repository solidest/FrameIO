﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal class Helper
    {
        public const ulong FULL = 0xFFFFFFFFFFFFFFFF;

        static internal IExpRun GetExp(JToken t)
        {
            if (t == null) return null;

            switch (t.Type)
            {
                case JTokenType.Object:
                    var p = (JProperty)((JObject)t).First;
                    switch (p.Name)
                    {
                        case SegRunBase.EXPSIZEOF_TOKEN:
                            return new ExpByteSizeOf(p.Value.Value<string>());
                        case SegRunBase.EXPADD_TOKEN:
                            return new ExpCalc(ExpCalcType.EXP_ADD, GetExp(p.Value.Value<JArray>()[0]), GetExp(p.Value.Value<JArray>()[1]));
                        case SegRunBase.EXPSUB_TOKEN:
                            return new ExpCalc(ExpCalcType.EXP_SUB, GetExp(p.Value.Value<JArray>()[0]), GetExp(p.Value.Value<JArray>()[1]));
                        case SegRunBase.EXPMUL_TOKEN:
                            return new ExpCalc(ExpCalcType.EXP_MUL, GetExp(p.Value.Value<JArray>()[0]), GetExp(p.Value.Value<JArray>()[1]));
                        case SegRunBase.EXPDIV_TOKEN:
                            return new ExpCalc(ExpCalcType.EXP_DIV, GetExp(p.Value.Value<JArray>()[0]), GetExp(p.Value.Value<JArray>()[1]));
                    }

                    break;
                case JTokenType.Integer:
                    return new ExpLongValue(t.Value<long>());
                case JTokenType.Float:
                    return new ExpDoubleValue(t.Value<double>());
                case JTokenType.String:
                    return new ExpIdValue(t.Value<string>());
            }
            throw new Exception("unknow");

        }



        static internal ByteOrderTypeEnum GetByteOrder(JObject o)
        {
            return (ByteOrderTypeEnum)Enum.Parse(typeof(ByteOrderTypeEnum), o[SegRunBase.BYTEORDERT_TOKEN].Value<string>());
        }

        static internal EncodedTypeEnum GetEncoded(JObject o)
        {
            return (EncodedTypeEnum)Enum.Parse(typeof(EncodedTypeEnum), o[SegRunBase.ENCODED_TOKEN].Value<string>());
        }

        static internal CheckTypeEnum GetCheckType(JToken v)
        {
            return (CheckTypeEnum)Enum.Parse(typeof(CheckTypeEnum), v.Value<string>());
        }


    }
}
