using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //整数字段
    internal class SegRunInteger : SegRunNumber
    {
        private bool _signed;
        private int _bitcount;
        private ByteOrderTypeEnum _byteorder;
        private EncodedTypeEnum _encoded;
        private IExpRun _value;
        private Validete _valid = new Validete();
        private SegmentCheckValidator _check;


        public override int BitLen { get => _bitcount; }

        #region --Initial--

        //从json初始化
        static public SegRunInteger NewSegInteger(JObject o, string name, bool isArray)
        {
            var ret = new SegRunInteger();
            ret.Name = name;
            if (isArray)
                ret.ArrayLen = Helper.GetExp(o[ARRAYLEN_TOKEN]);
            ret.InitialFromJson(o);
            return ret;
        }

        protected void InitialFromJson(JObject o)
        {
            _signed = o[SIGNED_TOKEN].Value<bool>();
            _bitcount = o[BITCOUNT_TOKEN].Value<int>();
            if (o.ContainsKey(ENCODED_TOKEN)) _encoded = Helper.GetEncoded(o);
            if (o.ContainsKey(BYTEORDERT_TOKEN)) _byteorder = Helper.GetByteOrder(o);
            if (o.ContainsKey(VALUE_TOKEN)) _value = Helper.GetExp(o[VALUE_TOKEN]);
            if (o.ContainsKey(MAXVALUE_TOKEN)) _valid.AddMaxValidate(o[MAXVALUE_TOKEN]);
            if (o.ContainsKey(MINVALUE_TOKEN)) _valid.AddMinValidate(o[MINVALUE_TOKEN]);
            if (o.ContainsKey(CHECKFROM_TOKEN)) _check = _valid.AddCheckValidate(o[CHECKTYPE_TOKEN], o[CHECKFROM_TOKEN], o[CHECKTO_TOKEN] );
        }



        #endregion

        #region --Pack--


        internal override ulong GetRaw(IFrameWriteBuffer buff, JValue jv)
        {
            ulong ret = 0;

            if(_signed)
            {
                ret = ConvertToRaw(jv.Value<long>());
            }
            else
            {
                ret = jv.Value<ulong>();
            }

            if (_byteorder == ByteOrderTypeEnum.Big)
            {
                ret = GetBigOrder(ret);
            }

            return ret;
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

            if (_signed)
                return BitConverter.ToInt64(BitConverter.GetBytes(v), 0);
            else
                return BitConverter.ToUInt64(BitConverter.GetBytes(v), 0);

        }

        #endregion

        #region --Helper--

        internal ulong ConvertToRaw(long v)
        {
            if (v >= 0) return (ulong)v;

            var uv = BitConverter.ToUInt64(BitConverter.GetBytes(v), 0);
            if (_encoded != EncodedTypeEnum.Primitive) uv = (_encoded == EncodedTypeEnum.Complement ? GetComplement(uv) : GetInversion(uv));
            return uv;

        }

        protected override void DoValid(IFrameReadBuffer buff, SegRunNumber seg, JToken value)
        {
            _valid.Valid(buff, seg, value);

        }

        public override JToken GetDefaultValue()
        {
            return new JValue(0);
        }


        public override JToken GetAutoValue(IFrameWriteBuffer buff, JObject parent)
        {
            if(IsArray)
            {
                var ret = new JArray();
                for (int i = 0; i < ArrayLen.GetLong(parent, this); i++)
                {
                    ret.Add(new JValue(0));
                }
                return ret;
            }
            if (_check != null)
            {
                return new JValue(_check.GetCheckResult(buff, parent, Parent));
            }
            else if (_value != null)
            {
                return new JValue(_value.GetLong(parent, this));
            }
            else
                LogError(Interface.FrameIOErrorType.SendErr, "未赋值");
            return GetDefaultValue();
        }




        #endregion
    }
}
