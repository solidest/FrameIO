using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_udp
    {
        //test_tcp
        [TestMethod]
        public void UDP()
        {
            var tester = new test_udp.testudp();
            tester.InitialParameter();

            //tester.InitialChannelCH_UDP_SEND(null);
            tester.InitialChannelCH_UDP_RECV(null);

            //Assert.IsTrue(tester.CH_UDP_SEND.Open());
            Assert.IsTrue(tester.CH_UDP_RECV.Open());

            //tester.head.Value = 0x55;
            //tester.len.Value = 1;
            //tester.end.Value = 0xaa;

            //tester.A_Send();

            tester.A_Recv();

            Assert.IsTrue(tester.head.Value == 0x55);
            Assert.IsTrue(tester.len.Value == 1);
            Assert.IsTrue(tester.end.Value == 0xaa);


        }
    }
}
