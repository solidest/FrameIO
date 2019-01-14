using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    public class FioChannel
    {
        private Interface.IChannelBase _ch;

        internal protected FioChannel(Interface.IChannelBase ch, ChannelOption ops)
        {
            _ch = ch;
            _ch.InitConfig(ops.Options);
        }

        public bool IsOpen { get => _ch.IsOpen(); }

        public bool Open()
        {
            return _ch.Open();
        }

        public void Close()
        {
            _ch.Close();
        }

        public void Clear()
        {
            _ch.Close();
        }

        internal void SendFrame(FrameObject value)
        {
            if (value.FrameName == null) throw new FrameIO.Interface.FrameIOException(Interface.FrameIOErrorType.RecvErr, "", "无法发送不完整数据帧");
            _ch.WriteFrame(new FramePacker(value));
        }

        internal FrameObject RecvFrame(string frameName)
        {
            var res = new FrameUnpacker(frameName);
            var o = _ch.ReadFrame(res);
            Debug.Assert(o == res.RootValue);
            return res.RootValue;

        }
    }
}
