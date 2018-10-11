using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameIO.Driver
{
    partial class YH_CAN_Impl
    {
        protected override System.Boolean DoWriteExternFrame(Byte DataLen, Boolean RemoteFlag, UInt32 id, Byte[] userdata)
        {
            canmsg_t sendbuf = InitExtendeFrameFlags(DataLen, RemoteFlag, id, userdata);

            UInt32 pulNumberofWritten = 0;
            Int32 nRet = DevCan.acCanWrite(new canmsg_t[] { sendbuf }, 1U, ref pulNumberofWritten);//CAN DATA SEND
            if (nRet == AdvCANIO.TIME_OUT)
            {
                Console.WriteLine("研华CAN卡发送失败：发送超时!");
                return false;
            }
            else if (nRet == AdvCANIO.OPERATION_ERROR)
            {
                Console.WriteLine("研华CAN卡发送失败：操作失败");
                return false;
            }
            return true;
        }
        protected override System.Boolean DoWriteStandardFrame(Byte DataLen, Boolean RemoteFlag, UInt16 id, Byte[] userdata)
        {
            canmsg_t sendbuf = InitStandeFrameFlags(DataLen, RemoteFlag, id, userdata);
            UInt32 pulNumberofWritten = 0;
            Int32 nRet = DevCan.acCanWrite(new canmsg_t[] { sendbuf }, 1U, ref pulNumberofWritten);//CAN DATA SEND
            if (nRet == AdvCANIO.TIME_OUT)
            {
                Console.WriteLine("研华CAN卡发送失败：发送超时!");
                return false;
            }
            else if (nRet == AdvCANIO.OPERATION_ERROR)
            {
                Console.WriteLine("研华CAN卡发送失败：操作失败");
                return false;
            }
            return true;
        }
        private static canmsg_t InitStandeFrameFlags(Byte DataLen, Boolean RemoteFlag, UInt32 id, Byte[] userdata)
        {
            canmsg_t sendbuf = new canmsg_t();
            sendbuf.length = DataLen;
            if (RemoteFlag)
            {
                sendbuf.flags += AdvCan.MSG_RTR;
                sendbuf.length = 0;
            }
            sendbuf.id = id;
            sendbuf.data = userdata;
            return sendbuf;
        }
        private static canmsg_t InitExtendeFrameFlags(Byte DataLen, Boolean RemoteFlag, UInt32 id, Byte[] userdata)
        {
            canmsg_t sendbuf = new canmsg_t();
            sendbuf.length = DataLen;
            sendbuf.flags += AdvCan.MSG_EXT;
            if (RemoteFlag)
            {
                sendbuf.flags += AdvCan.MSG_RTR;
                sendbuf.length = 0;
            }
            sendbuf.id = id;
            sendbuf.data = userdata;
            return sendbuf;
        }
    }
}
