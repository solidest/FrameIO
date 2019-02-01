using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_real_small
    {
        //test_tcp
        [TestMethod]
        public void TestRealSmall()
        {
            //输入数据：01 02 C2 F5 48 41 ( C2 F5 48 41==12.56)
            var tester1 = new test_real_small.testrealsmall();

            tester1.InitialParameter();

            tester1.InitialChannelCH_COM4(null);

            Assert.IsTrue(tester1.CH_COM4.Open());

            tester1.A_Recv();

            Assert.IsTrue(tester1.head.Value == 0x01);
            Assert.IsTrue(tester1.len.Value == 0x02);
            Assert.IsTrue(Math.Round((double)tester1.end.Value,2) == 12.56);

        }

    }
}
