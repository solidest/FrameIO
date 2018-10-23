using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameIO.Driver
{
    public static class BaudRateTypeConverter 
    {
        public static Object ConvertFrom(Object value)
        {
            if (!(value is System.String)) return value;
            if (Object.Equals("10Kbps", value)) return Convert.ToUInt16(10);
            if (Object.Equals("20Kbps", value)) return Convert.ToUInt16(20);
            if (Object.Equals("50Kbps", value)) return Convert.ToUInt16(50);
            if (Object.Equals("100Kbps", value)) return Convert.ToUInt16(100);
            if (Object.Equals("125Kbps", value)) return Convert.ToUInt16(125);
            if (Object.Equals("250Kbps", value)) return Convert.ToUInt16(250);;
            if (Object.Equals("500Kbps", value)) return Convert.ToUInt16(500);
            if (Object.Equals("800Kbps", value)) return Convert.ToUInt16(800);
            if (Object.Equals("1000Kbps", value)) return Convert.ToUInt16(1000);
            return Convert.ToUInt16(0);
        }
    }
}
