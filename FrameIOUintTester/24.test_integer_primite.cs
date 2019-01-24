using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_integer_primite
    {
        //test_tcp
        [TestMethod]
        public void TestIntegerPrimite()
        {
            //输入数据：0x01 0x02 0x55 0xaa
            var tester1 = new test_integer_primitive.testIntegerprimitive();

            tester1.InitialParameter();

            tester1.InitialChannelCH_COM4(null);

            Assert.IsTrue(tester1.CH_COM4.Open());

            tester1.A_Recv();

            Assert.IsTrue(tester1.head.Value == 0x01);
            Assert.IsTrue(tester1.len.Value == 0x02);
            Assert.IsTrue(tester1.end.Value == 0x62FF);

        }

    }
}
