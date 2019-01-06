using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //小数字段
    internal class SegRunReal : SegRunValue
    {
        private bool _isdouble;
        private ByteOrderTypeEnum _byteorder;
        private EncodedTypeEnum _encoded;
        private IExpRun _value;
        private Validete _valid = new Validete();

        internal override SegRunContainer Parent { get; set; }
        internal override SegRunBase Next { get; set; }
        internal override SegRunBase Previous { get; set; }
        internal override SegRunBase First { get; set; }
        internal override SegRunBase Last { get; set; }
        internal override SegRunContainer Root { get; set; }
        internal override string Name { get; set; }
        internal override int BitLen { get => _isdouble?64:32; }


        //从json加载内容
        static internal SegRunReal LoadFromJson(JObject o, string name, SegRunContainer parent)
        {
            var ret = new SegRunReal();
            ret.Parent = parent;
            ret.Name = name;
            ret.FillFromJson(o);
            return ret;
        }

        protected internal override void FillFromJson(JObject o)
        {
            _isdouble = (o[REALTYPE_TOKEN].Value<string>() == DOUBLE_TOKEN);
            _encoded = Helper.GetEncoded(o);
            _byteorder = Helper.GetByteOrder(o);
            _value = Helper.GetValueExp(o);
            _valid.AddMaxValidate(o);
            _valid.AddMinValidate(o);
        }

        internal override ulong GetBuffer(JValue value)
        {
            throw new NotImplementedException();
            //HACK
        }

        internal override SegRunBase Pack(FramePackBuffer buff, JToken value)
        {
            throw new NotImplementedException();
            //HACK
        }
    }
}
