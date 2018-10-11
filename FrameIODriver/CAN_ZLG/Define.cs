using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FrameIO.Driver
{
    public enum CanFuncReturn
    {
        ERR_CAN_OVERFLOW = 0x00000001,//CAN 控制器内部 FIFO溢出
        ERR_CAN_ERRALARM = 0x00000002,//CAN 控制器错误报警
        ERR_CAN_PASSIVE = 0x00000004,//CAN 控制器消极错误
        ERR_CAN_LOSE = 0x00000008,//CAN 控制器仲裁丢失
        ERR_CAN_BUSERR = 0x00000010,//CAN 控制器总线错误
        ERR_CAN_BUSOFF = 0x00000020,//CAN 控制器总线关闭
        ERR_CAN_BUFFER_OVERFLOW = 0x00000040,//CAN 控制器内部 Buffer
        ERR_DEVICEOPENED = 0x00000100,//设备已经打开
        ERR_DEVICEOPEN = 0x00000200,//打开设备错误
        ERR_DEVICENOTOPEN = 0x00000400,//设备没有打开
        ERR_BUFFEROVERFLOW = 0x00000800,//缓冲区溢出
        ERR_DEVICENOTEXIST = 0x00001000,//此设备不存在
        ERR_LOADKERNELDLL = 0x00002000,//装载动态库失败
        ERR_CMDFAILED = 0x00004000,//执行命令失败错误码
        ERR_BUFFERCREATE = 0x00008000,//内存不足
        ERR_CANETE_PORTOPENED = 0x00010000,//端口已经被打开
        ERR_CANETE_INDEXUSED = 0x00020000,//设备索引号已经被占用
        ERR_REF_TYPE_ID = 0x00030001,//SetReference 或GetReference 是传递的RefType 是不存在
        ERR_CREATE_SOCKET = 0x00030002,//创建 Socket 时失败
        ERR_OPEN_CONNECT = 0x00030003,//打开 socket 的连接时失败, 可能设备连接已经存在
        ERR_NO_STARTUP = 0x00030004,//设备没启动
        ERR_NO_CONNECTED = 0x00030005,//设备无连接
        ERR_SEND_PARTIAL = 0x00030006,//只发送了部分的 CAN帧
        ERR_SEND_TOO_FAST = 0x00030007,//数据发得太快，Socket缓冲区满了
    }
    //1.ZLGCAN系列接口卡信息的数据类型。
    public struct VCI_BOARD_INFO
    {
        public UInt16 hw_Version;
        public UInt16 fw_Version;
        public UInt16 dr_Version;
        public UInt16 in_Version;
        public UInt16 irq_Num;
        public byte can_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)] public byte[] str_Serial_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] str_hw_Type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Reserved;
    }

    /////////////////////////////////////////////////////
    //2.定义CAN信息帧的数据类型。
    unsafe public struct VCI_CAN_OBJ  //使用不安全代码
    {
        public uint ID;
        public uint TimeStamp;
        public byte TimeFlag;
        public byte SendType;
        public byte RemoteFlag;//是否是远程帧
        public byte ExternFlag;//是否是扩展帧
        public byte DataLen;

        public fixed byte Data[8];

        public fixed byte Reserved[3];

    }

    //3.定义CAN控制器状态的数据类型。
    public struct VCI_CAN_STATUS
    {
        public byte ErrInterrupt;
        public byte regMode;
        public byte regStatus;
        public byte regALCapture;
        public byte regECCapture;
        public byte regEWLimit;
        public byte regRECounter;
        public byte regTECounter;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved;
    }

    //4.定义错误信息的数据类型。
    public struct VCI_ERR_INFO
    {
        public UInt32 ErrCode;
        public byte Passive_ErrData1;
        public byte Passive_ErrData2;
        public byte Passive_ErrData3;
        public byte ArLost_ErrData;
    }

    //5.定义初始化CAN的数据类型
    public struct VCI_INIT_CONFIG
    {
        public UInt32 AccCode;
        public UInt32 AccMask;
        public UInt32 Reserved;
        public byte Filter;
        public byte Timing0;
        public byte Timing1;
        public byte Mode;
    }

    public struct CHGDESIPANDPORT
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] szpwd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] szdesip;
        public Int32 desport;

        public void Init()
        {
            szpwd = new byte[10];
            szdesip = new byte[20];
        }
    }
}
