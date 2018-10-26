using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using FrameIO.Interface;

namespace FrameIO.Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, IOutText
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadEditorConfig();

            Thread parseThread = new Thread(this.Parse);
            parseThread.IsBackground = true;
            parseThread.Start();

            DispatcherTimer fUpdateTimer = new DispatcherTimer();
            fUpdateTimer.Interval = TimeSpan.FromSeconds(1);
            fUpdateTimer.Tick += delegate { UpdateTimer(); };
            fUpdateTimer.Start();

            _isCoding = true;
            UpdateEditMode();
        }

        private IOProject _project;
        private bool _isCoding = false;
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
                Title = "FrameIO - " +  Path.GetFileNameWithoutExtension(value);
                __file = value;
            }
        }

        #region --Parse--

        private string _parseCode = "";
        private int _codeVersion = 0;
        private int _workedVersion = 0;
        private int _lastprojectid = -1;
        private bool _lastparseok = false;
        private ParseDb _db = new ParseDb();
        private Mutex _parseMutext = new Mutex(false);

        private delegate void ParseErrorHandler(int codeVer, IList<ParseError> errorlist);

        //挂起后台分析线程 
        private void SuspendBackgroundParse()
        {
            ParseCode();
            Thread.Sleep(1);
            _parseMutext.WaitOne();
            while (_workedVersion != _codeVersion)
            {
                _parseMutext.ReleaseMutex();
                Thread.Sleep(1);
                _parseMutext.WaitOne();
            }
        }

        //恢复后台分析线程
        private void RecoveryBackgroundParse()
        {
            _parseMutext.ReleaseMutex();
        }

        //分析代码 被主线程调用的接口
        private void ParseCode()
        {
            if (edCode.Text == _parseCode) return;
            lock (this)
            {
                _parseCode = edCode.Text; //保存代码
                _codeVersion += 1; //更新版本
            }
        }

        //分析线程的入口
        private void Parse()
        {
            string workCode = "";
            int workVersion = -1;
            int projectid = -1;
            while (true)
            {
                _parseMutext.WaitOne();
                if (_workedVersion != _codeVersion)
                {
                    lock (this)
                    {
                        workCode = _parseCode;
                        workVersion = _codeVersion;
                    }
                    if (projectid != -1) _db.DeleteProject(projectid);
                    UTF8Encoding utf8 = new UTF8Encoding(false);
                    workCode = GetUTF8String(utf8.GetBytes(workCode));
                    projectid = _db.CreateProject(workCode);
                    _lastprojectid = projectid;
                    int iret = ParseDb.parse(projectid);
                    if (iret>1) throw new Exception(string.Format("错误代码【{0}】:解析器启动失败", iret));
                    //加载错误信息
                    var errlist = _db.LoadError(projectid);
                    lock(this)
                    {
                        _lastparseok = (errlist.Count == 0);
                        _workedVersion = workVersion;
                    }
                    Dispatcher.BeginInvoke(new ParseErrorHandler(ShowParseError), _workedVersion, errlist);
                }
                _parseMutext.ReleaseMutex();
                Thread.Sleep(1);
            }
        }
        

        //转换成UTF8编码
        private static string GetUTF8String(byte[] buffer)
        {
            if (buffer == null)
                return null;

            if (buffer.Length <= 3)
            {
                return Encoding.UTF8.GetString(buffer);
            }

            byte[] bomBuffer = new byte[] { 0xef, 0xbb, 0xbf };

            if (buffer[0] == bomBuffer[0]
                && buffer[1] == bomBuffer[1]
                && buffer[2] == bomBuffer[2])
            {
                return new UTF8Encoding(false).GetString(buffer, 3, buffer.Length - 3);
            }

            return Encoding.UTF8.GetString(buffer);
        }

        #endregion

        #region --Event--

        //分析结果信息输出
        private void ShowParseError(int codeVer, IList<ParseError> errorlist)
        {
            if (codeVer != _codeVersion) return;
            if (errorlist == null || errorlist.Count == 0)
            {
                ClearErrTip();
                OutText("", true);
                return;
            }

            IServiceProvider sp = edCode;
            var markerService = (TextMarkerService)sp.GetService(typeof(TextMarkerService));
            markerService.Clear();

            OutText("", true);
            foreach (var err in errorlist)
            {
                var of1 = edCode.Document.GetOffset(err.FirstLine, err.FirstCol);
                var of2 = edCode.Document.GetOffset(err.LastLine, err.LastCol);
                if (of2 >= of1) textMarkerService.Create(of1, of2 - of1 + 1, err.ErrorTip);
                OutText(string.Format("错误：{0}, 行号{2} 列号{3}", err.ErrorTip, err.ErrorCode, err.FirstLine, err.FirstCol), false);
            }

        }

        #region --error tip--
        private void VisualinesChanged(object sender, EventArgs e)
        {
            if (toolTip != null)
            {
                toolTip.IsOpen = false;
            }
        }

        private void CodeEditorMouseHoverStopped(object sender, MouseEventArgs e)
        {
            if (toolTip != null)
            {
                toolTip.IsOpen = false;
                e.Handled = true;
            }
        }

        private void ToolTipClosed(object sender, RoutedEventArgs e)
        {
            toolTip = null; ;
        }

        private void MouseHover(object sender, MouseEventArgs e)
        {
            var textEditor = edCode;
            var pos = textEditor.TextArea.TextView.GetPositionFloor(e.GetPosition(textEditor.TextArea.TextView) + textEditor.TextArea.TextView.ScrollOffset);
            bool inDocument = pos.HasValue;
            if (inDocument)
            {
                TextLocation logicalPosition = pos.Value.Location;
                int offset = textEditor.Document.GetOffset(logicalPosition);

                var markersAtOffset = textMarkerService.GetMarkersAtOffset(offset);
                TextMarkerService.TextMarker markerWithToolTip = markersAtOffset.FirstOrDefault(marker => marker.ToolTip != null);


                if (markerWithToolTip != null)
                {
                    if (toolTip == null)
                    {
                        toolTip = new ToolTip();
                        toolTip.Closed += ToolTipClosed;
                        toolTip.PlacementTarget = this;
                        toolTip.Content = new TextBlock
                        {
                            Text = markerWithToolTip.ToolTip,
                            TextWrapping = TextWrapping.Wrap
                        };
                        toolTip.IsOpen = true;
                        e.Handled = true;
                    }
                    else
                    {
                        toolTip.Content = new TextBlock
                        {
                            Text = markerWithToolTip.ToolTip,
                            TextWrapping = TextWrapping.Wrap
                        };
                    }
                }
            }
        }
        #endregion

        //定时器更新
        void UpdateTimer()
        {
            ParseCode();
            if (_isCoding)
            {
                _foldingStrategy.UpdateFoldings(_foldingManager, edCode.Document);
            }
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

        //窗口关闭之前
        private void OnBeforeClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_lastprojectid>0)
            {
                _db.DeleteProject(_lastprojectid);
            }

            if (_isCoding)
            {
                _isModified = edCode.IsModified;
            }

            if (_isModified)
            {
                var res = MessageBox.Show(this, "是否保存对当前文档的更改？", "FrameIO", MessageBoxButton.YesNoCancel);
                switch (res)
                {
                    case MessageBoxResult.Yes:
                        File.WriteAllText(FileName, edCode.Text);
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

        private FoldingManager _foldingManager;
        private CodeFolding _foldingStrategy;
        private TextMarkerService textMarkerService;
        private ToolTip toolTip;
        private const string DefaultCode = "//项目:{0}\nproject main\n{\n\t//受控对象\n\tsystem subsys1\n\t{\n\t\n\t}\n\t//数据帧\n\tframe frame1\n\t{\n\t\n\t}\n}";
        
        //输出一条错误信息
        private void OutOneError(Dictionary<int, string> infos)
        {
            var errl = new List<ParseError>();
            foreach(var erri in infos)
            {
                var err = _db.GetError(erri.Key, 100);
                err.ErrorInfo = erri.Value;
                errl.Add(err);
            }
            ShowParseError(_workedVersion, errl);
        }

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

            //error tip
            textMarkerService = new TextMarkerService(edCode);
            var tv = edCode.TextArea.TextView;
            tv.BackgroundRenderers.Add(textMarkerService);
            //tv.LineTransformers.Add(textMarkerService);
            tv.Services.AddService(typeof(TextMarkerService), textMarkerService);
            tv.MouseHover += MouseHover;
            tv.MouseHoverStopped += CodeEditorMouseHoverStopped;
            tv.VisualLinesChanged += VisualinesChanged;
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
                btExpand.Visibility = Visibility.Collapsed;
                btAddEnum.Visibility = Visibility.Collapsed;

                btCopy.Visibility = Visibility.Visible;
                btCut.Visibility = Visibility.Visible;
                btPaste.Visibility = Visibility.Visible;
                btUndo.Visibility = Visibility.Visible;
                btRedo.Visibility = Visibility.Visible;
                btFindReplace.Visibility = Visibility.Visible;

                edCode.IsEnabled = (__file!="");
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
                btExpand.Visibility = Visibility.Visible;
                btAddEnum.Visibility = Visibility.Visible;

                btCopy.Visibility = Visibility.Collapsed;
                btCut.Visibility = Visibility.Collapsed;
                btPaste.Visibility = Visibility.Collapsed;
                btUndo.Visibility = Visibility.Collapsed;
                btRedo.Visibility = Visibility.Collapsed;
                btFindReplace.Visibility = Visibility.Collapsed;

                edCode.IsEnabled = false;
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
            _project = null;
            lock(this)
            {
                _codeVersion += 1;
            }
            ReLoadProjectToUI(!_isCoding);
        }

        //提示保存
        private bool TipSave()
        {
            if (_isCoding)
            {
                _isModified = edCode.IsModified;
            }
            if(_isModified)
            {
                var res = MessageBox.Show(this, "是否保存对当前文档的更改？", "FraeIO", MessageBoxButton.YesNoCancel);
                switch (res)
                {
                    case MessageBoxResult.Yes:
                        File.WriteAllText(FileName, edCode.Text);
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

        //清除错误提示
        private void ClearErrTip()
        {
            IServiceProvider sp = edCode;
            var markerService = (TextMarkerService)sp.GetService(typeof(TextMarkerService));
            markerService.Clear();
        }


        #endregion

        #region --Command--

        //代码检查
        private bool DoCheckCode()
        {
             //TODO if (!_isCoding) edCode.Text = _project.CreateCode();
            SaveProject(this, null);

            bool ret = ReLoadProjectToUI(false, true);

            if(ret)
            {
                ret = FrameIOCodeCheck.CheckProject(_project);
                if (!ret) OutOneError(FrameIOCodeCheck.ErrorList);
            }

            if (HSplitter.Visibility != Visibility.Visible) OutDispHide(this, null);
            OutText(string.Format("信息：代码检查{0}", ret?"成功":"失败"), false);
            return ret;
        }

        //代码检查
        private void CheckCode(object sender, RoutedEventArgs e)
        {
            if (FileName.Length == 0) return;
            DoCheckCode();
        }

        //切换视图
        private void SwitchView(object sender, RoutedEventArgs e)
        {
            if(FileName.Length != 0)
            {
                if(_isCoding)
                {
                    if(!ReLoadProjectToUI(true)) return;
                    SuspendBackgroundParse();
                }
                else
                {
                    //TODO if(_project!=null)edCode.Text = _project.CreateCode();
                    RecoveryBackgroundParse();
                }
            }
            else
            {
                if (_isCoding)
                    _parseMutext.WaitOne();
                else
                    _parseMutext.ReleaseMutex();
            }

            _isCoding = !_isCoding;
            UpdateEditMode();
            OutText(string.Format("信息：切换为{0}编辑模式", _isCoding ? "代码" : "可视化"), false);

            if(e!=null) e.Handled = true;
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
            if(e!=null) e.Handled = true;
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
            e.CanExecute = (FileName != "");
            btCheckCode.IsEnabled = e.CanExecute;
            if (e.Parameter.ToString() == "cpp") e.CanExecute = false;
            e.Handled = true;
        }

        //导出
        private void SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            if(e.Parameter.ToString() == "csharp")
            {
                if (FileName.Length == 0) return;
                if (!DoCheckCode())
                {
                    OutText("信息：无法启动代码输出", false);
                    return;
                }
                else
                {
                    FrameIOCodeGenerator.GenerateCodeFile(_project, this);
                }
            }

            
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
            File.WriteAllText(FileName, edCode.Text);
            _isModified = false;
            edCode.IsModified = false;
            if (e != null)
            {
                OutText(string.Format("信息：保存文件【{0}】", FileName), false);
                e.Handled = true;
            }
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
                edCode.Text = File.ReadAllText(FileName);
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
                var code = DefaultCode.Replace("{0}", System.IO.Path.GetFileNameWithoutExtension(FileName));
                File.WriteAllText(FileName, code);
                edCode.Text = code;
                ResetCodeState();
                OutText(string.Format("信息：新建文件【{0}】", FileName), true);
            }
            UpdateEditMode();
            e.Handled = true;

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

        #region --BindingData--

        //重新加载整个项目到UI startUI是否为了启动可视化界面 checkSemantics是否为了语义检查
        bool ReLoadProjectToUI(bool startUI, bool checkSemantics = false)
        {
            bool ret = true;
            bool needRelease = false;
            if (_isCoding)
            {
                SuspendBackgroundParse();
                ret = _lastparseok;
                needRelease = true;
            }
            else
            {
                RecoveryBackgroundParse();
                Thread.Sleep(1);
                SuspendBackgroundParse();
                ret = _lastparseok;
            }

            IOProject p = null;
            if(ret)
            {
                IList<ParseError> errorlist = null;
                p = _db.LoadProject(_lastprojectid, out errorlist, checkSemantics);
                if (p == null)
                {
                    ret = false;
                    ShowParseError(_workedVersion, errorlist);
                }
            }

            if(ret)
            {
                _project = p;
                trProject.Root = new ProjectNode(_project);
            }

            if (!ret)
            {
                if (!_isCoding) SwitchView(this, null);
                if (HSplitter.Visibility != Visibility.Visible) OutDispHide(this, null);
                if(startUI) OutText(string.Format("警告：无法启动可视化编辑，请修正代码错误"), false);
            }

            if(needRelease) RecoveryBackgroundParse();

            return ret;
        }



        #endregion

        #region --Output--

        public void OutText(string info, bool clear)
        {
            if (clear) txtOut.Clear();
            if (info == "") return;
            txtOut.AppendText(info);
            if (!info.EndsWith(Environment.NewLine)) txtOut.AppendText(Environment.NewLine);
            txtOut.ScrollToEnd();
        }


        public string GetMainOutPath()
        {
            return Path.GetDirectoryName(__file);
        }



        #endregion

        private void ClearOut(object sender, RoutedEventArgs e)
        {
            OutText("", true);
        }
    }


}
