﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_CRC_Sum8_oneof
    {
        [TestMethod]
        public void _CRC_sum8_oneof_one()
        {
            var tester_c = new test_crc_sum8_oneof.test_crc_sum8_oneof();

            tester_c.InitialParameter();

            tester_c.InitialChannelCHC(null);

            Assert.IsTrue(tester_c.CHC.Open());


            tester_c.A_Recv();

            Assert.IsTrue(tester_c.one.SegOne.Value == 0x06);

        }
    }
}
