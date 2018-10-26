using System;
using System.Collections.Generic;
using System.Threading;
using FrameIO.Interface;

namespace FrameIO.Driver
{
    public partial class YH_CAN_Impl : PhysicalChannel, IChannelBase
    {
        #region IFrameStream
        public bool Open()
        {
            //InitConfig(config);

            if (!OpenCan()) return false;
            if (!EnterResetMode()) return false;
            if (!SetTimeOut()) return false;
            if (!SetSelfReception()) return false;
            if (!EnterWorkMode()) return false;
            if (!SetAccmarkAccCode()) return false;

            return true;
        }

        public void Close()
        {
            Int32 nRet = DevCan.acCanClose();
            if (nRet < 0)
            {
                Console.WriteLine("关闭CAN 失败!");
            }
        }

        public void ClearChannel()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IFrameReader
        public ISegmentGettor ReadFrame(IFrameUnpack up)
        {
            canmsg_t[] msgRead = new canmsg_t[1];
            for (System.Int32 i = 0; i < msgRead.Length; i++)
            {
                msgRead[i].data = new Byte[8];
            }
            UInt32 pulNumberofRead = 0;
            Int32 nRet = 0;
            while (pulNumberofRead <= 0)
            {
                System.Threading.Thread.Sleep(1);
                nRet = DevCan.acCanRead(msgRead, (UInt32)msgRead.Length, ref pulNumberofRead);

                if (nRet == AdvCANIO.OPERATION_ERROR)
                {
                    Console.WriteLine("接收失败：操作失败!");
                    return null;
                }
                nRet = DevCan.acCanRead(msgRead, (UInt32)msgRead.Length, ref pulNumberofRead);
            }
            var lst = new List<System.Byte>();
            for (System.Int32 i = 0; i < 1; ++i)
            {
                var arr = PVCI_CAN_OBJ_ToBytes(msgRead[i]);
                lst.AddRange(arr);
            }
            up.AppendBlock(lst.ToArray());
            return up.Unpack();
        }

        public ISegmentGettor[] ReadFrameList(IFrameUnpack up, int framecount)
        {
            var ret = new ISegmentGettor[framecount];

            canmsg_t[] msgRead = new canmsg_t[framecount];
            for (System.Int32 i= 0;i< msgRead.Length;i++)
            {
                msgRead[i].data = new byte[8];
            }
            UInt32 pulNumberofRead = 0;
            Int32 nRet = 0;
            UInt32 hasRead = 0;
            UInt32 notRead = (UInt32)framecount;
            while(hasRead <= notRead)
            {
                nRet = DevCan.acCanRead(msgRead, (UInt32)msgRead.Length, ref pulNumberofRead);

                if (nRet == AdvCANIO.OPERATION_ERROR)
                {
                    Console.WriteLine("接收失败：操作失败!");
                    return null;
                }

                var lst = new List<System.Byte>();
                for(System.UInt32 i= hasRead; i< hasRead+pulNumberofRead; i++)
                {
                    lst.Clear();
                    var arr= PVCI_CAN_OBJ_ToBytes(msgRead[i]);
                    lst.AddRange(arr);
                    up.AppendBlock(lst.ToArray());
                    ret[i] = up.Unpack();
                }

                hasRead += pulNumberofRead;
                notRead = notRead - pulNumberofRead;
            }
            
            return ret;
        }
        public void BeginReadFrame(IFrameUnpack up, AsyncReadCallback callback, object AsyncState)
        {
            BeginReadFrameImpl( up,  callback,  AsyncState);
        }
        public void BeginReadFrameList(IFrameUnpack up, int framecount, bool isloop, AsyncReadListCallback callback, object AsyncState)
        {
            BeginReadFrameListImpl( up,  framecount,  isloop,  callback,  AsyncState);
        }

        #endregion

        #region IFrameWriter
        public int WriteFrame(IFramePack p)
        {
            byte[] buff = p.Pack();
            if (DoWrite(buff))
                return 1;
            return 0;
        }
        public int WriteFrameList(IFramePack[] p, int len)
        {
            Int32 ret = 0;

            canmsg_t[] sendFrames = new canmsg_t[len];
            for(int i=0;i<len;i++)
            {
                byte[] sendFrame = p[i].Pack();
                sendFrames[i] = ParseFrame(sendFrame);

            }
            UInt32 pulNumberofWritten = 0;
            ret = DevCan.acCanWrite(sendFrames, (uint)len, ref pulNumberofWritten);//CAN DATA SEND

            if (ret == AdvCANIO.TIME_OUT)
                throw new Exception("研华CAN卡发送失败：发送超时!");
            if (ret == AdvCANIO.OPERATION_ERROR)
                throw new Exception("研华CAN卡发送失败：操作失败");

            return (int)pulNumberofWritten;
        }
        public void BeginWriteFrame(IFramePack p, AsyncWriteCallback callback, object AsyncState)
        {
            BeginWriteFrameImpl(p, callback, AsyncState);
        }

        public void BeginWriteFrameList(IFramePack[] p, int len, AsyncWriteCallback callback, object AsyncState)
        {
            BeginWriteFrameListImpl(p, len, callback, AsyncState);        
        }

        #endregion

    }
}