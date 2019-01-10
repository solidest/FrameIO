using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{

    internal delegate void PackHandler(IFrameWriteBuffer buff, JObject parent, JToken theValue);
    internal delegate int GetBitLenHandler(JObject parent, JToken theValue);

    //数组字段
    internal static class SegRunArray
    {

        public static void Pack(IExpRun arrLen, PackHandler ph, SegRunBase me, IFrameWriteBuffer buff, JObject parent, JArray vs)
        {
            int len = arrLen.GetInt(parent, me);
            if (vs == null) vs = new JArray();

            for (int i = 0; i < len; i++)
            {
                if (i == vs.Count)
                {
                    var dv = me.GetDefaultValue();
                    if (dv == null)
                    {
                        me.LogError(Interface.FrameIOErrorType.SendErr, "数组长度不匹配");
                        return;
                    }
                    vs.Add(dv);
                }

                ph(buff, parent, vs[i]);
            }
        }

        public static int GetBitLen(IExpRun arrLen, GetBitLenHandler gh, SegRunBase me, JObject parent)
        {
            int len = arrLen.GetInt(parent, me);
            var vs = parent?[me.Name]?.Value<JArray>();

            int ret = 0;
            for (int i = 0; i < len; i++)
            {
                var v = (vs?.Count ?? 0) > i ? vs[i] : null;
                ret += gh(parent, v);
            }
            return ret;
        }

    }
}
