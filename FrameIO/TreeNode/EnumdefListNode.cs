using ICSharpCode.TreeView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    class EnumdefListNode : SharpTreeNode
    {
        private ObservableCollection<Enumdef> _enlist;
        public EnumdefListNode(ObservableCollection<Enumdef> enlist)
        {
            _enlist = enlist;
            LoadChildren();
            IsExpanded = true;
        }

        public override object Text
        {
            get
            {
                return "枚举";
            }
        }

        public override object Icon
        {
            get
            {
                return ImgHelper.GetImage("folder.png");
            }
        }

        public override object ExpandedIcon
        {
            get
            {
                return ImgHelper.GetImage("openfolder.png");
            }
        }


        public override bool IsEditable
        {
            get
            {
                return false;
            }
        }


        //删除子节点
        public void DeleteChild(EnumdefNode fn)
        {
            _enlist.Remove(fn.TheValue);
            Children.Remove(fn);
        }

        //增加子节点
        public SharpTreeNode AddChild(string n)
        {
            var newf = new Enumdef(n);
            _enlist.Add(newf);
            Children.Clear();
            LoadChildren();
            IsExpanded = true;
            foreach (EnumdefNode cn in Children)
            {
                if (cn.TheValue == newf)
                {
                    return cn;
                }
            }
            return null;
        }

        protected override void LoadChildren()
        {
            foreach (var i in _enlist.OrderBy(p => p.Name))
            {
                Children.Add(new EnumdefNode(i));
            }
        }
    }
}
