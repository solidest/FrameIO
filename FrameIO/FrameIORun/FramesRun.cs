using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //所有运行时数据帧
    public class FramesRun
    {

        private Dictionary<string, SegRunFrame> _frms ;

        public FramesRun()
        {
            _frms = new Dictionary<string, SegRunFrame>();
        }

        //从json字符串加载全部数据帧
        public static FramesRun LoadFromJson(string json)
        {
            var jfrms = JObject.Parse(json)[SegRunBase.FRAMELIST_TOKEN].Value<JArray>();
            var ret = new FramesRun();
            foreach(JObject vo in jfrms)
            {
                var p = (JProperty)vo.First;
                var o = p.Value.Value<JObject>();
                ret._frms.Add(p.Name, SegRunFrame.LoadFromJson(o, p.Name));
            }

            return ret;
        }
    }
}
