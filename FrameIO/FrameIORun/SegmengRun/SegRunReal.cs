using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        internal override int BitLen { get => _isdouble?64:32; }

        #region --Initial--


        //从json加载内容
        static internal SegRunReal NewSegReal(JObject o, string name, bool isArray)
        {
            var ret = new SegRunReal();
            ret.Name = name;
            ret.InitialFromJson(o);
            if (isArray) ret.InitialArray(o);
            return ret;
        }

        protected override void InitialFromJson(JObject o)
        {
            _isdouble = (o[REALTYPE_TOKEN].Value<string>() == DOUBLE_TOKEN);
            _encoded = Helper.GetEncoded(o);
            _byteorder = Helper.GetByteOrder(o);
            _value = Helper.GetExp(o[VALUE_TOKEN]);
            _valid.AddMaxValidate(o);
            _valid.AddMinValidate(o);
        }

        #endregion

        #region --Pack--


        internal override ulong GetRaw(IFrameWriteBuffer buff, JValue jv)
        {
            double d = jv.Value<double>();

            var v = BitConverter.ToUInt64(BitConverter.GetBytes(d), 0);

            if(d < 0 && _encoded!= EncodedTypeEnum.Primitive)
            {
                v = (_encoded == EncodedTypeEnum.Complement ? GetComplement(v) : GetInversion(v));
            }
            
            if(_byteorder == ByteOrderTypeEnum.Big)
            {
                v = GetBigOrder(v);
            }

            return v;
        }


        internal override JValue GetAutoValue(IFrameWriteBuffer buff, JObject parent)
        {
            Debug.Assert(_value != null);
            double d = _value.GetDouble(parent, Parent);
            return new JValue(d);
        }

        #endregion


        #region --Unpack--

        internal override object FromRaw(ulong v)
        {
            if (_byteorder == ByteOrderTypeEnum.Big)
            {
                v = GetBigOrder(v);
            }

            if (_encoded != EncodedTypeEnum.Primitive)
            {
                v = (_encoded == EncodedTypeEnum.Complement ? GetComplement(v) : GetInversion(v));
            }

            if (_isdouble)
                return BitConverter.ToDouble(BitConverter.GetBytes(v), 0);
            else
                return BitConverter.ToSingle(BitConverter.GetBytes(v), 0);

        }

        protected override void DoValid(IFrameReadBuffer buff, SegRunValue seg, JToken value)
        {
            _valid.Valid(buff, seg, value);
        }


        #endregion


    }
}
