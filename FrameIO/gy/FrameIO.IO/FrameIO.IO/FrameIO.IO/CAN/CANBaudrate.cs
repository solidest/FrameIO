using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameIO.Driver
{
    public class CANBaudrate
    {
        public UInt32? BusClock { get; set; }
        public Byte? BTR0 { get; set; }
        public Byte? BTR1 { get; set; }
        public UInt32 Baudrate { get; set; }

        private IEnumerable<CANBaudrate> lists = new List<CANBaudrate>();
        public CANBaudrate()
        {
            
        }
        public CANBaudrate(UInt32 baudrate)
        {
            lists = DoGetKnownBaudrates();
            CANBaudrate canBaudrate=GetParams(baudrate);

            this.Baudrate = canBaudrate.Baudrate;
            this.BTR0 = canBaudrate.BTR0;
            this.BTR1 = canBaudrate.BTR1;
            this.BusClock = canBaudrate.BusClock;
        }
        protected  IEnumerable<CANBaudrate> DoGetKnownBaudrates()
        {
            yield return new CANBaudrate() { Baudrate = 10_000, BTR0 = 0X31, BTR1 = 0X1C, BusClock = 16_000_000 };
            yield return new CANBaudrate() { Baudrate = 20_000, BTR0 = 0X18, BTR1 = 0X1C, BusClock = 16_000_000 };
            yield return new CANBaudrate() { Baudrate = 40_000, BTR0 = 0X87, BTR1 = 0XFF, BusClock = 16_000_000 };
            yield return new CANBaudrate() { Baudrate = 50_000, BTR0 = 0X09, BTR1 = 0X1C, BusClock = 16_000_000 };
            yield return new CANBaudrate() { Baudrate = 80_000, BTR0 = 0X83, BTR1 = 0XFF, BusClock = 12_000_000 };
            yield return new CANBaudrate() { Baudrate = 100_000, BTR0 = 0X04, BTR1 = 0X1C, BusClock = 16_000_000 };
            yield return new CANBaudrate() { Baudrate = 125_000, BTR0 = 0X03, BTR1 = 0X1C, BusClock = 16_000_000 };
            yield return new CANBaudrate() { Baudrate = 200_000, BTR0 = 0X81, BTR1 = 0XFA, BusClock = 16_000_000 };
            yield return new CANBaudrate() { Baudrate = 250_000, BTR0 = 0X01, BTR1 = 0X1C, BusClock = 16_000_000 };
            yield return new CANBaudrate() { Baudrate = 400_000, BTR0 = 0X80, BTR1 = 0XFA, BusClock = 16_000_000 };
            yield return new CANBaudrate() { Baudrate = 500_000, BTR0 = 0X00, BTR1 = 0X1C, BusClock = 16_000_000 };
            yield return new CANBaudrate() { Baudrate = 666_000, BTR0 = 0X80, BTR1 = 0XB6, BusClock = 16_000_000 };
            yield return new CANBaudrate() { Baudrate = 800_000, BTR0 = 0X00, BTR1 = 0X16, BusClock = 16_000_000 };
            yield return new CANBaudrate() { Baudrate = 1_000_000, BTR0 = 0X00, BTR1 = 0X14, BusClock = 16_000_000 };
        }
        private CANBaudrate GetParams(UInt32 baudrate)
        {
            foreach(var v in lists)
            {
                if (v.Baudrate == baudrate)
                    return v;
            }
            return null;
        }
    }
}
