using FrameIO.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace FrameIO.Driver
{
    public partial class Com_Impl : IChannelBase
    {
        ComHelper Com = null;
        #region IFrameStream
        public bool Open()
        {
            if (!Com.Open())
                return false;
            return true;
        }

        public void InitConfig(Dictionary<string, object> config)
        {
            Com = new ComHelper();
            Com.InitPort(config);
        }

        public void Close()
        {
            Com.CloseCom();
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

            while(len!=0)
                len = up.AppendBlock(ReadBlock(len));

            return up.Unpack();
        }
        private Byte[] ReadBlock(int len)
        {
            Byte[] buff = new byte[len];
            Stopwatch watcher = new Stopwatch();
            watcher.Start();
            while (Com.RS232.BytesToRead < len)
            {
                if (watcher.ElapsedMilliseconds > Com.ReceiveTimeOut)
                {
                    watcher.Stop();
                    throw new FrameIO.Interface.FrameIOException(FrameIOErrorType.RecvErr, "接收串口数据", "数据接收超时！");
                }
                    
                System.Threading.Thread.Sleep(1);
            }

            Com.RS232.Read(buff, 0, len);
            return buff;
        }
        public ISegmentGettor[] ReadFrameList(IFrameUnpack up, int framecount)
        {
            ISegmentGettor[] ret = new ISegmentGettor[framecount];
            for (int i=0;i< framecount;i++)
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
            //byte[] buff = new byte[p.ByteSize];
            byte[] buff = p.Pack();
            if (Com.DoWrite(buff))
                return 1;
            return 0;
        }

        public int WriteFrameList(IFramePack[] p, int len)
        {
            int ret = 0;

            for(int i=0;i<len;i++)
            {
                if(WriteFrame(p[i])==1)
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
