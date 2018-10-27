using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private DispatcherTimer _fUpdateTimer;
        public FrameEditor(Frame frm)
        {
            _frm = frm;
            _segs = new  FrameSegmentSummaryList(_frm.Segments);
            InitializeComponent();
            DataContext = _segs;

            _fUpdateTimer = new DispatcherTimer();
            _fUpdateTimer.Interval = TimeSpan.FromSeconds(0.3);
            _fUpdateTimer.Tick += delegate { UpdateTimer(); };
        }

        private void UpdateTimer()
        {
            var s = segGrid.SelectionCell.Row;
            if (s < 0 || _segs.Segs.Count==0)
                proGrid.SelectedObject = null;
            else
            {
                proGrid.SelectedObject = _segs.Segs[s]._seg;
            }
        }

        private void StopUpdate(object sender, RoutedEventArgs e)
        {
            _fUpdateTimer.Stop();
        }

        private void StartUpdate(object sender, RoutedEventArgs e)
        {
            _fUpdateTimer.Start();
        }
    }
}
