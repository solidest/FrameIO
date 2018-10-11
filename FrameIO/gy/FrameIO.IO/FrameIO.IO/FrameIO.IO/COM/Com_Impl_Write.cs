using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FrameIO.Interface;

namespace FrameIO.Driver
{
    partial class Com_Impl
    {
        private void DoAsyncWrite(object wr)
        {
            var p = (AsyncWriteInfo)wr;
            int count = WriteFrame(p.packer);
            p.callback.Invoke(count, p.AsyncState);

        }

        private void DoAsyncWriteList(object wr)
        {
            var p = (AsyncWriteInfoList)wr;
            int count = WriteFrameList(p.packer, p.len);
            p.callback.Invoke(count, p.AsyncState);
        }
        private void BeginWriteFrameImpl(IFramePack p, AsyncWriteCallback callback, object AsyncState)
        {
            AsyncWriteInfo info;
            info.packer = p;
            info.callback = callback;
            info.AsyncState = AsyncState;

            new Thread(new ParameterizedThreadStart(DoAsyncWrite)).Start(info);
        }

        private void BeginWriteFrameListImpl(IFramePack[] p, int len, AsyncWriteCallback callback, object AsyncState)
        {
            AsyncWriteInfoList info;
            info.packer = p;
            info.callback = callback;
            info.AsyncState = AsyncState;
            info.len = len;

            new Thread(new ParameterizedThreadStart(DoAsyncWriteList)).Start(info);
        }
    }
}
