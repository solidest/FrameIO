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
        public bool IsArray { get; private set; } = false;

        protected void InitialArray(JObject o)
        {
            IsArray = true;
            _arr = new SegRunArrayWrapper(this, o);
        }

        #endregion

        #region --Pack--

        public abstract int GetItemBitLen(IFrameBuffer buff, JObject parent);
        public abstract ISegRun PackItem(IFrameBuffer buff, JObject parent);


        public override ISegRun Pack(IFrameBuffer buff, JObject parent)
        {
            return IsArray ? _arr.Pack(buff, parent) : PackItem(buff, parent);
        }

        public override int GetBitLen(IFrameBuffer buff, JObject parent)
        {
            return IsArray ? _arr.GetBitLen(buff, parent) : GetItemBitLen(buff, parent);
        }

        #endregion

        #region --UnPack--

        public abstract bool TryGetItemBitLen(IFrameBuffer buff, ref int len, JObject parent);
        public override bool TryGetBitLen(IFrameBuffer buff, ref int len, JObject parent)
        {
            return IsArray ? _arr.TryGetBitLen(buff, ref len, parent) : TryGetItemBitLen(buff, ref len, parent);
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
