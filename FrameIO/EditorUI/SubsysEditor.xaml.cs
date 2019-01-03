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
        private IOProject _proj;
        private Subsys _sys;

        private ObservableCollection<Frame> _frms;
        public SubsysEditor(Subsys sys, ObservableCollection<Frame> frms, IOProject proj)
        {
            _sys = sys;
            _proj = proj;

            InitializeComponent();
            DataContext = _sys;
            _frms = frms;

            acGrid.ColumnDefinitions[2].ItemsSource = _sys.Channels;
            acGrid.ColumnDefinitions[2].SelectedValuePath = "Name";
            acGrid.ColumnDefinitions[3].ItemsSource = frms;
            acGrid.ColumnDefinitions[3].SelectedValuePath = "Name";
        }

        //更新动作关联字段名选择
        private void UpdateTimerAc(object sender, RoutedEventArgs e)
        {
            var s = acGrid.SelectionCell.Row;
            if (s < 0 || _sys.Actions.Count==0)
                acOpGrid.ItemsSource = null;
            else
            {
                acOpGrid.ItemsSource = _sys.Actions[s].LiteMaps;
                if (_frms.Where(p => p.Name == _sys.Actions[s].FrameName).Count() > 0)
                {
                    acOpGrid.ColumnDefinitions[0].ItemsSource = Helper.GetFrameSegmentsName(_sys.Actions[s].FrameName, _frms, null, true);
                    acOpGrid.ColumnDefinitions[1].ItemsSource = _proj.GetPropertyList(_sys.Propertys);

                }
                else
                {
                    acOpGrid.ColumnDefinitions[0].ItemsSource = null;
                    acOpGrid.ColumnDefinitions[1].ItemsSource = null;

                }
            }
        }

        //更新通道参数名选择
        private void UpdateTimerCh(object sender, RoutedEventArgs e)
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


        //更新属性类型列表
        private void UpdatePropertyTypeList(object sender, RoutedEventArgs e)
        {
            _proj.UpdateSubSys();
            propGrid.ColumnDefinitions[1].ItemsSource = _proj.GetPropertySelectTypeList();
            propGrid.ColumnDefinitions[1].SelectedValuePath = "SelectValue";
            propGrid.ColumnDefinitions[1].DisplayMemberPath = "SelectName";
            propGrid.ColumnDefinitions[1].ItemsSourceProperty = "SelectValue";
        }

    }
}
