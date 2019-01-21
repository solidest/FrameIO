using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_udp_nolocalip
    {
        //test_tcp
        [TestMethod]
        public void UDP_No_Localip()
        {
            var tester = new test_udp_no_localip.testudpno_localip();
            var tester1 = new test_udp_no_localip.testudpno_localip();
            tester.InitialParameter();
            tester1.InitialParameter();

            tester.InitialChannelCH_UDP_SEND(null);
            tester1.InitialChannelCH_UDP_RECV(null);

            Assert.IsTrue(tester.CH_UDP_SEND.Open());
            Assert.IsTrue(tester1.CH_UDP_RECV.Open());

            tester.head.Value = 0x55;
            tester.len.Value = 1;
            tester.end.Value = 0xaa;

            tester.A_Send();

            tester1.A_Recv();

            Assert.IsTrue(tester1.head.Value == 0x55);
            Assert.IsTrue(tester1.len.Value == 1);
            Assert.IsTrue(tester1.end.Value == 0xaa);


        }
    }
}
