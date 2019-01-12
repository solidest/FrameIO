using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    public class FioNetObject
    {
        private FrameObject _fo;
        internal FioNetObject(FrameObject fo)
        {
            _fo = fo;
        }

        public void SetValue(string segname, Parameter<bool?> value)
        {
            _fo.SetValue(segname, value.Value ?? false);
        }

        public void GetValue(string segname, Parameter<bool?> value)
        {
            value.Value = _fo.GetBool(segname);
        }

        public void SetValue(string segname, Collection<Parameter<bool?>> value)
        {
            _fo.SetValueArray(segname, value.Select(p => p.Value ?? false));
        }

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

        public void SetValueObject(string segname, FioNetObject value)
        {
            _fo.SetObject(segname, value._fo);
        }

        public FioNetObject GetValueObject(string segname)
        {
            return new FioNetObject(_fo.GetObject(segname));
        }

        public IEnumerable<FioNetObject> GetValueObjectArray(string segname)
        {
            return _fo.GetObjectArray(segname).Select(p => new FioNetObject(p)).ToArray();
        }

        public void SetValueObjectArray(string segname, Collection<FioNetObject> values)
        {
            _fo.SetObjectArray(segname, values.Select(p => p._fo));
        }

    }
}
