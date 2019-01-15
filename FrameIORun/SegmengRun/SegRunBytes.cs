using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FrameIO.Run.SegmengRun
{
    //原始字节流字段
    internal class SegRunBytes :  SegRunValue
    {

        private IExpRun _byteLen;
        private string _value;

        public override int BitLen => 8;


        #region --Initial--

        private SegRunBytes()
        {

        }

        public static SegRunBytes NewSegBytes(JObject o, bool isArray)
        {
            var ret = new SegRunBytes();
            ret._byteLen = new ExpLongValue(1);
            ret._value = "";
            if (isArray) ret.ArrayLen = Helper.GetExp(o[ARRAYLEN_TOKEN]);
            return ret;
        }


        #endregion


        public override void Pack(IFrameWriteBuffer buff, JObject parent, JToken theValue)
        {
            buff.Write(Encoding.Default.GetBytes(theValue.Value<string>()), theValue);
        }

        public override JToken GetDefaultValue()
        {
            return new JValue("");
        }

        public override JToken GetAutoValue(IFrameWriteBuffer buff, JObject parent)
        {
            if(_value == null)
            {
                LogError(Interface.FrameIOErrorType.SendErr, "未赋值");
                return GetDefaultValue();
            }
            return new JValue(_value);
        }

        public override int GetBitLen(JObject parent)
        {
            return _byteLen.GetInt(parent, this) * 8;
        }

        public override JValue UnpackValue(IFrameReadBuffer buff, JContainer pc)
        {
            var ret = (JValue)GetDefaultValue();
            var ctx = IsArray ? (JObject)pc.Parent : (JObject)pc;
            ret.Value = Encoding.Default.GetString(buff.ReadBytes(_byteLen.GetInt(ctx, this), ret));

            if(IsArray)
            {
                pc.Add(ret);
            }
            else
            {
                ctx.Add(Name, ret);
            }
            return ret;
        }
    }
}
