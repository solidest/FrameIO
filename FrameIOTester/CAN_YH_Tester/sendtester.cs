using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOTester.CAN_YH_Tester
{
    class sendtester
    {
        [TestMethod]
        public void UDP_Send_Receive()
        {
            var sys = new main.SYS1();

            sys.InitialChannelCH1(null);
            sys.CH1.Open();
            sys.data0.Value = 12;
            sys.frametype.Value = false;
            sys.acsend();

            sys.acrecv();

            var sys2 = new main.sys2();
            sys2.acrecv();
           
        }
    }
}
