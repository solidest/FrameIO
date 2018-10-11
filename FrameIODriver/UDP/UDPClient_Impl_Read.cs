using FrameIO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrameIO.Driver
{
    partial class UDPClient_Impl
    {
        private void DoAsyncRead(object wr)
        {
            lock (this)
            {
                var p = (AsyncReadInfo)wr;
                IFrameData frameBase = ReadFrame(p.packer);
                p.callback.Invoke(frameBase, p.AsyncState);
            }
        }
        private void DoAsyncReadList(object wr)
        {
            lock (this)
            {
                var p = (AsyncReadInfoList)wr;
                do
                {
                    IFrameData[] frameBase = ReadFrameList(p.packer, p.framecount);
                    bool isCompleted = false;
                    p.callback.Invoke(frameBase, out isCompleted, p.AsyncState);
                    if (isCompleted) return;
                } while (p.isloop);
            }
        }
        private void BeginReadFrameImpl(IFrameUnpack up, AsyncReadCallback callback, object AsyncState)
        {
            AsyncReadInfo info;
            info.packer = up;
            info.callback = callback;
            info.AsyncState = AsyncState;

            new Thread(new ParameterizedThreadStart(DoAsyncRead)).Start(info);
        }

        private void BeginReadFrameListImpl(IFrameUnpack up, int framecount, bool isloop, AsyncReadListCallback callback, object AsyncState)
        {

            AsyncReadInfoList info;

            info.packer = up;
            info.callback = callback;
            info.AsyncState = AsyncState;
            info.framecount = framecount;
            info.isloop = isloop;

            new Thread(new ParameterizedThreadStart(DoAsyncReadList)).Start(info);
        }

    }
}
