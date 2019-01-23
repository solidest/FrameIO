
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FrameIO.Run;

namespace test_tcp_noconnect
{
    public class FioNetObject
    {
        private FrameObject _fo;

        public FioNetObject()
        {
            _fo = new FrameObject();
        }

        internal FioNetObject(FrameObject fo)
        {
            _fo = fo;
        }

        internal FrameObject TheObject { get => _fo; }

        #region --SetValue--

        public void SetValue(string segname, int value)
        {
            _fo.SetValue(segname, value);
        }

        public void SetValue(string segname, Parameter<bool?> value)
        {
            _fo.SetValue(segname, value.Value ?? false);
        }


        public void SetValue(string segname, Parameter<byte?> value)
        {
            _fo.SetValue(segname, value.Value ?? (byte)0);
        }


        public void SetValue(string segname, Parameter<sbyte?> value)
        {
            _fo.SetValue(segname, value.Value ?? (sbyte)0);
        }


        public void SetValue(string segname, Parameter<short?> value)
        {
            _fo.SetValue(segname, value.Value ?? (short)0);
        }


        public void SetValue(string segname, Parameter<ushort?> value)
        {
            _fo.SetValue(segname, value.Value ?? (ushort)0);
        }


        public void SetValue(string segname, Parameter<int?> value)
        {
            _fo.SetValue(segname, value.Value ?? (int)0);
        }


        public void SetValue(string segname, Parameter<uint?> value)
        {
            _fo.SetValue(segname, value.Value ?? (uint)0);
        }


        public void SetValue(string segname, Parameter<long?> value)
        {
            _fo.SetValue(segname, value.Value ?? (long)0);
        }


        public void SetValue(string segname, Parameter<ulong?> value)
        {
            _fo.SetValue(segname, value.Value ?? (ulong)0);
        }


        public void SetValue(string segname, Parameter<double?> value)
        {
            _fo.SetValue(segname, value.Value ?? (double)0);
        }


        public void SetValue(string segname, Parameter<float?> value)
        {
            _fo.SetValue(segname, value.Value ?? (float)0);
        }
        #endregion

        #region --SetValueArray--

        public void SetValue(string segname, Collection<Parameter<bool?>> value)
        {
            _fo.SetValueArray(segname, value.Select(p => p.Value ?? false));
        }

        public void SetValue(string segname, Collection<Parameter<byte?>> value)
        {
            _fo.SetValueArray(segname, value.Select(p => p.Value ?? 0));
        }

        public void SetValue(string segname, Collection<Parameter<sbyte?>> value)
        {
            _fo.SetValueArray(segname, value.Select(p => p.Value ?? 0));
        }

        public void SetValue(string segname, Collection<Parameter<short?>> value)
        {
            _fo.SetValueArray(segname, value.Select(p => p.Value ?? 0));
        }

        public void SetValue(string segname, Collection<Parameter<ushort?>> value)
        {
            _fo.SetValueArray(segname, value.Select(p => p.Value ?? 0));
        }

        public void SetValue(string segname, Collection<Parameter<int?>> value)
        {
            _fo.SetValueArray(segname, value.Select(p => p.Value ?? 0));
        }


        public void SetValue(string segname, Collection<Parameter<uint?>> value)
        {
            _fo.SetValueArray(segname, value.Select(p => p.Value ?? 0));
        }


        public void SetValue(string segname, Collection<Parameter<long?>> value)
        {
            _fo.SetValueArray(segname, value.Select(p => p.Value ?? 0));
        }


        public void SetValue(string segname, Collection<Parameter<ulong?>> value)
        {
            _fo.SetValueArray(segname, value.Select(p => p.Value ?? 0));
        }


        public void SetValue(string segname, Collection<Parameter<double?>> value)
        {
            _fo.SetValueArray(segname, value.Select(p => p.Value ?? 0));
        }


        public void SetValue(string segname, Collection<Parameter<float?>> value)
        {
            _fo.SetValueArray(segname, value.Select(p => p.Value ?? 0));
        }

        #endregion

        #region --GetValue--


        public int GetValue(string segname)
        {
            return _fo.GetInt(segname);
        }

        public void GetValue(string segname, Parameter<bool?> value)
        {
            value.Value = _fo.GetBool(segname);
        }


        public void GetValue(string segname, Parameter<byte?> value)
        {
            value.Value = _fo.GetByte(segname);
        }

        public void GetValue(string segname, Parameter<sbyte?> value)
        {
            value.Value = _fo.GetSByte(segname);
        }

        public void GetValue(string segname, Parameter<short?> value)
        {
            value.Value = _fo.GetShort(segname);
        }

        public void GetValue(string segname, Parameter<ushort?> value)
        {
            value.Value = _fo.GetUShort(segname);
        }


        public void GetValue(string segname, Parameter<int?> value)
        {
            value.Value = _fo.GetInt(segname);
        }

        public void GetValue(string segname, Parameter<uint?> value)
        {
            value.Value = _fo.GetUInt(segname);
        }

        public void GetValue(string segname, Parameter<long?> value)
        {
            value.Value = _fo.GetLong(segname);
        }

        public void GetValue(string segname, Parameter<ulong?> value)
        {
            value.Value = _fo.GetULong(segname);
        }

        public void GetValue(string segname, Parameter<float?> value)
        {
            value.Value = _fo.GetFloat(segname);
        }


        public void GetValue(string segname, Parameter<double?> value)
        {
            value.Value = _fo.GetDouble(segname);
        }


        #endregion

        #region --GetValueArray--


        public void GetValue(string segname, Collection<Parameter<bool?>> values)
        {
            var vs = _fo.GetBoolArray(segname);
            var len = Math.Min(values.Count, vs.Count());
            int i = 0;
            foreach (var v in vs)
            {
                values[i++].Value = v;
                if (i == len) break;
            }
        }

        public void GetValue(string segname, Collection<Parameter<byte?>> values)
        {
            var vs = _fo.GetByteArray(segname);
            var len = Math.Min(values.Count, vs.Count());
            int i = 0;
            foreach (var v in vs)
            {
                values[i++].Value = v;
                if (i == len) break;
            }
        }

        public void GetValue(string segname, Collection<Parameter<sbyte?>> values)
        {
            var vs = _fo.GetSByteArray(segname);
            var len = Math.Min(values.Count, vs.Count());
            int i = 0;
            foreach (var v in vs)
            {
                values[i++].Value = v;
                if (i == len) break;
            }
        }

        public void GetValue(string segname, Collection<Parameter<short?>> values)
        {
            var vs = _fo.GetShortArray(segname);
            var len = Math.Min(values.Count, vs.Count());
            int i = 0;
            foreach (var v in vs)
            {
                values[i++].Value = v;
                if (i == len) break;
            }
        }

        public void GetValue(string segname, Collection<Parameter<ushort?>> values)
        {
            var vs = _fo.GetUShortArray(segname);
            var len = Math.Min(values.Count, vs.Count());
            int i = 0;
            foreach (var v in vs)
            {
                values[i++].Value = v;
                if (i == len) break;
            }
        }

        public void GetValue(string segname, Collection<Parameter<int?>> values)
        {
            var vs = _fo.GetIntArray(segname);
            var len = Math.Min(values.Count, vs.Count());
            int i = 0;
            foreach (var v in vs)
            {
                values[i++].Value = v;
                if (i == len) break;
            }
        }

        public void GetValue(string segname, Collection<Parameter<uint?>> values)
        {
            var vs = _fo.GetUIntArray(segname);
            var len = Math.Min(values.Count, vs.Count());
            int i = 0;
            foreach (var v in vs)
            {
                values[i++].Value = v;
                if (i == len) break;
            }
        }

        public void GetValue(string segname, Collection<Parameter<long?>> values)
        {
            var vs = _fo.GetLongArray(segname);
            var len = Math.Min(values.Count, vs.Count());
            int i = 0;
            foreach (var v in vs)
            {
                values[i++].Value = v;
                if (i == len) break;
            }
        }

        public void GetValue(string segname, Collection<Parameter<ulong?>> values)
        {
            var vs = _fo.GetULongArray(segname);
            var len = Math.Min(values.Count, vs.Count());
            int i = 0;
            foreach (var v in vs)
            {
                values[i++].Value = v;
                if (i == len) break;
            }
        }

        public void GetValue(string segname, Collection<Parameter<double?>> values)
        {
            var vs = _fo.GetDoubleArray(segname);
            var len = Math.Min(values.Count, vs.Count());
            int i = 0;
            foreach (var v in vs)
            {
                values[i++].Value = v;
                if (i == len) break;
            }
        }

        public void GetValue(string segname, Collection<Parameter<float?>> values)
        {
            var vs = _fo.GetFloatArray(segname);
            var len = Math.Min(values.Count, vs.Count());
            int i = 0;
            foreach (var v in vs)
            {
                values[i++].Value = v;
                if (i == len) break;
            }
        }



        #endregion

        #region --Object-


        public void SetValue(string segname, FioNetObject value)
        {
            _fo.SetObject(segname, value._fo);
        }

        public void SetValue(string segname, Collection<FioNetObject> values)
        {
            _fo.SetObjectArray(segname, values.Select(p => p._fo));
        }

        public FioNetObject GetObject(string segname)
        {
            return new FioNetObject(_fo.GetObject(segname));
        }

        public IEnumerable<FioNetObject> GetObjectArray(string segname)
        {
            return _fo.GetObjectArray(segname).Select(p => new FioNetObject(p)).ToArray();
        }

        #endregion




    }
}
