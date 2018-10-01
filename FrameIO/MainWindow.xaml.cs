using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using System.Xml;

namespace FrameIO.Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadEditorConfig();

            _isCoding = true;
            UpdateEditMode();
        }

        private IOProject _project;
        private bool _isCoding = false;
        private string _code = "";
        private string __file = "";
        private bool _isModified = false;

        private string FileName
        {
            get
            {
                return __file;
            }
            set
            {
                Title = "FrameIO - " + System.IO.Path.GetFileNameWithoutExtension(value);
                __file = value;
            }
        }

        #region --Event--

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

        //窗口关闭之前
        private void OnBeforeClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_isCoding)
            {
                _isModified = edCode.IsModified;
                _code = edCode.Text;
            }

            if (_isModified)
            {
                var res = MessageBox.Show(this, "是否保存对当前文档的更改？", "FrameIO", MessageBoxButton.YesNoCancel);
                switch (res)
                {
                    case MessageBoxResult.Yes:
                        File.WriteAllText(FileName, _code);
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        e.Cancel = true;
                        return;
                }
            }
        }

        #endregion

        #region --Helper--

        FoldingManager _foldingManager;
        CodeFolding _foldingStrategy;
        //加载编辑器配置
        private void LoadEditorConfig()
        {
            IHighlightingDefinition customHighlighting;
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("FrameIO.Main.CustomHighlighting.xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            // and register it in the HighlightingManager
            HighlightingManager.Instance.RegisterHighlighting("Custom Highlighting", new string[] { ".cool" }, customHighlighting);
            edCode.SyntaxHighlighting = customHighlighting;
            //edCode.TextArea.TextView.Margin = new Thickness(10, 0, 0, 0);

            
            _foldingManager = FoldingManager.Install(edCode.TextArea);
            _foldingStrategy = new CodeFolding();
            DispatcherTimer foldingUpdateTimer = new DispatcherTimer();
            foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2);
            foldingUpdateTimer.Tick += delegate { UpdateFoldings(); };
            foldingUpdateTimer.Start();
        }

        void UpdateFoldings()
        {
            if (!_isCoding) return;
            _foldingStrategy.UpdateFoldings(_foldingManager, edCode.Document);
        }

        //更新工作模式
        private void UpdateEditMode()
        {
            if (_isCoding)
            {
                edCode.Visibility = Visibility.Visible;
                tbDocTree.Visibility = Visibility.Collapsed;
                VSplitter.Visibility = Visibility.Collapsed;
                tbPages.Visibility = Visibility.Collapsed;

                btAddFrame.Visibility = Visibility.Collapsed;
                btAddSubsys.Visibility = Visibility.Collapsed;
                btDelete.Visibility = Visibility.Collapsed;
                btRename.Visibility = Visibility.Collapsed;
                btAddEnum.Visibility = Visibility.Collapsed;

                btCopy.Visibility = Visibility.Visible;
                btCut.Visibility = Visibility.Visible;
                btPaste.Visibility = Visibility.Visible;
                btUndo.Visibility = Visibility.Visible;
                btRedo.Visibility = Visibility.Visible;
                btFindReplace.Visibility = Visibility.Visible;

                edCode.IsEnabled = (__file!="");
                edCode.Text = _code;
                edCode.IsModified = _isModified;
                edCode.Focus();

            }
            else
            {
                edCode.Visibility = Visibility.Collapsed;
                tbDocTree.Visibility = Visibility.Visible;
                VSplitter.Visibility = Visibility.Visible;
                tbPages.Visibility = Visibility.Visible;

                btAddFrame.Visibility = Visibility.Visible;
                btAddSubsys.Visibility = Visibility.Visible;
                btDelete.Visibility = Visibility.Visible;
                btRename.Visibility = Visibility.Visible;
                btAddEnum.Visibility = Visibility.Visible;

                btCopy.Visibility = Visibility.Collapsed;
                btCut.Visibility = Visibility.Collapsed;
                btPaste.Visibility = Visibility.Collapsed;
                btUndo.Visibility = Visibility.Collapsed;
                btRedo.Visibility = Visibility.Collapsed;
                btFindReplace.Visibility = Visibility.Collapsed;

                edCode.IsEnabled = false;
                _code = edCode.Text;
                _isModified = edCode.IsModified;
                tbDocTree.Focus();
            }
        }

        //展开所有节点
        private void ExpandNode(ICSharpCode.TreeView.SharpTreeNode n, bool isExpand = true)
        {
            if (n == null) return;
            if(!n.IsRoot) n.IsExpanded = isExpand;
            if(n.Children != null)
            {
                foreach (var nc in n.Children)
                    ExpandNode(nc, isExpand);
            }
        }

        //重置代码状态
        private void ResetCodeState()
        {
            _isModified = false;
            edCode.IsModified = false;
            trProject.Root = new ProjectNode(_project);
        }

        //提示保存
        private bool TipSave()
        {
            if (_isCoding)
            {
                _isModified = edCode.IsModified;
                _code = edCode.Text;
            }
            if(_isModified)
            {
                var res = MessageBox.Show(this, "是否保存对当前文档的更改？", "FraeIO", MessageBoxButton.YesNoCancel);
                switch (res)
                {
                    case MessageBoxResult.Yes:
                        File.WriteAllText(FileName, _code);
                        return true;
                    case MessageBoxResult.No:
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                return true;
            }
        }

        //取数据帧父节点
        private FrameListNode GetFrameParent()
        {
            foreach(var n in trProject.Root.Children)
            {
                if (typeof(FrameListNode) == n.GetType())
                    return (FrameListNode)n;
            }
            return null;
        }

        //取受控对象父节点
        private SubsysListNode GetSubsysParent()
        {
            foreach (var n in trProject.Root.Children)
            {
                if (typeof(SubsysListNode) == n.GetType())
                    return (SubsysListNode)n;
            }
            return null;
        }

        //取枚举定义父节点
        private EnumdefListNode GetEnumdefList()
        {
            foreach (var n in trProject.Root.Children)
            {
                if (typeof(EnumdefListNode) == n.GetType())
                    return (EnumdefListNode)n;
            }
            return null;
        }

        #endregion

        #region --Command--


        //切换视图
        private void SwitchView(object sender, RoutedEventArgs e)
        {
            _isCoding = !_isCoding;
            UpdateEditMode();
            OutText(string.Format("信息：切换为{0}编辑模式", _isCoding ? "代码" : "可视化"), true);
            e.Handled = true;
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
                    gridMain.RowDefinitions[3].Height = new GridLength(160, GridUnitType.Pixel);
                    break;
            }
            e.Handled = true;
        }


        //显示关于窗口
        private void OnHelper(object sender, RoutedEventArgs e)
        {
            new AboutDlg().ShowDialog();
            e.Handled = true;
        }


        //是否可以导出
        private void CanSaveAs(object sender, CanExecuteRoutedEventArgs e)
        {
            //TODO
            e.CanExecute = false;
            e.Handled = true;
        }

        //导出
        private void SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            //TODO
        }

        //是否可以保存
        private void CanSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = FileName.Length > 0;
            e.Handled = true;
        }

        //保存
        private void SaveProject(object sender, ExecutedRoutedEventArgs e)
        {
            if (_isCoding)
            {
                _code = edCode.Text;
            }
            File.WriteAllText(FileName, _code);
            _isModified = false;
            edCode.IsModified = false;
            OutText(string.Format("信息：保存文件【{0}】", FileName), false);
            e.Handled = true;
        }

        //打开
        private void OpenProject(object sender, ExecutedRoutedEventArgs e)
        {
            if (!TipSave()) return;
            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".fio",
                Filter = "FrameIO 文件|*.fio",
                Title = "打开"
            };
            if (ofd.ShowDialog() == true && ofd.FileName != FileName)
            {
                FileName = ofd.FileName;
                _code = File.ReadAllText(FileName);
                edCode.Text = _code;
                _project = new IOProject();
                ResetCodeState();
                OutText(string.Format("信息：打开文件【{0}】", FileName), true);
            }
            UpdateEditMode();
            e.Handled = true;
        }

        //新建
        private void NewProject(object sender, ExecutedRoutedEventArgs e)
        {
            if (!TipSave()) return ;

            var sfd = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".fio",
                Filter = "FrameIO 文件|*.fio",
                Title = "新建"
            };

            if (sfd.ShowDialog() == true)
            {
                if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName);
                FileName = sfd.FileName;
                File.WriteAllText(FileName, "");
                _code = "";
                edCode.Text = _code;
                _project = new IOProject(System.IO.Path.GetFileNameWithoutExtension(sfd.FileName));
                ResetCodeState();
                OutText(string.Format("信息：新建文件【{0}】", FileName), true);
            }
            UpdateEditMode();
            e.Handled = true;

            //_project = new IOProject();
            //_project.SubSysList.Add(new Subsys("SYS1"));
            //_project.SubSysList.Add(new Subsys("SYS2"));
            //_project.SubSysList.Add(new Subsys("SYS3"));
            //_project.FrameList.Add(new Frame("FRAME1"));
            //_project.FrameList.Add(new Frame("FRAME2"));
            //_project.FrameList.Add(new Frame("FRAME3"));
            //_project.FrameList.Add(new Frame("FRAME4"));
            //_project.ProjectName = "PROJECT1";
            //trProject.Root = new ProjectNode(_project);
        }

        //是否可以查找替换
        private void CanFind(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _isCoding;
            e.Handled = true;
        }

        //是否可以删除
        private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            bool b = !_isCoding && FileName.Length > 0;
            var n = trProject.SelectedItem;
            e.CanExecute = b && n!=null && ((ICSharpCode.TreeView.SharpTreeNode)n).CanDelete();
            btRename.IsEnabled = b && n != null && ((ICSharpCode.TreeView.SharpTreeNode)n).IsEditable;
            btAddFrame.IsEnabled = b;
            btAddSubsys.IsEnabled = b;
            btAddEnum.IsEnabled = b;
            e.Handled = true;
        }

        //重命名选中项
        private void RenameSelected(object sender, RoutedEventArgs e)
        {
            ((ICSharpCode.TreeView.SharpTreeNode)trProject.SelectedItem).IsEditing = true;
            e.Handled = true;
        }

        //删除选中项
        private void DeleteSelected(object sender, ExecutedRoutedEventArgs e)
        {
            ((ICSharpCode.TreeView.SharpTreeNode)trProject.SelectedItem).Delete();
            e.Handled = true;
        }

        //添加受控对象
        private void AddSubsys(object sender, RoutedEventArgs e)
        {
            var dlg = new InputDlg();
            dlg.caption.Text = "请输入新受控对象的名称:";
            dlg.Validate = Helper.ValidId;
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                var p = GetSubsysParent().AddChild(dlg.input.Text);
                trProject.SelectedItem = p;
            }
            e.Handled = true;
        }

        //添加数据帧
        private void AddFrame(object sender, RoutedEventArgs e)
        {
            var dlg = new InputDlg();
            dlg.caption.Text = "请输入新建数据帧的名称:";
            dlg.Validate = Helper.ValidId;
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                var p = GetFrameParent().AddChild(dlg.input.Text);
                trProject.SelectedItem = p;
            }
            e.Handled = true;
        }


        //添加枚举
        private void AddEnum(object sender, RoutedEventArgs e)
        {
            var dlg = new InputDlg();
            dlg.caption.Text = "请输入新建枚举的名称:";
            dlg.Validate = Helper.ValidId;
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                var p = GetEnumdefList().AddChild(dlg.input.Text);
                trProject.SelectedItem = p;
            }
            e.Handled = true;
        }

        //全部展开
        private void Expand(object sender, RoutedEventArgs e)
        {
            if (trProject.Root == null) return;
            if(trProject.Root.Children[0].IsExpanded)
            {
                ExpandNode(trProject.Root, false);
            }
            else
            {
                ExpandNode(trProject.Root);
            }

            e.Handled = true;
        }

        //右键菜单
        private void trMain_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            miAddFrame.IsEnabled = btAddFrame.IsEnabled;
            miAddSubsys.IsEnabled = btAddSubsys.IsEnabled;
            miAddEnum.IsEnabled = btAddEnum.IsEnabled;
            miRename.IsEnabled = btRename.IsEnabled;

            var n = trProject.SelectedItem;
            if( n != null)
            {
                var t = n.GetType();
                if(t == typeof(FrameNode) || t == typeof(FrameListNode))
                {
                    miAddFrame.Visibility = Visibility.Visible;
                    miAddSubsys.Visibility = Visibility.Collapsed;
                    miAddEnum.Visibility = Visibility.Collapsed;
                }
                else if(t == typeof(SubsysNode) || t == typeof(SubsysListNode))
                {
                    miAddFrame.Visibility = Visibility.Collapsed;
                    miAddSubsys.Visibility = Visibility.Visible;
                    miAddEnum.Visibility = Visibility.Collapsed;
                }
                else if(t == typeof(EnumdefNode) || t == typeof(EnumdefListNode))
                {
                    miAddFrame.Visibility = Visibility.Collapsed;
                    miAddSubsys.Visibility = Visibility.Collapsed;
                    miAddEnum.Visibility = Visibility.Visible;
                }
                else
                {
                    miAddFrame.Visibility = Visibility.Visible;
                    miAddSubsys.Visibility = Visibility.Visible;
                    miAddEnum.Visibility = Visibility.Visible;
                }
            }
        }

        //查找 替换
        private void FindAndReplace(object sender, ExecutedRoutedEventArgs e)
        {
            FindReplaceDlg.ShowForReplace(edCode);
        }

        #endregion

        #region --Output--

        public void OutText(string info, bool clear)
        {
            if (clear) txtOut.Clear();
            txtOut.AppendText(info);
            txtOut.AppendText(Environment.NewLine);
            txtOut.ScrollToEnd();
        }

        #endregion

        private void onfoobar(object sender, RoutedEventArgs e)
        {
            //txtOut.AppendText(foobar.Add(100, 200).ToString() + Environment.NewLine);

            //byte[] buff = new byte[8];
            //UInt32 x = 0xffff;
            //var bf = BitConverter.GetBytes(x);
            //for(int i=0; i<4; i++)
            //{
            //    buff[i + 4] = bf[i]; 
            //}
            //ulong iresult = Helper.GetUIntxFromByte(buff, 32, 4);
            //txtOut.AppendText(iresult.ToString("x") + Environment.NewLine);

        }


    }


}
