using ICSharpCode.TreeView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class SubsysListNode:SharpTreeNode
    {
        ObservableCollection<Subsys> _sysList;
        public SubsysListNode(ObservableCollection<Subsys> sysList)
        {
            _sysList = sysList;
            LoadChildren();
            IsExpanded = true;
        }

        public override object Text
        {
            get
            {
                return "受控对象";
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
        public void DeleteChild(SubsysNode fn)
        {
            _sysList.Remove(fn.TheValue);
            Children.Remove(fn);
        }


        //增加字节的
        public SharpTreeNode AddChild(string name)
        {
            var n = new Subsys(name);
            _sysList.Add(n);
            Children.Clear();
            LoadChildren();
            IsExpanded = true;
            foreach(SubsysNode sn in Children)
            {
                if (sn.TheValue == n) return sn;
            }
            return null;
        }

        protected override void LoadChildren()
        {
            foreach (var i in _sysList.OrderBy(p=>p.Name))
            {
                Children.Add(new SubsysNode(i));
            }
        }
    }
}
