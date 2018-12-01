using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    
    public enum ByteOrderType
    {
        Small = 100,
        Big
    }

    public enum EncodedType
    {
        Primitive = 102,
        Inversion,
        Complement
    }

    public enum CheckType
    {
        None = 0,
        SEGPV_SUM8 = 200,
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


    //项目成员类型
    public enum projectitemtype
    {
        PI_SYSTEM = 1,
        PI_FRAME,
        PI_ENUMCFG
    };

    //系统成员类型
    public enum systitemtype
    {
        SYSI_PROPERTY = 1,
        SYSI_CHANNEL,
        SYSI_ACTION
    };

    //系统属性类型
    public enum syspropertytype
    {
        SYSPT_BOOL = 1,
        SYSPT_BYTE,
        SYSPT_SBYTE,
        SYSPT_USHORT,
        SYSPT_SHORT,
        SYSPT_UINT,
        SYSPT_INT,
        SYSPT_ULONG,
        SYSPT_LONG,
        SYSPT_FLOAT,
        SYSPT_DOUBLE
    };

    //通道类型
    public enum syschanneltype
    {
        SCHT_COM = 1,
        SCHT_CAN,
        SCHT_TCPSERVER,
        SCHT_TCPCLIENT,
        SCHT_UDP,
        SCHT_DIO
    };


    //IO操作类型
    public enum actioniotype
    {
        AIO_SEND = 1,
        AIO_RECV
        //AIO_RECVLOOP
    };

    //表达式类型
    public enum exptype
    {
        EXP_INT = 1,
        EXP_REAL,
        EXP_ID,
        EXP_BYTESIZEOF,
        EXP_ADD,
        EXP_SUB,
        EXP_MUL,
        EXP_DIV
    };

    //字段属性
    public enum segpropertytype
    {
        SEGP_SIGNED = 1,
        SEGP_BITCOUNT,
        SEGP_VALUE,
        SEGP_REPEATED,
        SEGP_BYTEORDER,
        SEGP_ENCODED,
        SEGP_ISDOUBLE,
        SEGP_TAIL,
        SEGP_ALIGNEDLEN,
        SEGP_TYPE,
        SEGP_BYTESIZE,
        SEGP_TOENUM,
        SEGP_MAX,
        SEGP_MIN,
        SEGP_CHECK,
        SEGP_CHECKRANGE_BEGIN,
        SEGP_CHECKRANGE_END
    };

    //字段属性值类型
    public enum segpropertyvaluetype
    {
        SEGPV_INT = 1,
        SEGPV_REAL,
        SEGPV_STRING,
        SEGPV_TRUE,
        SEGPV_FALSE,
        SEGPV_ID,

        SEGPV_SMALL = 100,
        SEGPV_BIG,
        SEGPV_PRIMITIVE,
        SEGPV_INVERSION,
        SEGPV_COMPLEMENT,

        SEGPV_SUM8 = 200,
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
        SEGPV_CRC64_WE,

        SEGPV_NONAMEFRAME = 300,
        SEGPV_ONEOF,
        SEGPV_EXP
    };

    //字段类型
    public enum segmenttype
    {
        SEGT_INTEGER = 1,
        SEGT_REAL,
        SEGT_BLOCK,
        SEGT_TEXT
    };
}
