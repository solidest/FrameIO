using Newtonsoft.Json.Linq;
using System;


namespace FrameIO.Run
{

    internal class ExpRunCtx : IExpRunCtx
    {
        private JObject _vParent;
        private SegRunContainer _segParent;
        private IFrameWriteBuffer _buff;

        public ExpRunCtx(IFrameWriteBuffer buff, JObject vParent, SegRunContainer segParent)
        {
            _vParent = vParent;
            _segParent = segParent;
            _buff = buff;
        }


        public double GetDouble(string id)
        {
            return _vParent[id].Value<double>();
        }

        public long GetLong(string id)
        {
            return _vParent[id].Value<long>();
        }

        public int GetStartPos(string seg)
        {
            return _buff.GetPos(_vParent[seg]);
        }

        public int GetEndPos(string seg)
        {
            return GetStartPos(seg) + GetSizeOfSegment(seg);
        }


        public int GetSizeOfSegment(string seg)
        {
            var len = _segParent[seg].GetBitLen(_buff, _vParent);
            if(len%8!=0) throw new Exception("runtime 数据帧字段未能整字节对齐");
            return len/8;
        }

        public int GetSizeOfThis()
        {
            var len = _segParent.GetBitLen(_buff, _vParent);
            if (len % 8 != 0) throw new Exception("runtime 数据帧字段未能整字节对齐");
            return len / 8;
        }

        public bool TryGetLong(string id, ref long v)
        {
            if (_vParent != null && _vParent.ContainsKey(id))
            {
                v = GetLong(id);
                return true;
            }

            return false;
        }

        public bool TryGetDouble(string id, ref double v)
        {
            if (_vParent != null && _vParent.ContainsKey(id))
            {
                v = GetDouble(id);
                return true;
            }
            return false;
        }
    }
}
