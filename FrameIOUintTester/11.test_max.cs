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
            tester.InitialParameter();

            tester.InitialChannelCH_SEND(null);
            tester.InitialChannelCH_RECV(null);

            Assert.IsTrue(tester.CH_SEND.Open());
            Assert.IsTrue(tester.CH_RECV.Open());

            tester.A.Value = 11;

            tester.A_SEND();

            tester.A_RECV();

            Assert.IsTrue(tester.A.Value == 11);
        }
    }
}
