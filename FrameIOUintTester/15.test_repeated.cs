using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_Repeated
    {
        //test_tcp
        [TestMethod]
        public void TestRepeated()
        {
            var tester1 = new test_repeated.testRepeated();
            var tester2 = new test_repeated.testRepeated();

            tester1.InitialParameter();
            tester2.InitialParameter();

            tester1.InitialChannelCHS(null);
            tester2.InitialChannelCHC(null);

            Assert.IsTrue(tester1.CHS.Open());
            Assert.IsTrue(tester2.CHC.Open());

            tester1.end[0].Value=10;
            tester1.end[1].Value = 20;

            tester1.head.Value = 0x50;

            tester1.len.Value = 2;

            tester1.A_Send();

            tester2.A_Recv();

            Assert.IsTrue(tester2.head.Value == 0x50);
            Assert.IsTrue(tester2.len.Value == 2);
            Assert.IsTrue(tester2.end[0].Value == 10);
            Assert.IsTrue(tester2.end[1].Value == 20);
        }

    }
}
