using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
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




        public static ulong GetCheckValue(ushort checktype, byte[] data, int startpos, int endpos)
            {
                //byte[] data = new byte[] { 0x01, 0x02, 0x03, 0x04 };
                var len = endpos - startpos;
                var ptr = Marshal.AllocHGlobal(len);
                for (var pos = startpos; pos < endpos; ++pos)
                    Marshal.WriteByte(ptr, pos-startpos, data[pos]);

            //SEGPV_SUM8 = 200,
            //SEGPV_XOR8,
            //SEGPV_SUM16,
            //SEGPV_SUM16_FALSE,
            //SEGPV_XOR16,
            //SEGPV_XOR16_FALSE,
            //SEGPV_SUM32,
            //SEGPV_SUM32_FALSE,
            //SEGPV_XOR32,
            //SEGPV_XOR32_FALSE,
            //SEGPV_CRC4_ITU,
            //SEGPV_CRC5_EPC,
            //SEGPV_CRC5_ITU,
            //SEGPV_CRC5_USB,
            //SEGPV_CRC6_ITU,
            //SEGPV_CRC7_MMC,
            //SEGPV_CRC8,
            //SEGPV_CRC8_ITU,
            //SEGPV_CRC8_ROHC,
            //SEGPV_CRC8_MAXIM,
            //SEGPV_CRC16_IBM,
            //SEGPV_CRC16_MAXIM,
            //SEGPV_CRC16_USB,
            //SEGPV_CRC16_MODBUS,
            //SEGPV_CRC16_CCITT,
            //SEGPV_CRC16_CCITT_FALSE,
            //SEGPV_CRC16_X25,
            //SEGPV_CRC16_XMODEM,
            //SEGPV_CRC16_DNP,
            //SEGPV_CRC32,
            //SEGPV_CRC32_MPEG_2,
            //SEGPV_CRC64,
            //SEGPV_CRC64_WE

                switch (checktype)
                {
                case 1:
                    return sum8(ptr, len);
                case 2:
                    return xor8(ptr, len);
                case 3:
                    return sum16(ptr, len);
                case 4:
                    return sum16_false(ptr, len);
                case 5:
                    return xor16(ptr, len);
                case 6:
                    return xor16_false(ptr, len);
                case 7:
                    return sum32(ptr, len);
                case 8:
                    return sum32_false(ptr, len);
                case 9:
                    return xor32(ptr, len);
                case 10:
                    return xor32_false(ptr, len);
                case 11:
                    return crc4_itu(ptr, len);
                case 12:
                    return crc5_epc(ptr, len);
                case 13:
                    return crc5_itu(ptr, len);
                case 14:
                    return crc5_usb(ptr, len);
                case 15:
                    return crc6_itu(ptr, len);
                case 16:
                    return crc7_mmc(ptr, len);
                case 17:
                    return crc8(ptr, len);
                case 18:
                    return crc8_itu(ptr, len);
                case 19:
                    return crc8_rohc(ptr, len);
                case 20:
                    return crc8_maxim(ptr, len);
                case 21:
                    return crc16_ibm(ptr, len);
                case 22:
                    return crc16_maxim(ptr, len);
                case 23:
                    return crc16_usb(ptr, len);
                case 24:
                    return crc16_modbus(ptr, len);
                case 25:
                    return crc16_ccitt(ptr, len);
                case 26:
                    return crc16_ccitt_false(ptr, len);
                case 27:
                    return crc16_x25(ptr, len);
                case 28:
                    return crc16_xmodem(ptr, len);
                case 29:
                    return crc16_dnp(ptr, len);
                case 30:
                    return crc32(ptr, len);
                case 31:
                    return crc32_mpeg_2(ptr, len);
                case 32:
                    return crc64(ptr, len);
                case 33:
                    return crc64_we(ptr, len);

                default:
                    return 0;
                }
            }
    }
}
