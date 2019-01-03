using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FrameIO.Main
{
    /// <summary>
    /// FrameEditor.xaml 的交互逻辑
    /// </summary>
    public partial class FrameEditor : UserControl
    {
        private Frame _frm;
        private FrameSegmentSummaryList _segs;
        private FrameSegmentSummaryList _subsegs;

        public FrameEditor(Frame frm)
        {
            _frm = frm;
            _segs = new  FrameSegmentSummaryList(_frm.Segments);
            InitializeComponent();
            DataContext = _segs;

        }

        private void UpdateMainTimer(object sender, RoutedEventArgs e)
        {
            var s = segGrid.SelectionCell.Row;
            if (s < 0 || _segs.Segs.Count==0)
                proGrid.SelectedObject = null;
            else
            {
                if (_segs.Segs[s].SegType == SegmentType.SubSys)
                {
                    if(mgrid.RowDefinitions[1].Height.Value==0) mgrid.RowDefinitions[1].Height = new GridLength(260, GridUnitType.Pixel);
                    var subsegs = _segs.Segs[s].GetSubSegs();
                    if(_subsegs != subsegs)
                    {
                        _subsegs = subsegs;
                        subsegGrid.DataContext = _subsegs;
                    }

                }
                else
                {
                    mgrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
                    _subsegs = null;
                    subsegGrid.DataContext = null;
                }

                proGrid.SelectedObject = _segs.Segs[s]._seg;
            }
        }

        private void UpdateSubTimer(object sender, RoutedEventArgs e)
        {
            var s = subsegGrid.SelectionCell.Row;
            if (s < 0 || _subsegs==null || _subsegs.Segs.Count == 0)
                proGrid.SelectedObject = null;
            else
            {
                proGrid.SelectedObject = _subsegs.Segs[s]._seg;
            }
        }

    }
}
