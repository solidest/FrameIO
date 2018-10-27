﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{

    public class FrameSegmentSummaryList : INotifyPropertyChanged
    {
        public ObservableCollection<FrameSegmentSummary> Segs { get; private set; }
        private ObservableCollection<FrameSegmentBase> _segs;

        public event PropertyChangedEventHandler PropertyChanged;

        public FrameSegmentSummaryList(ObservableCollection<FrameSegmentBase> segs)
        {
            _segs = segs;
            Segs = new ObservableCollection<FrameSegmentSummary>();
            foreach (var seg in segs)
            {
                var nseg = new FrameSegmentSummary(seg, segs);
                Segs.Add(nseg);
                
            }
            Segs.CollectionChanged += ChangedSegment;
        }

        //集合发生改变
        private void ChangedSegment(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (FrameSegmentSummary seg in e.NewItems)
                    {
                        seg._seg = new FrameSegmentInteger();
                        seg._type = SegmentType.Integer;
                        seg._segs = _segs;
                        _segs.Add(seg._seg);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (FrameSegmentSummary seg in e.OldItems)
                    {
                        seg._segs.Remove(seg._seg);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

    }

    public enum SegmentType
    {
        Integer,
        Real,
        Frame,
        OneOf
    }

    public class FrameSegmentSummary : INotifyPropertyChanged
    {

        public FrameSegmentBase _seg;
        public ObservableCollection<FrameSegmentBase> _segs;
        public SegmentType _type;
        public event PropertyChangedEventHandler PropertyChanged;



        public FrameSegmentSummary()
        {

        }

        public FrameSegmentSummary(FrameSegmentBase seg, ObservableCollection<FrameSegmentBase> parent)
        {
            if (seg.GetType() == typeof(FrameSegmentInteger)) _type = SegmentType.Integer;
            if (seg.GetType() == typeof(FrameSegmentReal)) _type = SegmentType.Real;
            if (seg.GetType() == typeof(FrameSegmentBlock))
            {
                var se = (FrameSegmentBlock)seg;
                if (se.UsedType == BlockSegType.RefFrame)
                    _type = SegmentType.Frame;
                else if (se.UsedType == BlockSegType.OneOf)
                    _type = SegmentType.OneOf;
            }
            _seg = seg;
            _segs = parent;
        }

        public string Name { get => _seg.Name; set { _seg.Name = value; } }

        public SegmentType SegType { get => _type; set { ChangedType(value); } }

        private void ChangedType(SegmentType t)
        {
            if (_type == t) return;
            _type = t;
            var idx = _segs.IndexOf(_seg);
            FrameSegmentBase seg = null;
            switch (t)
            {
                case SegmentType.Integer:
                    seg = new FrameSegmentInteger();
                    break;
                case SegmentType.Real:
                    seg = new FrameSegmentReal();
                    break;
                case SegmentType.Frame:
                    var nseg = new FrameSegmentBlock();
                    nseg.UsedType = BlockSegType.RefFrame;
                    seg = nseg;
                    break;
                case SegmentType.OneOf:
                    var bseg = new FrameSegmentBlock();
                    bseg.UsedType = BlockSegType.OneOf;
                    seg = bseg;
                    break;
            }
            seg.Name = _seg.Name;
            _seg = seg;
            _segs[idx] = _seg;
        }
    }

}
