using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// SubsysEditor.xaml 的交互逻辑
    /// </summary>
    public partial class SubsysEditor : UserControl
    {
        private Subsys _sys;
        private DispatcherTimer _fUpdateTimerCh;
        private DispatcherTimer _fUpdateTimerAc;
        private ObservableCollection<Frame> _frms;
        public SubsysEditor(Subsys sys, ObservableCollection<Frame> frms)
        {
            _sys = sys;

            InitializeComponent();
            DataContext = _sys;
            _frms = frms;

            acGrid.ColumnDefinitions[2].ItemsSource = _sys.Channels;
            acGrid.ColumnDefinitions[3].ItemsSource = frms;
            acOpGrid.ColumnDefinitions[1].ItemsSource = _sys.Propertys;
  
            _fUpdateTimerCh = new DispatcherTimer();
            _fUpdateTimerCh.Interval = TimeSpan.FromSeconds(0.3);
            _fUpdateTimerCh.Tick += delegate { UpdateTimerCh(); };

            _fUpdateTimerAc = new DispatcherTimer();
            _fUpdateTimerAc.Interval = TimeSpan.FromSeconds(0.3);
            _fUpdateTimerAc.Tick += delegate { UpdateTimerAc(); };
        }

        private void UpdateTimerAc()
        {
            var s = acGrid.SelectionCell.Row;
            if (s < 0 || _sys.Actions.Count==0)
                acOpGrid.ItemsSource = null;
            else
            {
                acOpGrid.ItemsSource = _sys.Actions[s].LiteMaps;
                if (_frms.Where(p => p.Name == _sys.Actions[s].FrameName).Count() > 0)
                    acOpGrid.ColumnDefinitions[0].ItemsSource = GetFrameSegmentsName(_sys.Actions[s].FrameName);
                else
                    acOpGrid.ColumnDefinitions[0].ItemsSource = null;
            }
        }

        private IList<string> GetFrameSegmentsName(string name)
        {
            var ret = new List<string>();
            var frms = _frms.Where(p => p.Name == name);
            if (frms==null || frms.Count() == 0) return ret;
            var frm = frms.First();
            foreach(var seg in frm.Segments)
            {
                AddSegName(ret, "", seg);
            }
            return ret;
        }

        private void AddSegName(List<string> segnames, string pre, FrameSegmentBase seg)
        {
            if (segnames.Count > 1000) return;
            var ty = seg.GetType();
            if (ty == typeof(FrameSegmentInteger))
                segnames.Add((pre == "" ? "" : (pre + ".")) + seg.Name);
            else if (ty == typeof(FrameSegmentReal))
                segnames.Add((pre == "" ? "" : (pre + ".")) + seg.Name);
            else if (ty == typeof(FrameSegmentBlock))
            {
                var mypre = (pre == "" ? "" : (pre + ".")) + seg.Name;
                var bseg = (FrameSegmentBlock)seg;
                switch (bseg.UsedType)
                {
                    case BlockSegType.RefFrame:
                        {
                            var mylist = GetFrameSegmentsName(bseg.RefFrameName);
                            segnames.AddRange(mylist.Select(p => mypre + "." + p));
                            return;
                        }
                    case BlockSegType.DefFrame:
                        foreach (var myseg in bseg.DefineSegments)
                            AddSegName(segnames, mypre, myseg);
                        return;
                    case BlockSegType.OneOf:
                        foreach(var item in bseg.OneOfCaseList)
                        {
                            var itempre = mypre + "." + item.EnumItem;
                            var mylist = GetFrameSegmentsName(item.FrameName);
                            segnames.AddRange(mylist.Select(p => itempre + "." + p));
                        }
                        return;
                }
            }
        }

        private void UpdateTimerCh()
        {
            var s = chGrid.SelectionCell.Row;
            if (s < 0 || _sys.Channels.Count==0)
                chOpGrid.ItemsSource = null;
            else
            {
                chOpGrid.ItemsSource = _sys.Channels[s].Options;
                chOpGrid.ColumnDefinitions[0].ItemsSource = FrameIOCodeCheck.GetChannelOptionName(_sys.Channels[s].ChannelType);
            }
        }

        private void StartUpdateCh(object sender, RoutedEventArgs e)
        {
            _fUpdateTimerCh.Start();
        }

        private void StopUpdateCh(object sender, RoutedEventArgs e)
        {
            _fUpdateTimerCh.Start();
        }

        private void StartUpdateAc(object sender, RoutedEventArgs e)
        {
            _fUpdateTimerAc.Start();
        }

        private void StopUpdateAc(object sender, RoutedEventArgs e)
        {
            _fUpdateTimerAc.Stop();
        }
    }
}
