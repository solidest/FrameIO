using ICSharpCode.TreeView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class FrameNode : SharpTreeNode
    {
        private Frame _f;
        public FrameNode(Frame f)
        {
            _f = f;
            LazyLoading = false;
        }

        public Frame TheValue { get => _f; }

        public override object Text
        {
            get => _f.Name;
        }

        public override bool IsEditable
        {
            get
            {
                return true;
            }
        }

        public override string LoadEditText()
        {
            return Text.ToString();
        }

        public override bool SaveEditText(string value)
        {
            var s = Helper.ValidId(value);
            if (s == string.Empty)
            {
                _f.Name = value;
                return true;
            }
            return false;
        }

        public override bool CanDelete()
        {
            return true;
        }

        public override void Delete()
        {
            ((FrameListNode)Parent).DeleteChild(this);
        }

        public override object Icon
        {
            get
            {
                return ImgHelper.GetImage("frame.png");
            }
        }
    }
}
