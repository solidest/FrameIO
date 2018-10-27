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

namespace FrameIO.Main
{
    /// <summary>
    /// EnumdefEditor.xaml 的交互逻辑
    /// </summary>
    public partial class EnumdefEditor : UserControl
    {

        private Enumdef _emdef;
        public EnumdefEditor(Enumdef emdef)
        {
            _emdef = emdef;
            InitializeComponent();
            DataContext = this;
        }

        public ObservableCollection<EnumdefItem> ItemsSource { get => _emdef.ItemsList; }

    }
}
