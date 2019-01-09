using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal class FramePacker : Interface.IFramePack
    {
        private FrameObject _v;
        internal FramePacker(FrameObject value)
        {
            _v = value;
        }

        public byte[] Pack()
        {
            var buff = new FrameSendBuffer();
            var frm = IORunner.GetFrame(_v.FrameName);
            var res = frm.Pack(buff, _v.RootValue);
            Debug.Assert(res == null);
            return buff.Flush();
        }
    }
}
