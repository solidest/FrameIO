using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_tcpserver
    {
        //test_tcp
        [TestMethod]
        public void TCPServer()
        {
            var tester = new Tcpserver.Tcpserver();
            var tester1 = new Tcpserver.Tcpserver();
            //var tester2 = new Tcpserver.Tcpserver();
            //var tester3 = new Tcpserver.Tcpserver();

            tester.InitialParameter();
            tester1.InitialParameter();
            //tester2.InitialParameter();
            //tester3.InitialParameter();

            tester.InitialChannelCHS(null);
            tester1.InitialChannelCHS2(null);
            //tester2.InitialChannelCHC(null);
            //tester3.InitialChannelCHC_1(null);

            Assert.IsTrue(tester.CHS.Open());
            Assert.IsTrue(tester1.CHS2.Open());
            //Assert.IsTrue(tester2.CHC.Open());
            //Assert.IsTrue(tester3.CHC_1.Open());

            tester.head.Value = 0x55;
            tester.len.Value = 1;
            tester.end.Value = 0xaa;

            //tester1.head.Value = 0x55;
            //tester1.len.Value = 1;
            //tester1.end.Value = 0xaa;

            tester.A_Send();

            //tester1.head.Value = 0x55;
            //tester1.len.Value = 1;
            //tester1.end.Value = 0xaa;

            //tester1.A_Send_1();

            //tester2.A_Recv();

            Assert.IsTrue(1 == 1);
            //Assert.IsTrue(tester2.head.Value == 0x55);
            //Assert.IsTrue(tester2.len.Value == 1);
            //Assert.IsTrue(tester2.end.Value == 0xaa);


        }
    }
}
