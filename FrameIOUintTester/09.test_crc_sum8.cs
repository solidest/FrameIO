using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_CRC_Sum8
    {
        [TestMethod]
        public void _CRC_sum8_No_Range()
        {
            //send Data：01 00 00 00 02 00 00 00 03 00 00 00 05 00 00 00 0B 00 00 00
            //var tester_s = new test_crc_sum8.test_crc_sum8();
            var tester_c = new test_crc_sum8.test_crc_sum8();

            //tester_s.InitialParameter();
            tester_c.InitialParameter();

            //tester_s.InitialChannelCHS(null);
            tester_c.InitialChannelCHC(null);

            //Assert.IsTrue(tester_s.CHS.Open());
            Assert.IsTrue(tester_c.CHC.Open());

            //tester_s.A.Value = 0x03;
            //tester_s.head.Value = 0x01;
            //tester_s.len.Value = 0x02;

            //tester_s.B.Value = 0x04;

            //tester_s.A_Send();
            tester_c.A_Recv();

            Assert.IsTrue(tester_c.head.Value == 0x02);
            Assert.IsTrue(tester_c.len.Value == 0x03);
            Assert.IsTrue(tester_c.end.Value == 0x0B);

        }

    }
}
