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
            //if (UDPClient.UdpClient != null)
            //    DeviceIsOpen = true;
            //else
            //    DeviceIsOpen = false;

            //return DeviceIsOpen;

            DeviceIsOpen = UDPClient.Open();
            return DeviceIsOpen;
        }
        public bool DeviceIsOpen { get; set; } = false;
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
//         public IFrameData ReadFrame(IFrameUnpack up)
//         {
//             int len = up.FirstBlockSize;
//             while (len != 0)
//                 len = up.AppendBlock(ReadBlock(len));
// 
//             return up.Unpack();
//         }
        public ISegmentGettor ReadFrame(IFrameUnpack up)
        {
            int len = up.FirstBlockSize;
            while (len != 0)
            {
                byte[] readDaata = ReadFixedBlock(len);
                len = up.AppendBlock(readDaata);
            }
                
            return up.Unpack();
        }
        byte[] buffExtra = new byte[65535];
        int buffExtraDataLen= 0;

        private byte[] ReadFixedBlock2(int len)
        {
            try
            {
                while (buffExtraDataLen < len)
                {
                    var readData = UDPClient.ReceiveMsg();
                    Array.Copy(readData, 0, buffExtra, buffExtraDataLen, readData.Length);
                    buffExtraDataLen += readData.Length;
                }

                var ret = new byte[len];
                Array.Copy(ret, buffExtra, len);
                for (int i = len; i < buffExtraDataLen; i++)
                    buffExtra[i - len] = buffExtra[i];
                buffExtraDataLen -= len;
                return ret;
            }
            catch (Exception)
            {
                throw new FrameIO.Interface.FrameIOException(FrameIOErrorType.RecvErr, "UDP客户端", "接收数据超时!");
            }

        }

        private byte[] ReadFixedBlock(int len)
        {
            byte[] buff = new byte[len];
            byte[] recvBuf = new byte[len];
            bool wanttoRecv = true;

            int dataLeft = len;
            int start = 0;
            try
            {
                while (dataLeft > 0)
                {
                    if (buffExtraDataLen != 0)
                    {
                        if (buffExtraDataLen >= dataLeft)
                        {
                            Array.Copy(buffExtra, 0, buff, 0, dataLeft);
                            //Array.Copy(buffExtra, buffExtraDataLen - dataLeft, buffExtra, 0, buffExtraDataLen - dataLeft);
                            for (int i = dataLeft; i < buffExtraDataLen; i++)
                            {
                                buffExtra[i - dataLeft] = buffExtra[i];
                            }
                            buffExtraDataLen = buffExtraDataLen - dataLeft;
                            start += dataLeft;
                            dataLeft -= dataLeft;
                            wanttoRecv = false;
                        }
                        else
                        {
                            Array.Copy(buffExtra, 0, buff, 0, buffExtraDataLen);
                            start += buffExtraDataLen;
                            dataLeft -= buffExtraDataLen;
                            //if (dataLeft > 0)
                            wanttoRecv = true;
                            //else
                            //    wanttoRecv = false;
                            buffExtraDataLen = 0;
                        }
                    }
                    if (wanttoRecv)
                    {
                        var readData = UDPClient.ReceiveMsg();

                        if (readData.Length > len - start)
                        {
                            Array.Copy(readData, 0, buff, start, len - start);
                            buffExtraDataLen = readData.Length - (len - start);
                            Array.Copy(readData, len - start, buffExtra, 0, buffExtraDataLen);

                        }
                        else
                            Array.Copy(readData, 0, buff, start, readData.Length);

                        start += readData.Length;
                        dataLeft -= readData.Length;
                    }
                }
                return buff;
            }
            catch (Exception)
            {
                throw new FrameIO.Interface.FrameIOException(FrameIOErrorType.RecvErr, "UDP客户端", "接收数据超时!");
            }

        }
        private byte[] ReadBlock(int len)
        {
            byte[] buff = new byte[len];
            byte[] recvBuf = new byte[len];

            int dataLeft = len;
            int start = 0;
            while (dataLeft > 0)
            {
                var readData = UDPClient.ReceiveMsg();

                if (readData.Length > len - start)
                    Array.Copy(readData, 0, buff, start, len - start);
                else
                    Array.Copy(readData, 0, buff, start, readData.Length);

                start += readData.Length;
                dataLeft -= readData.Length;
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

        public bool IsOpen()
        {
            return DeviceIsOpen;
        }


        #endregion



    }
}
