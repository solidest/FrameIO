using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{

    internal enum SegmentTypeEnum
    {
        SegInteger,
        SegIntegerArray,
        SegReal,
        SegRealArray,
        SegGroup,
        SegGroupArray,
        SegOneOfGroup,
        SegOneOfGroupArray,
        SegOneOfItem
    }

    public enum ChannelTypeEnum
    {
        COM = 1,
        CAN,
        TCPSERVER,
        TCPCLIENT,
        UDP,
        DIO
    }


    internal enum ByteOrderTypeEnum
    {
        Small,
        Big
    }

    internal enum EncodedTypeEnum
    {
        Primitive,
        Inversion,
        Complement
    }

    internal enum CheckTypeEnum
    {
        SEGPV_SUM8,
        SEGPV_XOR8,
        SEGPV_SUM16,
        SEGPV_SUM16_FALSE,
        SEGPV_XOR16,
        SEGPV_XOR16_FALSE,
        SEGPV_SUM32,
        SEGPV_SUM32_FALSE,
        SEGPV_XOR32,
        SEGPV_XOR32_FALSE,
        SEGPV_CRC4_ITU,
        SEGPV_CRC5_EPC,
        SEGPV_CRC5_ITU,
        SEGPV_CRC5_USB,
        SEGPV_CRC6_ITU,
        SEGPV_CRC7_MMC,
        SEGPV_CRC8,
        SEGPV_CRC8_ITU,
        SEGPV_CRC8_ROHC,
        SEGPV_CRC8_MAXIM,
        SEGPV_CRC16_IBM,
        SEGPV_CRC16_MAXIM,
        SEGPV_CRC16_USB,
        SEGPV_CRC16_MODBUS,
        SEGPV_CRC16_CCITT,
        SEGPV_CRC16_CCITT_FALSE,
        SEGPV_CRC16_X25,
        SEGPV_CRC16_XMODEM,
        SEGPV_CRC16_DNP,
        SEGPV_CRC32,
        SEGPV_CRC32_MPEG_2,
        SEGPV_CRC64,
        SEGPV_CRC64_WE
    }
}
