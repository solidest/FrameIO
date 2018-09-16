using System;
using System.Collections.Generic;
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

namespace FrameIO
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //窗体加载 UI初始化
        private void MainFormLoaded(object sender, RoutedEventArgs e)
        {
            var toolBarThumb = MainToolBar.Template.FindName("ToolBarThumb", MainToolBar) as FrameworkElement;
            if (toolBarThumb != null)
            {
                toolBarThumb.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = MainToolBar.Template.FindName("MainPanelBorder", MainToolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }

        }

        //切换视图
        private void SwitchView(object sender, RoutedEventArgs e)
        {
            if(edCode.Visibility == Visibility.Collapsed)
            {
                tbDocTree.Visibility = Visibility.Collapsed;
                VSplitter.Visibility = Visibility.Collapsed;
                tbPages.Visibility = Visibility.Collapsed;
                edCode.Visibility = Visibility.Visible;
            }
            else
            {
                edCode.Visibility = Visibility.Collapsed;
                tbDocTree.Visibility = Visibility.Visible;
                VSplitter.Visibility = Visibility.Visible;
                tbPages.Visibility = Visibility.Visible;
            }
        }

        //显示隐藏输出面板
        private void OutDispHide(object sender, RoutedEventArgs e)
        {
            switch (this.HSplitter.Visibility)
            {
                case Visibility.Visible:
                    HSplitter.Visibility = Visibility.Collapsed;
                    gridMain.RowDefinitions[3].Height = new GridLength(0, GridUnitType.Pixel);
                    break;
                case Visibility.Collapsed:
                    HSplitter.Visibility = Visibility.Visible;
                    gridMain.RowDefinitions[3].Height = new GridLength(300, GridUnitType.Pixel);
                    break;
            }
            e.Handled = true;
        }
    }


}
