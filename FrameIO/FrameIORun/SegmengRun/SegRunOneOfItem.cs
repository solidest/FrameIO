﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //One of 的字段组 case分支
    internal class SegRunOneOfItem : SegRunGroup
    {
        private long? _byvalue;

        #region --Initial--

        //从json加载内容
        static public SegRunOneOfItem NewOneOfItem(JObject o, string name)
        {
            var ret = new SegRunOneOfItem();
            ret.Name = name;
            ret.InitialFromJson(o);
            return ret;
        }

        protected override void InitialFromJson(JObject o)
        {
            base.InitialFromJson(o);
            _byvalue = o[ONEOFBYVALUE_TOKEN].Value<long?>();
        }

        #endregion

        #region --Helper--

        internal protected bool IsDefault { get => _byvalue == null; }

        internal protected long ByValue { get => _byvalue??0;  }


        #endregion

    }
}
