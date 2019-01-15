using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;



    public class FrameObject : Interface.ISegmentGettor
    {
        public FrameObject()
        {
            RootValue = new JObject();
            FrameName = "";
        }

        internal FrameObject(string frameName)
        {
            RootValue = new JObject();
            FrameName = frameName;
        }


        internal string FrameName { get; private set; }

        private FrameObject(JObject o)
        {
            RootValue = o;
            FrameName = null;
        }

        internal JObject RootValue { get; }


        #region --SetValue--


        //设置数值型字段
        public void SetValue(string segname, object value)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = LookUpObject(segs);
            o.Add(new JProperty(segs[segs.Length - 1], value));
        }


        //设置数值型数组字段
        public void SetValueArray(string segname, IEnumerable values)
        {
            var vs = new JArray();
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = LookUpObject(segs);
            foreach (var ov in values)
            {
                vs.Add(ov);
            }
            o.Add(segs[segs.Length - 1], vs);
        }

        //设置对象字段
        public void SetObject(string segname, FrameObject ovalue)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = LookUpObject(segs);
            o.Add(segs[segs.Length - 1], ovalue.RootValue);
        }


        //设置对象数组字段
        public void SetObjectArray(string segname, IEnumerable<FrameObject> ovalues)
        {
            var vs = new JArray();
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = LookUpObject(segs);
            foreach (var ov in ovalues)
            {
                vs.Add(ov.RootValue);
            }
            o.Add(segs[segs.Length - 1], vs);
        }

        #endregion

        #region --GetValue

        public FrameObject GetObject(string segname)
        {
            return new FrameObject((JObject)FindToken(segname));
        }

        public IEnumerable<FrameObject> GetObjectArray(string segname)
        {
            var arr = (JArray)FindToken(segname);
            return arr.Select(p=>new FrameObject((JObject)p));
        }

        public bool GetBool(string segname)
        {
            return FindToken(segname).Value<bool>();
        }

        public byte GetByte(string segname)
        {
            return FindToken(segname).Value<byte>();
        }

        public sbyte GetSByte(string segname)
        {
            return FindToken(segname).Value<sbyte>();
        }

        public short GetShort(string segname)
        {
            return FindToken(segname).Value<short>();
        }

        public ushort GetUShort(string segname)
        {
            return FindToken(segname).Value<ushort>();
        }

        public int GetInt(string segname)
        {
            return FindToken(segname).Value<int>();
        }

        public uint GetUInt(string segname)
        {
            return FindToken(segname).Value<uint>();
        }

        public long GetLong(string segname)
        {
            return FindToken(segname).Value<long>();
        }

        public ulong GetULong(string segname)
        {
            return FindToken(segname).Value<ulong>();
        }

        public float GetFloat(string segname)
        {
            return FindToken(segname).Value<float>();
        }

        public double GetDouble(string segname)
        {
            return FindToken(segname).Value<double>();
        }


        public IEnumerable<bool> GetBoolArray(string segname)
        {
            return ((JArray)FindToken(segname)).Values<bool>();
        }

        public IEnumerable<byte> GetByteArray(string segname)
        {
            return ((JArray)FindToken(segname)).Values<byte>();
        }

        public IEnumerable<sbyte> GetSByteArray(string segname)
        {
            return ((JArray)FindToken(segname)).Values<sbyte>();
        }


        public IEnumerable<short> GetShortArray(string segname)
        {
            return ((JArray)FindToken(segname)).Values<short>();
        }

        public IEnumerable<ushort> GetUShortArray(string segname)
        {
            return ((JArray)FindToken(segname)).Values<ushort>();
        }


        public IEnumerable<int> GetIntArray(string segname)
        {
            return ((JArray)FindToken(segname)).Values<int>();
        }


        public IEnumerable<uint> GetUIntArray(string segname)
        {
            return((JArray)FindToken(segname)).Values<uint>();
        }


        public IEnumerable<long> GetLongArray(string segname)
        {
            return ((JArray)FindToken(segname)).Values<long>();
        }


        public IEnumerable<ulong> GetULongArray(string segname)
        {
            return ((JArray)FindToken(segname)).Values<ulong>();
        }


        public IEnumerable<float> GetFloatArray(string segname)
        {
            return ((JArray)FindToken(segname)).Values<float>();
        }

        public IEnumerable<double> GetDoubleArray(string segname)
        {
            return ((JArray)FindToken(segname)).Values<double>();
        }


        #endregion

        #region --Helper--

        public override string ToString()
        {
            return RootValue.ToString();
        }

        //查找已经存在的Object
        private JToken FindToken(string segFullName)
        {
            var r = ((FrameName ==null || FrameName=="")? RootValue : RootValue[FrameName]);
            if (!segFullName.Contains(".")) return  r[segFullName];
            var nms = segFullName.Split('.');
            for (int i = 0; i < nms.Length-1; i++)
                r = (JObject)r[nms[i]];
            return r[nms[nms.Length-1]];
        }


        //查找Object，如果没有则创建
        private JObject LookUpObject(string[] segs)
        {
            var ret = RootValue;
            for (int i = 0; i < segs.Length - 1; i++)
            {
                if (ret.ContainsKey(segs[i]))
                    ret = (JObject)ret[segs[i]];
                else
                {
                    var n = new JObject();
                    ret.Add(segs[i], n);
                    ret = n;
                }
            }
            return ret;
        }

        public object RootObject()
        {
            return RootValue;
        }

        #endregion


    }
}
