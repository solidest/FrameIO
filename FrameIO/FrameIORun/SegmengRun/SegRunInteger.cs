using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //整数字段
    internal class SegRunInteger : SegRunValue
    {
        private bool _signed;
        private int _bitcount;
        private ByteOrderTypeEnum _byteorder;
        private EncodedTypeEnum _encoded;
        private IExpRun _value;
        private Validete _valid = new Validete();

        internal override SegRunContainer Parent { get; set; }
        internal override SegRunBase Next { get; set; }
        internal override SegRunBase Previous {  get; set;  }
        internal override SegRunBase First {  get; set;  }
        internal override SegRunBase Last {  get; set;  }
        internal override SegRunContainer Root {  get; set;  }
        internal override string Name { get; set; }
        internal override int BitLen { get => _bitcount; }

        #region --Initial--

        //从json加载内容
        static internal SegRunInteger LoadFromJson(JObject o, string name, SegRunContainer parent)
        {
            var ret = new SegRunInteger();
            ret.Name = name;
            ret.Parent = parent;
            ret.FillFromJson(o);
            return ret;
        }

        protected internal override void FillFromJson(JObject o)
        {
            _signed = o[SIGNED_TOKEN].Value<bool>();
            _bitcount = o[BITCOUNT_TOKEN].Value<int>();
            _encoded = Helper.GetEncoded(o);
            _byteorder = Helper.GetByteOrder(o);
            _value = Helper.GetValueExp(o);
            _valid.AddMaxValidate(o);
            _valid.AddMinValidate(o);
            _valid.AddCheckValidate(o);
        }


        #endregion


        #region --Pack--


        internal override ulong GetBuffer(JValue value)
        {
            throw new NotImplementedException();
            //HACK
        }

        internal override SegRunBase Pack(FramePackBuffer buff, JToken value)
        {

            buff.DoWriteValue(this, value?.Value<JValue>());
            return Next;
        }


        #endregion
    }
}
