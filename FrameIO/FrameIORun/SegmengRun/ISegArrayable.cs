using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal interface ISegArrayable : ISegRun
    {
        bool TryGetItemBitLen(ref int len, JObject parent);
        int GetItemBitLen(JObject parent);
        ISegRun PackItem(IFrameWriteBuffer buff, JObject parent);
        bool IsArray { get; }
    }

    internal class SegRunArrayWrapper
    {
        private ISegArrayable _item;
        private IExpRun _arrLen;

        public SegRunArrayWrapper(ISegArrayable item, JObject o)
        {
            _arrLen = Helper.GetExp(o[SegRunBase.ARRAYLEN_TOKEN]);
            _item = item;
        }

        public ISegRun Pack(IFrameWriteBuffer buff, JObject parent)
        {
            var vs = parent?[_item.Name]?.Value<JArray>();
            var len = _arrLen.GetLong(parent, _item.Parent);

            for (int i = 0; i < len; i++)
            {
                var vsi = i < (vs?.Count ?? 0) ? vs[i].Value<JObject>() : null;
                _item.PackItem(buff, vsi);
            }

            return _item.Next;
        }


        public int GetBitLen(JObject parent)
        {
            int len = (int)_arrLen.GetLong(parent, _item.Parent);

            return _item.GetItemBitLen(parent) * len;
        }

        public bool TryGetBitLen(ref int len, JObject parent)
        {
            long myLen = 0;
            if (_arrLen.IsConst)
                myLen = _arrLen.GetLong(parent, _item.Parent);
            else
            {
                if (!_arrLen.TryGetLong(parent, _item.Parent, ref myLen))
                    return false;
            }

            for (int i = 0; i < myLen; i++)
            {
                if (!_item.TryGetItemBitLen(ref len, parent))
                    return false;
            }

            return true;
        }
    }
}
