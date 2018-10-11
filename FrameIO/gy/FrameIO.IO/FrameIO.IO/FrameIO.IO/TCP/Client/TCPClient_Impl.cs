using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;

namespace FrameIO.Driver
{
    partial class TCPClient_Impl : IFrameStream, IFrameReader, IFrameWriter
    {
        TCPClientHelper TCPClient;
        #region IFrameStream
        public bool Open(Dictionary<string, object> config)
        {
            TCPClient = new TCPClientHelper();
            TCPClient.InitClient();
            if (TCPClient.client != null)
                return true;

            return false;
        }

        public void Close()
        {
            TCPClient.CloseClient();
        }

        public void ClearChannel()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IFrameReader
        public FrameBase ReadFrame(IFrameUnpack up)
        {
            int len = up.FirstBlockSize;
            while (len != 0)
                len = up.AppendBlock(ReadBlock(len));

            return up.Unpack();
        }
        private Byte[] ReadBlock(int len)
        {
            byte[] buff = new byte[len];
            int dataleft = len;
            int start = 0;
            NetworkStream netStream = new NetworkStream(TCPClient.client);

            while(dataleft>0)
            {
                int recv = netStream.Read(buff, start, dataleft);
                start += recv;
                dataleft -= recv;

            }
            return buff;
        }
        public FrameBase[] ReadFrameList(IFrameUnpack up, int framecount)
        {
            FrameBase[] ret = new FrameBase[framecount];
            for (int i = 0; i < framecount; i++)
            {
                ret[i] = ReadFrame(up);
            }
            return ret;
        }

        public void BeginReadFrame(IFrameUnpack up, AsyncReadCallback callback, object AsyncState)
        {
            BeginReadFrameImpl(up, callback, AsyncState);
        }

        public void BeginReadFrameList(IFrameUnpack up, int framecount, bool isloop, AsyncReadListCallback callback, object AsyncState)
        {
            BeginReadFrameListImpl(up, framecount, isloop, callback, AsyncState);
        }
        #endregion

        #region IFrameWriter
        public int WriteFrame(IFramePack p)
        {
            byte[] buff = p.Pack();
            try
            {
                NetworkStream netStream = new NetworkStream(TCPClient.client);
                netStream.Write(buff, 0, buff.Length);
                return 1;
            }
            catch { return 0; }
        }

        public int WriteFrameList(IFramePack[] p, int len)
        {
            int ret = 0;

            for (int i = 0; i < len; i++)
            {
                if (WriteFrame(p[i]) == 1)
                    ret += 1;
            }
            return ret;
        }
        public void BeginWriteFrame(IFramePack p, AsyncWriteCallback callback, object AsyncState)
        {
            BeginWriteFrameImpl(p, callback, AsyncState);
        }

        public void BeginWriteFrameList(IFramePack[] p, int len, AsyncWriteCallback callback, object AsyncState)
        {
            BeginWriteFrameList(p, len, callback, AsyncState);
        }

        #endregion

    }
}
