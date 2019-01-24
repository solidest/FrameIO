using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_CRC
    {
        //test_tcp
        [TestMethod]
        public void _CRC()
        {
            var tester_s = new test_crc.test_crc();
            var tester_c = new test_crc.test_crc();

            tester_s.InitialParameter();
            tester_c.InitialParameter();

            tester_s.InitialChannelCHS(null);
            tester_c.InitialChannelCHC(null);

            Assert.IsTrue(tester_s.CHS.Open());
            Assert.IsTrue(tester_c.CHC.Open());

            tester_s.head.Value = 0x02;
            tester_s.len.Value = 0x06;

            tester_s.A_Send();

            tester_c.A_Recv();

            Assert.IsTrue(tester_c.head.Value == 0x02);
            Assert.IsTrue(tester_c.len.Value == 0x06);
            Assert.IsTrue(tester_c.end.Value == 0x08);

        }
    }
}
