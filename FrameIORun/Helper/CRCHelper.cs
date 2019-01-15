using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal class CRCHelper
    {

            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint sum8(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint xor8(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint sum16(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint sum16_false(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint xor16(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint xor16_false(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint sum32(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint sum32_false(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint xor32(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint xor32_false(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc4_itu(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc5_epc(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc5_itu(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc5_usb(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc6_itu(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc7_mmc(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc8(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc8_itu(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc8_rohc(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc8_maxim(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc16_ibm(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc16_maxim(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc16_usb(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc16_modbus(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc16_ccitt(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc16_ccitt_false(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc16_x25(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc16_xmodem(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc16_dnp(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc32(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint crc32_mpeg_2(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern ulong crc64(IntPtr data, int data_len);
            [DllImport("CRC.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
            public static extern ulong crc64_we(IntPtr data, int data_len);




        public static ulong GetCheckValue(CheckTypeEnum checktype, byte[] data, int startpos, int endpos)
            {
                var len = endpos - startpos;
                var ptr = Marshal.AllocHGlobal(len);
                for (var pos = startpos; pos < endpos; ++pos)
                    Marshal.WriteByte(ptr, pos-startpos, data[pos]);


                switch (checktype)
                {
                case CheckTypeEnum.SEGPV_SUM8:
                    return sum8(ptr, len);
                case CheckTypeEnum.SEGPV_XOR8:
                    return xor8(ptr, len);
                case CheckTypeEnum.SEGPV_SUM16:
                    return sum16(ptr, len);
                case CheckTypeEnum.SEGPV_SUM16_FALSE:
                    return sum16_false(ptr, len);
                case CheckTypeEnum.SEGPV_XOR16:
                    return xor16(ptr, len);
                case CheckTypeEnum.SEGPV_XOR16_FALSE:
                    return xor16_false(ptr, len);
                case CheckTypeEnum.SEGPV_SUM32:
                    return sum32(ptr, len);
                case CheckTypeEnum.SEGPV_SUM32_FALSE:
                    return sum32_false(ptr, len);
                case CheckTypeEnum.SEGPV_XOR32:
                    return xor32(ptr, len);
                case CheckTypeEnum.SEGPV_XOR32_FALSE:
                    return xor32_false(ptr, len);
                case CheckTypeEnum.SEGPV_CRC4_ITU:
                    return crc4_itu(ptr, len);
                case CheckTypeEnum.SEGPV_CRC5_EPC:
                    return crc5_epc(ptr, len);
                case CheckTypeEnum.SEGPV_CRC5_ITU:
                    return crc5_itu(ptr, len);
                case CheckTypeEnum.SEGPV_CRC5_USB:
                    return crc5_usb(ptr, len);
                case CheckTypeEnum.SEGPV_CRC6_ITU:
                    return crc6_itu(ptr, len);
                case CheckTypeEnum.SEGPV_CRC7_MMC:
                    return crc7_mmc(ptr, len);
                case CheckTypeEnum.SEGPV_CRC8:
                    return crc8(ptr, len);
                case CheckTypeEnum.SEGPV_CRC8_ITU:
                    return crc8_itu(ptr, len);
                case CheckTypeEnum.SEGPV_CRC8_ROHC:
                    return crc8_rohc(ptr, len);
                case CheckTypeEnum.SEGPV_CRC8_MAXIM:
                    return crc8_maxim(ptr, len);
                case CheckTypeEnum.SEGPV_CRC16_IBM:
                    return crc16_ibm(ptr, len);
                case CheckTypeEnum.SEGPV_CRC16_MAXIM:
                    return crc16_maxim(ptr, len);
                case CheckTypeEnum.SEGPV_CRC16_USB:
                    return crc16_usb(ptr, len);
                case CheckTypeEnum.SEGPV_CRC16_MODBUS:
                    return crc16_modbus(ptr, len);
                case CheckTypeEnum.SEGPV_CRC16_CCITT:
                    return crc16_ccitt(ptr, len);
                case CheckTypeEnum.SEGPV_CRC16_CCITT_FALSE:
                    return crc16_ccitt_false(ptr, len);
                case CheckTypeEnum.SEGPV_CRC16_X25:
                    return crc16_x25(ptr, len);
                case CheckTypeEnum.SEGPV_CRC16_XMODEM:
                    return crc16_xmodem(ptr, len);
                case CheckTypeEnum.SEGPV_CRC16_DNP:
                    return crc16_dnp(ptr, len);
                case CheckTypeEnum.SEGPV_CRC32:
                    return crc32(ptr, len);
                case CheckTypeEnum.SEGPV_CRC32_MPEG_2:
                    return crc32_mpeg_2(ptr, len);
                case CheckTypeEnum.SEGPV_CRC64:
                    return crc64(ptr, len);
                case CheckTypeEnum.SEGPV_CRC64_WE:
                    return crc64_we(ptr, len);

                default:
                    return 0;
                }
            }
    }
}
