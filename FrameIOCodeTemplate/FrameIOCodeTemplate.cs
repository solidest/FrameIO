using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;

namespace FrameIO.CodeTemplate
{
    public class CANChannel : IFrameReader, IFrameWriter, IFrameStream
    {
        public void BeginReadFrame(IFrameUnpack up, AsyncReadCallback callback)
        {
            throw new NotImplementedException();
        }

        public void BeginReadFrameList(IFrameUnpack up, int framecount, bool isloop, AsyncReadListCallback callback)
        {
            throw new NotImplementedException();
        }

        public void BeginWriteFrame(IFramePack p, AsyncWriteCallback callback, object AsyncState)
        {
            throw new NotImplementedException();
        }

        public void ClearChannel()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public bool Open(Dictionary<string, object> config)
        {
            throw new NotImplementedException();
        }

        public FrameBase ReadFrame(IFrameUnpack up)
        {
            throw new NotImplementedException();
        }

        public FrameBase[] ReadFrameList(IFrameUnpack up, int framecount)
        {
            throw new NotImplementedException();
        }

        public int WriteFrame(IFramePack p)
        {
            throw new NotImplementedException();
        }
    }

    public class COMChannel : IFrameStream
    {
        public void ClearChannel()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public bool Open(Dictionary<string, object> config)
        {
            throw new NotImplementedException();
        }
    }
}
