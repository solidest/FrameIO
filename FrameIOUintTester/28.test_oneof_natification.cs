using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_oneof_natification
    {
        //test_tcp
        [TestMethod]
        public void TestRealSmall()
        {
            //输入数据：01 02 C2 F5 48 41 ( C2 F5 48 41==12.56)
            var tester = new test_oneof_natification.testoneofnatification();
            var tester1 = new test_oneof_natification.testoneofnatification();

            tester.InitialParameter();
            tester1.InitialParameter();

            tester.InitialChannelCH_UDP_SEND(null);
            tester1.InitialChannelCH_UDP_RECV(null);

            Assert.IsTrue(tester.CH_UDP_SEND.Open());
            Assert.IsTrue(tester1.CH_UDP_RECV.Open());

            //tester1.A_Recv();

            //Assert.IsTrue(tester1.head.Value == 0x01);
            //Assert.IsTrue(tester1.len.Value == 0x02);
            //Assert.IsTrue(Math.Round((double)tester1.end.Value,2) == 12.56);

        }

    }
}
