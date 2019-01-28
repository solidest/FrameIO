using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace FrameIO.Run
{
    //运行时字段的容器
    internal abstract class SegRunContainer : SegRunBase
    {
        private Dictionary<string, SegRunBase> _segs;

        //internal abstract protected JObject ItemsList { get; }
        internal abstract protected SegmentTypeEnum GetItemType(JObject o);


        public SegRunContainer()
        {
            _segs = new Dictionary<string, SegRunBase>();
        }

        public SegRunBase this[string segname]
        {
            get
            {
                return _segs[segname];
            }
        }

        #region --Unpack--

        //子节点全部查找完毕，父节点继续查找下一数值字段
        public abstract bool  LookUpNextValueSeg(out SegRunValue firstSeg, out JContainer pc, out int repeated, JObject ctxOfChild);


        #endregion

        #region --Initial--

        //从json初始化字段列表

        protected void InitialFromJson(JObject segs)
        {
            
            foreach (var pseg in segs.Properties())
            {
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
                        AddItem(pseg.Name, SegRunGroup.NewSegGroup(oseg[SEGMENTLIST_TOKEN].Value<JObject>(), pseg.Name));
                        break;
                    case SegmentTypeEnum.SegGroupArray:
                        AddItem(pseg.Name, SegRunGroup.NewSegGroupArray(oseg[SEGMENTLIST_TOKEN].Value<JObject>(), pseg.Name, oseg));
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
