using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace FrameIO.Run
{
    //运行时字段的容器
    internal abstract class SegRunContainer : SegRunBase, ISegArrayable
    {
        private Dictionary<string, ISegRun> _segs;

        internal abstract protected string ItemsListToken { get; }
        internal abstract protected SegmentTypeEnum GetItemType(JObject o);
        public abstract int GetItemBitLen(JObject parent, JToken theValu);
        public abstract bool GetItemNeedBitLen(ref int len, out ISegRun next, JObject parent, JToken theValue);
        public abstract ISegRun PackItem(IFrameWriteBuffer buff, JObject parent, JArray arr, JToken theValue);
        public abstract ISegRun UnpackItem(IFrameReadBuffer buff, JObject parent, JArray arr, JToken theValue);


        internal SegRunContainer()
        {
            _segs = new Dictionary<string, ISegRun>();
        }

        internal ISegRun this[string segname]
        {
            get
            {
                return _segs[segname];
            }
        }


        #region --Array--

        private SegRunArrayWrapper _arr;
        private bool _isarr  = false;

        protected void InitialArray(JObject o)
        {
            _isarr = true;
            _arr = new SegRunArrayWrapper(this, o);
        }

        #endregion

        #region --Pack && Unpack--

        public override ISegRun Pack(IFrameWriteBuffer buff, JObject parent)
        {
            return _isarr ? _arr.Pack(buff, parent) : PackItem(buff, parent, null, parent[Name]);
        }
        public override ISegRun Unpack(IFrameReadBuffer buff, JObject parent)
        {
            return _isarr ? _arr.Unpack(buff, parent) : UnpackItem(buff, parent, null, parent?[Name]);
        }

        public override int GetBitLen(JObject parent)
        {
            return _isarr ? _arr.GetBitLen(parent) : GetItemBitLen(parent, parent[Name]);
        }

        public override bool GetNeedBitLen(ref int len, out ISegRun next, JObject parent)
        {
            return _isarr ? _arr.GetNeedBitLen(ref len, out next, parent) : GetItemNeedBitLen(ref len, out next, parent, parent?[Name]);
        }


        #endregion


        #region --Initial--

        //从json初始化字段列表

        protected override void InitialFromJson(JObject o)
        {
            var segs = o[ItemsListToken].Value<JArray>();
            foreach (JObject seg in segs)
            {
                var pseg = (JProperty)seg.First;
                var oseg = pseg.Value.Value<JObject>();
                SegmentTypeEnum t = GetItemType(oseg);
                switch (t)
                {
                    case SegmentTypeEnum.SegInteger:
                        AddItem(pseg.Name, SegRunInteger.NewSegInteger(oseg, pseg.Name, false));
                        break;
                    case SegmentTypeEnum.SegIntegerArray:
                        AddItem(pseg.Name, SegRunInteger.NewSegInteger(oseg, pseg.Name, true));
                        break;
                    case SegmentTypeEnum.SegReal:
                        AddItem(pseg.Name, SegRunReal.NewSegReal(oseg, pseg.Name, false));
                        break;
                    case SegmentTypeEnum.SegRealArray:
                        AddItem(pseg.Name, SegRunReal.NewSegReal(oseg, pseg.Name, true));
                        break;
                    case SegmentTypeEnum.SegGroup:
                        AddItem(pseg.Name, SegRunGroup.NewSegGroup(oseg, pseg.Name, false));
                        break;
                    case SegmentTypeEnum.SegGroupArray:
                        AddItem(pseg.Name, SegRunGroup.NewSegGroup(oseg, pseg.Name, true));
                        break;
                    case SegmentTypeEnum.SegOneOfGroup:
                        AddItem(pseg.Name, SegRunOneOfGroup.NewOneOfGroup(oseg, pseg.Name, false));
                        break;
                    case SegmentTypeEnum.SegOneOfGroupArray:
                        AddItem(pseg.Name, SegRunOneOfGroup.NewOneOfGroup(oseg, pseg.Name, true));
                        break;
                    case SegmentTypeEnum.SegOneOfItem:
                        AddItem(pseg.Name, SegRunOneOfItem.NewOneOfItem(oseg, pseg.Name));
                        break;
                    default:
                        throw new Exception("unknow");
                }
            }
        }

        #endregion


        #region --Helper--

        private void AddItem(string name, SegRunBase seg)
        {
            _segs.Add(name, seg);
            seg.Root = Root;
            seg.Parent = this;

            if (First == null)
            {
                First = seg;
                Last = seg;
            }
            else
            {
                seg.Previous = Last;
                ((SegRunBase)Last).Next = seg;
                Last = seg;
            }
        }

        #endregion

    }
}
