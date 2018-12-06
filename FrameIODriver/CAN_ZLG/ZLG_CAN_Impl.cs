using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using FrameIO.Interface;

namespace FrameIO.Driver
{
    public partial class CAN_ZLG_Impl : PhysicalChannel,IFrameStream, IFrameReader, IFrameWriter
    {
        #region IFrameStream
        public void ClearChannel()
        {
            if (m_bOpen == 0)
                Wrapor.VCI_ClearBuffer(m_devtype, m_devind, m_canind);
        }

        public void Close()
        {
            if (m_bOpen == 1)
                Wrapor.VCI_CloseDevice(m_devtype, m_devind);
        }
        public bool Open()
        {
            if (m_bOpen == 1)
                return false;

            VCI_INIT_CONFIG Init_Config = new VCI_INIT_CONFIG();

            Init_Config.AccCode = AccCode;
            Init_Config.AccMask = AccMask;
            Init_Config.Timing0 = Timing0;
            Init_Config.Timing1 = Timing1;
            Init_Config.Filter = Filter;
            Init_Config.Mode = Mode;

            Wrapor.VCI_InitCAN(m_devtype, m_devind, m_canind, ref Init_Config);
            Wrapor.VCI_StartCAN(m_devtype, m_devind, m_canind);

            return true;
        }

        void IFrameStream.InitConfig(Dictionary<string, object> config)
        {
            if (!config.ContainsKey("devtype") || !config.ContainsKey("devInd") 
                || !config.ContainsKey("channelind") || !config.ContainsKey("baudrate")
                || !config.ContainsKey("waittimeout") || !config.ContainsKey("filter")
                || !config.ContainsKey("acccode") || !config.ContainsKey("accmark"))
                throw new FrameIO.Interface.FrameIOException(Interface.FrameIOErrorType.ChannelErr, "初始化广州致远CAN接口", "缺少初始化配置参数!");

            m_devtype = (uint)config["devtype"];
            m_devind = (UInt32)config["devInd"];
            m_canind = (UInt32)config["channelind"];
            m_waittime = (int)config["waittime"];

            AccCode = System.Convert.ToUInt32("0x" + config["accCode"], 16);
            AccMask = System.Convert.ToUInt32("0x" + config["accMask"], 16);

            Baudrate = System.Convert.ToUInt32(config["baudrate"]);
            CANBaudrate canBaudrate = new CANBaudrate(Baudrate);
            Timing0 = System.Convert.ToByte("" + canBaudrate.BTR0, 16);
            Timing1 = System.Convert.ToByte("" + canBaudrate.BTR1, 16);

            Filter = (Byte)config["filter"];
            Mode = (Byte)config["mode"];
            if (config.ContainsKey("waittimeout"))
                ReceiveTimeOut = Convert.ToInt32(config["waittimeout"]);
        }

        #endregion

        #region IFrameReader
        public ISegmentGettor ReadFrame(IFrameUnpack up)
        {
            //TODO 
            UInt32 res = new UInt32();
            res = Wrapor.VCI_GetReceiveNum(m_devtype, m_devind, m_canind);
            Stopwatch watcher = new Stopwatch();
            watcher.Start();
            while (res == 0)
            {
                if(watcher.ElapsedMilliseconds> ReceiveTimeOut)
                {
                    watcher.Stop();
                    throw new FrameIO.Interface.FrameIOException(FrameIOErrorType.SendErr, "广州致远CAN接口", "接收数据超时!");
                }
                System.Threading.Thread.Sleep(1);
                res = Wrapor.VCI_GetReceiveNum(m_devtype, m_devind, m_canind);
            }
            UInt32 con_maxlen = 1;
            IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * (Int32)con_maxlen);
            res = Wrapor.VCI_Receive(m_devtype, m_devind, m_canind, pt, con_maxlen, m_waittime);

            Debug.Assert(res > 0);
            VCI_CAN_OBJ obj = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((UInt32)pt + 1 * Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ));
            up.AppendBlock(CAN_RX_MSG_ToBytes(obj));
            return up.Unpack();
        }

        public ISegmentGettor[] ReadFrameList(IFrameUnpack up, int framecount)
        {
            var ret = new ISegmentGettor[framecount];

            UInt32 res = new UInt32();
            res = Wrapor.VCI_GetReceiveNum(m_devtype, m_devind, m_canind);
            while (res <= framecount)
            {
                System.Threading.Thread.Sleep(1);
                res = Wrapor.VCI_GetReceiveNum(m_devtype, m_devind, m_canind);
            }
            UInt32 con_maxlen = (UInt32)framecount;
            IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)) * (Int32)con_maxlen);
            res = Wrapor.VCI_Receive(m_devtype, m_devind, m_canind, pt, con_maxlen, m_waittime);

            for (UInt32 i = 0; i < res; i++)
            {
                VCI_CAN_OBJ obj = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((UInt32)pt + i * Marshal.SizeOf(typeof(VCI_CAN_OBJ))), typeof(VCI_CAN_OBJ));
                up.AppendBlock(CAN_RX_MSG_ToBytes(obj));
                ret[i] = up.Unpack();
            }

            return ret;
        }
        public void BeginReadFrame(IFrameUnpack up, AsyncReadCallback callback, object AsyncState)
        {
            BeginReadFrameImpl( up,  callback,  AsyncState);

        }

        public void BeginReadFrameList(IFrameUnpack up, int framecount, bool isloop, AsyncReadListCallback callback, object AsyncState)
        {
            BeginReadFrameListImpl(up, framecount, isloop, callback, AsyncState);

        }
        #endregion

        #region IFrameWriter
        unsafe public int WriteFrame(IFramePack p)
        {
            byte[] buff = p.Pack();
            if (DoWrite(buff))
                return 1;
            return 0;
        }

        public int WriteFrameList(IFramePack[] p, int len)
        {
            UInt32 ret = 0;

            VCI_CAN_OBJ[] sendFrames = new VCI_CAN_OBJ[len];
            for (int i=0;i<len;i++)
            {
                byte[] sendFrame = p[i].Pack();
                sendFrames[i] = ParseFrame(sendFrame);
            }

            ret = Api.VCI_Transmit(m_devtype, m_devind, m_canind, ref sendFrames[0], (uint)len);
            if (ret == 0)
            {
                throw new Exception("设备不存在或掉线");
            }

            return (int)ret;
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
