﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void TCP_nolocalip()
        {
            var tester = new test_tcp_nolocalip .test_tcp();
            tester.InitialParameter();
            tester.InitialChannelCHS(null);

            Assert.IsTrue(tester.CHS.Open());


            tester.A_Recv();

            Assert.IsTrue(tester.head.Value == 0x55);
            Assert.IsTrue(tester.len.Value == 1);
            Assert.IsTrue(tester.end.Value == 0xaa);


        }
    }
}
