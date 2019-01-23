using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_tcp_server_client
    {
        //test_tcp
        [TestMethod]
        public void TCP()
        {
            var tester = new test_tcp_server_client.test_tcp_server_client();
            var tester1 = new test_tcp_server_client.test_tcp_server_client();

            tester.InitialParameter();
            tester1.InitialParameter();

            tester.InitialChannelCHS(null);
            tester1.InitialChannelCHC(null);

            Assert.IsTrue(tester.CHS.Open());
            Assert.IsTrue(tester1.CHC.Open());

            tester1.CHC.Close();
            tester1.InitialChannelCHC(null);
            Assert.IsTrue(tester1.CHC.Open());

            tester.head.Value = 0x55;
            tester.len.Value = 1;
            tester.end.Value = 0xaa;

            tester.A_Send();

            System.Threading.Thread.Sleep(5000);
            tester.head.Value = 0x55;
            tester.len.Value = 1;
            tester.end.Value = 0xaa;

            tester1.A_Recv();

            Assert.IsTrue(tester1.head.Value == 0x55);
            Assert.IsTrue(tester1.len.Value == 1);
            Assert.IsTrue(tester1.end.Value == 0xaa);


        }
    }
}
