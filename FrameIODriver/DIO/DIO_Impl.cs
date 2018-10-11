using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;

namespace FrameIO.Driver
{
    partial class DIO_Impl : IFrameStream, IFrameReader, IFrameWriter
    {
        DIHelper CardDIO = null;
        bool isOpen = false;

        #region IFrameStream
        public bool Open()
        {
            return isOpen;
        }

        public void InitConfig(Dictionary<string, object> config)
        {
            CardDIO = new DIHelper();
            isOpen=CardDIO.OpenDIO(config);
        }
        
        public void Close()
        {
            throw new NotImplementedException();
        }

        public void ClearChannel()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IFrameReader
        public IFrameData ReadFrame(IFrameUnpack up)
        {
            throw new NotImplementedException();
        }

        public IFrameData[] ReadFrameList(IFrameUnpack up, int framecount)
        {
            throw new NotImplementedException();
        }

        public void BeginReadFrame(IFrameUnpack up, AsyncReadCallback callback, object AsyncState)
        {
            throw new NotImplementedException();
        }

        public void BeginReadFrameList(IFrameUnpack up, int framecount, bool isloop, AsyncReadListCallback callback, object AsyncState)
        {
            throw new NotImplementedException();
        }



        #endregion

        #region IFrameWriter
        public int WriteFrame(IFramePack p)
        {
            throw new NotImplementedException();
        }

        public int WriteFrameList(IFramePack[] p, int len)
        {
            throw new NotImplementedException();
        }

        public void BeginWriteFrame(IFramePack p, AsyncWriteCallback callback, object AsyncState)
        {
            throw new NotImplementedException();
        }

        public void BeginWriteFrameList(IFramePack[] p, int len, AsyncWriteCallback callback, object AsyncState)
        {
            throw new NotImplementedException();
        }



        #endregion
    }
}
