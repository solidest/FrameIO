﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //整数数组
    internal class SegRunIntegerArray : SegRunInteger
    {
        private IExpRun _arrlen;


        #region --Initial--

//从json加载内容
        new static internal SegRunIntegerArray LoadFromJson(JObject o, string name, SegRunContainer parent)
        {
            var ret = new SegRunIntegerArray();
            ret.Parent = parent;
            ret.Name = name;
            ret.FillFromJson(o);
            return ret;
        }

        internal protected override void FillFromJson(JObject o)
        {
            base.FillFromJson(o);
            _arrlen = Helper.GetExp(o[ARRAYLEN_TOKEN]);
        }

        #endregion
        


        #region --Pack--

        internal override SegRunBase Pack(FramePackBuffer buff, JToken value)
        {
            var vs = (value?.Value<JArray>()) ?? new JArray();
            for (int i = 0; i < Helper.GetInt(_arrlen, value, this); i++)
            {
                base.Pack(buff, i < vs.Count ? vs[i] : null);
            }

            return Next;
        }



        #endregion


    }
}
