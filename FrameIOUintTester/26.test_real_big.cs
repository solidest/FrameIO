using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_real_big
    {
        //test_tcp
        [TestMethod]
        public void TestIntegerBig()
        {
            //输入数据：01 02 41 48 F5 C2 (41 48 F5 C2 ==12.56)
            var tester1 = new test_real_big.testrealbig();

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
