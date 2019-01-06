using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //运行时字段的容器
    internal abstract class SegRunContainer : SegRunBase
    {
        private Dictionary<string, SegRunBase> _segs;
        internal override string Name { get; set; }

        internal abstract protected string ItemsListToken { get; }
        internal abstract protected SegmentTypeEnum GetItemType(JObject o);

        internal SegRunContainer()
        {
            _segs = new Dictionary<string, SegRunBase>();
        }


        internal SegRunBase this[string segname]
        {
            get
            {
                return _segs[segname];
            }
        }


        #region --Initial--

        //从json填充字段列表

        internal protected override void FillFromJson(JObject o)
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
                        AddItem(pseg.Name, SegRunInteger.LoadFromJson(oseg, pseg.Name, this));
                        break;
                    case SegmentTypeEnum.SegIntegerArray:
                        AddItem(pseg.Name, SegRunIntegerArray.LoadFromJson(oseg, pseg.Name, this));
                        break;
                    case SegmentTypeEnum.SegReal:
                        AddItem(pseg.Name, SegRunReal.LoadFromJson(oseg, pseg.Name, this));
                        break;
                    case SegmentTypeEnum.SegRealArray:
                        AddItem(pseg.Name, SegRunRealArray.LoadFromJson(oseg, pseg.Name, this));
                        break;
                    case SegmentTypeEnum.SegGroup:
                        AddItem(pseg.Name, SegRunGroup.LoadFromJson(oseg, pseg.Name, this));
                        break;
                    case SegmentTypeEnum.SegGroupArray:
                        AddItem(pseg.Name, SegRunGroupArray.LoadFromJson(oseg, pseg.Name, this));
                        break;
                    case SegmentTypeEnum.SegOneOfGroup:
                        AddItem(pseg.Name, SegRunOneOfGroup.LoadFromJson(oseg, pseg.Name, this));
                        break;
                    case SegmentTypeEnum.SegOneOfGroupArray:
                        AddItem(pseg.Name, SegRunOneOfGroupArray.LoadFromJson(oseg, pseg.Name, this));
                        break;
                    case SegmentTypeEnum.SegOneOfItem:
                        AddItem(pseg.Name, SegRunOneOfItem.LoadFromJson(oseg, pseg.Name, this));
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

            if (First == null)
            {
                First = seg;
                Last = seg;
            }
            else
            {
                seg.Previous = Last;
                Last.Next = seg;
                Last = seg;
            }
        }

        #endregion

    }
}
