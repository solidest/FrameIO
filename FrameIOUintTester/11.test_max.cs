using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_max_min
    {
        //test_tcp
        [TestMethod]
        public void Test_Max_Min()
        {
            var tester = new test_max.subsys1();
            var tester1 = new test_max.subsys1();
            tester.InitialParameter();
            tester1.InitialParameter();

            tester.InitialChannelCH_SEND(null);
            tester1.InitialChannelCH_RECV(null);

            Assert.IsTrue(tester.CH_SEND.Open());
            Assert.IsTrue(tester1.CH_RECV.Open());

            tester.A.Value = 8;

            tester.A_SEND();

            tester1.A_RECV();

            Assert.IsTrue(tester.A.Value == 8);
        }
    }
}
