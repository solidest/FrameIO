using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;

namespace FrameIO.Driver
{
    public partial class UDPClient_Impl : IChannelBase
    {
        UDPHelper UDPClient = null;

        #region IFrameStream
        public bool Open()
        {
            if (UDPClient.client != null)
                return true;

            return false;
        }

        public void InitConfig(Dictionary<string, object> config)
        {
            UDPClient = new UDPHelper();
            UDPClient.InitClient(config);
        }

        public void Close()
        {
            UDPClient.CloseUDPClient();
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
                len = up.AppendBlock(ReadBlock(len));

            return up.Unpack();
        }
        private byte[] ReadBlock(int len)
        {
            byte[] buff = new byte[len];
            byte[] recvBuf = new byte[len];


            int dataLeft = len;
            int start = 0;
            while (dataLeft > 0)
            {
                int recv = UDPClient.client.ReceiveFrom(recvBuf, ref UDPClient.point);

                if (recv > len - start)
                    Array.Copy(recvBuf, 0, buff, start, len - start);
                else
                    Array.Copy(recvBuf, 0, buff, start, recv);

                start += recv;
                dataLeft -= recv;

            }
            return buff;

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
            try
            {
                UDPClient.sendMsg(buff);
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
