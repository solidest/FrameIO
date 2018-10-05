using ICSharpCode.TreeView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class SubsysNode : SharpTreeNode
    {
        private Subsys _subsys;
        public SubsysNode(Subsys subsys)
        {
            _subsys = subsys;
            LazyLoading = false;
        }

        public override object Text
        {
            get => _subsys.Name;
        }

        public Subsys TheValue  { get => _subsys; }

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
                _subsys.Name = value;
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
            ((SubsysListNode)Parent).DeleteChild(this);
        }
        public override object Icon
        {
            get
            {
                return Helper.GetImage("subsys.png");
            }
        }
    }
}
