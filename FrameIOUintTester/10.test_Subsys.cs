using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_SubSys
    {
        //test_tcp
        [TestMethod]
        public void Test_SubSystem()
        {
            var tester = new test_subsys.testsubsys();
            tester.InitialParameter();

            tester.InitialChannelCH_UDP_SEND(null);
            tester.InitialChannelCH_UDP_RECV(null);

            Assert.IsTrue(tester.CH_UDP_SEND.Open());
            Assert.IsTrue(tester.CH_UDP_RECV.Open());

            tester.head.Value = 1;
            tester.len.Value = 2;
            tester.end.Value = 3;

            tester.pos.jingdu.Value = 89.87f;
            tester.pos.weidu.Value = 92.53f;

            tester.A_Send();

            tester.A_Recv();

            Assert.IsTrue(tester.head.Value == 1);
            Assert.IsTrue(tester.len.Value == 2);
            Assert.IsTrue(tester.end.Value == 3);
            Assert.IsTrue(tester.pos.jingdu.Value == 89.87f);
            Assert.IsTrue(tester.pos.weidu.Value == 92.53f);
        }
    }
}
