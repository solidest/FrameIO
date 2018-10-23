using ICSharpCode.TreeView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class EnumdefNode : SharpTreeNode
    {
        private Enumdef _en;
        public EnumdefNode(Enumdef en)
        {
            _en = en;
            LazyLoading = false;
        }

        public Enumdef TheValue { get => _en; }

        public override object Text
        {
            get => _en.Name;
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
                _en.Name = value;
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
            ((EnumdefListNode)Parent).DeleteChild(this);
        }

        public override object Icon
        {
            get
            {
                return ImgHelper.GetImage("enum1.png");
            }
        }
    }
}
