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
        private FrameSegmentSummaryList _subsegssummary;
        private ObservableCollection<FrameSegmentBase> _subsegs;
        private bool IsUpdating = false;

        public FrameEditor(Frame frm)
        {
            _frm = frm;
            _segs = new FrameSegmentSummaryList(_frm.Segments);
            InitializeComponent();
            DataContext = _segs;
            var bd = new Binding() { Path = new PropertyPath("SelectedItems"), Source = this, Mode = BindingMode.OneWayToSource };
            var bd2 = new Binding() { Path = new PropertyPath("SelectedItems"), Source = this, Mode = BindingMode.OneWayToSource };
            segGrid.SetBinding(PropertyTools.Wpf.DataGrid.SelectedItemsProperty, bd);
            segGrid.SetBinding(PropertyTools.Wpf.DataGrid.SelectionCellProperty, bd2);

            var bbd = new Binding() { Path = new PropertyPath("SelectedSubItems"), Source = this, Mode = BindingMode.OneWayToSource };
            var bbd2 = new Binding() { Path = new PropertyPath("SelectedSubItems"), Source = this, Mode = BindingMode.OneWayToSource };
            subsegGrid.SetBinding(PropertyTools.Wpf.DataGrid.SelectedItemsProperty, bbd);
            subsegGrid.SetBinding(PropertyTools.Wpf.DataGrid.SelectionCellProperty, bbd2);

        }

        public Object SelectedItems
        {
            set
            {
                UpdateSelectSeg();
            }
        }

        public Object SelectedSubItems
        {
            set
            {
                UpdateSubSelectSeg();
            }
        }

        //更新主字段表格选中的字段
        private void UpdateSelectSeg()
        {
            IsUpdating = true;
            FrameSegmentSummary sel = null;
            if (segGrid.SelectedItems != null)
            {
                foreach (FrameSegmentSummary ssel in segGrid.SelectedItems)
                {
                    sel = ssel;
                }
            }

            proGrid.SelectedObject = sel==null? null : sel._seg;

            if (sel != null && sel.SegType == SegmentType.SubSys)
            {
                ShowSubSegGrid(sel.GetSubSegs());
            }
            else
            {
                HideSubSegGrid();
            }

            IsUpdating = false;
        }

        //更新子字段表格选中的字段
        private void UpdateSubSelectSeg()
        {
            if (IsUpdating) return;
            FrameSegmentSummary sel = null;
            if (subsegGrid.SelectedItems != null)
            {
                foreach (FrameSegmentSummary ssel in subsegGrid.SelectedItems)
                {
                    sel = ssel;
                }
            }
            proGrid.SelectedObject = sel == null ? null : sel._seg;

        }

        //隐藏子字段表格
        private void HideSubSegGrid()
        {
            if (_subsegs != null)
            {
                mgrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Pixel);
                _subsegs = null;
                _subsegssummary = null;
                subsegGrid.DataContext = null;
            }
        }

        //显示子字段表格
        private void ShowSubSegGrid(ObservableCollection<FrameSegmentBase> subsegs)
        {
            if (_subsegs == null)  
                mgrid.RowDefinitions[1].Height = new GridLength(260, GridUnitType.Pixel);
            if (_subsegs == subsegs) return;
            _subsegs = subsegs;
            _subsegssummary = new FrameSegmentSummaryList(_subsegs);
            subsegGrid.DataContext = _subsegssummary;
        }

    }
}
