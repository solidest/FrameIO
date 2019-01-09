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
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            o = (JObject)o[segs[segs.Length - 1]];
            return new FrameObject(o);
        }

        public IEnumerable<FrameObject> GetObjectArray(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            var arr = (JArray)o[segs[segs.Length - 1]];
            var ret = new List<FrameObject>(arr.Count);
            return arr.Select(p=>new FrameObject((JObject)p));
        }

        public bool GetBool(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Value<bool>();
        }

        public IEnumerable<bool> GetBoolArray(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Values<bool>();
        }

        public byte GetByte(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Value<byte>();
        }

        public IEnumerable<byte> GetByteArray(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Values<byte>();
        }

        public sbyte GetSByte(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Value<sbyte>();
        }

        public IEnumerable<sbyte> GetSByteArray(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Values<sbyte>();
        }

        public short GetShort(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Value<short>();
        }

        public IEnumerable<short> GetShortArray(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Values<short>();
        }

        public ushort GetUShort(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Value<ushort>();
        }

        public IEnumerable<ushort> GetUShortArray(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Values<ushort>();
        }

        public int GetInt(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Value<int>();
        }

        public IEnumerable<int> GetIntArray(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Values<int>();
        }

        public uint GetUInt(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Value<uint>();
        }

        public IEnumerable<uint> GetUIntArray(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Values<uint>();
        }

        public long GetLong(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Value<long>();
        }

        public IEnumerable<long> GetLongArray(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Values<long>();
        }

        public ulong GetULong(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Value<ulong>();
        }

        public IEnumerable<ulong> GetULongArray(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Values<ulong>();
        }

        public float GetFloat(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Value<float>();
        }


        public IEnumerable<float> GetFloatArray(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Values<float>();
        }


        public double GetDouble(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Value<double>();
        }


        public IEnumerable<double> GetDoubleArray(string segname)
        {
            var o = RootValue;
            var segs = segname.Split('.');
            if (segs.Length > 1) o = FindObject(segs);
            return o[segs[segs.Length - 1]].Values<double>();
        }


    #endregion

        #region --Helper--

        public override string ToString()
        {
            return RootValue.ToString();
        }

        //查找已经存在的Object
        private JObject FindObject(string[] segs)
        {
            var ret = RootValue;
            for (int i = 0; i < segs.Length - 1; i++)
                ret = (JObject)ret[segs[i]];
            return ret;
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
