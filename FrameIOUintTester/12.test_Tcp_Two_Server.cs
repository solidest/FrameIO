using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_tcp_two_server
    {
        //test_tcp
        [TestMethod]
        public void TCP_Two_Server()
        {
            var tester_S = new Tcpserver.Tcpserver();
            var tester_S_2 = new Tcpserver.Tcpserver();
            var tester_C = new Tcpserver.Tcpserver();
            var tester_C_2 = new Tcpserver.Tcpserver();

            tester_S.InitialParameter();
            tester_S_2.InitialParameter();
            tester_C.InitialParameter();
            tester_C_2.InitialParameter();

            tester_S.InitialChannelCHS(null);
            tester_S_2.InitialChannelCHS_2(null);
            tester_C.InitialChannelCHC(null);
            tester_C_2.InitialChannelCHC_2(null);

            Assert.IsTrue(tester_S.CHS.Open());
            Assert.IsTrue(tester_S_2.CHS_2.Open());
            Assert.IsTrue(tester_C.CHC.Open());
            Assert.IsTrue(tester_C_2.CHC_2.Open());

            tester_S.head.Value = 0x55;
            tester_S.len.Value = 1;
            tester_S.end.Value = 0xaa;

            tester_S_2.head.Value = 0x55;
            tester_S_2.len.Value = 2;
            tester_S_2.end.Value = 0xaa;

            tester_S.A_Send();

            tester_S_2.A_Send_2();

            tester_C.A_Recv();

            tester_C_2.A_Recv_2();

            Assert.IsTrue(tester_C.head.Value == 0x55);
            Assert.IsTrue(tester_C.len.Value == 1);
            Assert.IsTrue(tester_C.end.Value == 0xaa);

            Assert.IsTrue(tester_C_2.head.Value == 0x55);
            Assert.IsTrue(tester_C_2.len.Value == 2);
            Assert.IsTrue(tester_C_2.end.Value == 0xaa);


        }
    }
}
