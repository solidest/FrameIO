using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class ProjectNode  : ICSharpCode.TreeView.SharpTreeNode
    {
        private IOProject _project;
        public ProjectNode(IOProject pj)
        {
            _project = pj;
            LoadChildren();
        }
        public override object Text
        {
            get
            {
                return _project.Name ;
            }
        }

        public override object Icon
        {
            get
            {
                return ImgHelper.GetImage("project.png");
            }
        }


        public override bool IsEditable
        {
            get
            {
                return false;
            }
        }

        protected override void LoadChildren()
        {
            Children.Add(new SubsysListNode(_project.SubsysList));
            Children.Add(new FrameListNode(_project.FrameList));
            Children.Add(new EnumdefListNode(_project.EnumdefList));
        }
    }
}
