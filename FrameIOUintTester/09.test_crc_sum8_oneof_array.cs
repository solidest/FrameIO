﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_CRC_Sum8_oneof_array
    {
        [TestMethod]
        public void _CRC_sum8_oneof_one()
        {
            //send data 01 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 02 00 00 00 02 00 00 00 02 00 00 00 0a 00 00 00
            var tester_c = new test_crc_sum8_oneof_array.test_crc_sum8_oneof_array();

            tester_c.InitialParameter();

            tester_c.InitialChannelCHC(null);

            Assert.IsTrue(tester_c.CHC.Open());


            tester_c.A_Recv();

            Assert.IsTrue(tester_c.one.SegOne.Value == 0x0D);

        }
    }
}
