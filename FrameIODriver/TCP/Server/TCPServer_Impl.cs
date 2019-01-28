using FrameIO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrameIO.Driver
{
    public partial class TCPServer_Impl : IChannelBase
    {
        TCPServerHelper TCPServer = null;

        #region IFrameStream
        public bool Open()
        {
            DeviceIsOpen = TCPServer.Open();
            return DeviceIsOpen;
        }
        public bool DeviceIsOpen { get; set; } = false;

        public void InitConfig(Dictionary<string, object> config)
        {
            TCPServer = new TCPServerHelper();
            TCPServer.InitServer(config);
        }

        public void Close()
        {
            TCPServer.CloseServer();
        }

        public void ClearChannel()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IFrameReader
        public ISegmentGettor ReadFrame(IFrameUnpack up)
        {
            int len = up.FirstBlockSize;
            while (len != 0)
                len = up.AppendBlock(TCPServer.BeginReceive2(len));

            return up.Unpack();
        }
        public ISegmentGettor[] ReadFrameList(IFrameUnpack up, int framecount)
        {
            ISegmentGettor[] ret = new ISegmentGettor[framecount];
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

            while (TCPServer.client == null)
                Thread.Sleep(1);
            try
            {
                NetworkStream netStream = new NetworkStream(TCPServer.client);
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

        public bool IsOpen()
        {
            return DeviceIsOpen;
        }


        #endregion

    }
}
