using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_tcp_nolocalip
    {
        //test_tcp
        [TestMethod]
        public void TCP()
        {
            var tester = new test_tcp_nolocalip .test_tcp();
            tester.InitialParameter();
            tester.InitialChannelCHS(null);
            tester.InitialChannelCHC(null);

            Assert.IsTrue(tester.CHS.Open());
            Assert.IsTrue(tester.CHC.Open());

            tester.head.Value = 0x55;
            tester.len.Value = 1;
            tester.end.Value = 0xaa;

            tester.A_Send();

            tester.A_Recv();

            Assert.IsTrue(tester.head.Value == 0x55);
            Assert.IsTrue(tester.len.Value == 1);
            Assert.IsTrue(tester.end.Value == 0xaa);


        }
    }
}
