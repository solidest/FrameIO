using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_tcp_client_repeate_connectServer
    {
        [TestMethod]
        public void TCP_Client_Repeate_Close_ByAssistant()
        {
            var tester = new test_tcp_client_repeate_connectServer.testtcpclientrepeateconnectServer();

            tester.InitialParameter();

            tester.InitialChannelCHS(null);

            Assert.IsTrue(tester.CHS.Open());

            while(true)
            {
                System.Threading.Thread.Sleep(100);

                tester.A_Recv();

                Assert.IsTrue(tester.head.Value==1);
            }

        }
    }
}
