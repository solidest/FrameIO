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
        public void _CRC_No_Range()
        {
            var tester_s = new test_crc_sum16.test_crc_sum16();
            var tester_c = new test_crc_sum16.test_crc_sum16();

            tester_s.InitialParameter();
            tester_c.InitialParameter();

            tester_s.InitialChannelCHS(null);
            tester_c.InitialChannelCHC(null);

            Assert.IsTrue(tester_s.CHS.Open());
            Assert.IsTrue(tester_c.CHC.Open());

            tester_s.head.Value = 0x0102;
            tester_s.len.Value = 0x0302;
            tester_s.A.Value = 0x0101;
            tester_s.B.Value = 0x0202;


            tester_s.A_Send();

            tester_c.A_Recv();

            Assert.IsTrue(tester_c.head.Value == 0x0102);
            Assert.IsTrue(tester_c.len.Value == 0x0302);
            Assert.IsTrue(tester_c.end.Value == 0x0707);

        }

    }
}
