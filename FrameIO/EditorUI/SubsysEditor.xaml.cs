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
        private bool _updating = false;

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

            var bd = new Binding() { Path = new PropertyPath("SelectedAction"), Source = this, Mode = BindingMode.OneWayToSource };
            var bd2 = new Binding() { Path = new PropertyPath("SelectedAction"), Source = this, Mode = BindingMode.OneWayToSource };
            acGrid.SetBinding(PropertyTools.Wpf.DataGrid.SelectedItemsProperty, bd);
            acGrid.SetBinding(PropertyTools.Wpf.DataGrid.SelectionCellProperty, bd2);

            var bbd = new Binding() { Path = new PropertyPath("SelectedChannel"), Source = this, Mode = BindingMode.OneWayToSource };
            var bbd2 = new Binding() { Path = new PropertyPath("SelectedChannel"), Source = this, Mode = BindingMode.OneWayToSource };
            chGrid.SetBinding(PropertyTools.Wpf.DataGrid.SelectedItemsProperty, bbd);
            chGrid.SetBinding(PropertyTools.Wpf.DataGrid.SelectionCellProperty, bbd2);

        }

        public Object SelectedAction
        {
            set
            {
                UpdateActionSel();
            }
        }

        public Object SelectedChannel
        {
            set
            {
                UpdateChannelSel();
            }
        }

        //更新动作关联字段名选择
        private void UpdateActionSel()
        {
            if (_updating) return;
            _updating = true;
            var sels = acGrid.SelectedItems;
            SubsysAction sel = null;
            if (sels != null)
            {
                foreach (SubsysAction sell in sels)
                    sel = sell;
            }
            else if(_sys.Actions.Count>0)
            {
                sel = _sys.Actions[0];
            }

            if (sel == null)
            {
                actionMapGrid.ItemsSource = null;
            }
            else
            {
                actionMapGrid.ItemsSource = sel.LiteMaps;
                if (_frms.Where(p => p.Name == sel.FrameName).Count() > 0)
                {
                    actionMapGrid.ColumnDefinitions[0].ItemsSource = Helper.GetFrameSegmentsName(sel.FrameName, _frms, null, true);
                    actionMapGrid.ColumnDefinitions[1].ItemsSource = _sys.Propertys.Select(p => p.Name);
                }
                else
                {
                    actionMapGrid.ColumnDefinitions[0].ItemsSource = null;
                    actionMapGrid.ColumnDefinitions[1].ItemsSource = null;
                }
            }
            _updating = false;
        }

        //更新通道参数名选择
        private void UpdateChannelSel()
        {
            if (_updating) return;
            _updating = true;
            var sels = chGrid.SelectedItems;
            SubsysChannel sel = null;
            if (sels != null)
            {
                foreach (SubsysChannel sell in sels)
                    sel = sell;
            }
            if (sel == null)
                chOpGrid.ItemsSource = null;
            else
            {
                chOpGrid.ItemsSource = sel.Options;
                chOpGrid.ColumnDefinitions[0].ItemsSource = FrameIOCodeCheck.GetChannelOptionName(sel.ChannelType);
            }
            _updating = false;
        }


        //更新属性类型列表
        private void UpdatePropertyTypeList(object sender, RoutedEventArgs e)
        {
            if (_updating) return;
            _updating = true;
            _proj.UpdateChildSys();
            propGrid.ColumnDefinitions[1].ItemsSource = _proj.GetPropertySelectTypeList();
            propGrid.ColumnDefinitions[1].SelectedValuePath = "SelectValue";
            propGrid.ColumnDefinitions[1].DisplayMemberPath = "SelectName";
            propGrid.ColumnDefinitions[1].ItemsSourceProperty = "SelectValue";
            _updating = false;
        }

        private void UpdateSelChannel(object sender, RoutedEventArgs e)
        {
            UpdateChannelSel();
        }

        private void UpdateSelAction(object sender, RoutedEventArgs e)
        {
            UpdateActionSel();
        }
    }
}
