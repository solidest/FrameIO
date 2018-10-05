using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.TreeView;

namespace FrameIO.Main
{
    public class FrameListNode:SharpTreeNode
    {
        private ObservableCollection<Frame> _flist;
        public FrameListNode(ObservableCollection<Frame> flist)
        {
            _flist = flist;
            LoadChildren();
            IsExpanded = true;
        }

        public override object Text
        {
            get
            {
                return "数据帧";
            }
        }

        public override object Icon
        {
            get
            {
                return Helper.GetImage("folder.png");
            }
        }

        public override object ExpandedIcon
        {
            get
            {
                return Helper.GetImage("openfolder.png");
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
        public void DeleteChild(FrameNode fn)
        {
            _flist.Remove(fn.TheValue);
            Children.Remove(fn);
        }

        //增加子节点
        public SharpTreeNode AddChild(string n)
        {
            var newf = new Frame(n);
            _flist.Add(newf);
            Children.Clear();
            LoadChildren();
            IsExpanded = true;
            foreach (FrameNode cn in Children)
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
            foreach(var i in _flist.OrderBy(p=>p.Name))
            {
                Children.Add(new FrameNode(i));
            }
        }
    }
}
