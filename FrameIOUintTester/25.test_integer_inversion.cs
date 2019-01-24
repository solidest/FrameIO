using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_integer_Inversion
    {
        //test_tcp
        [TestMethod]
        public void TestIntegerInversion()
        {
            //输入数据：0x01 0x02 0x9d 0x80
            //flo文件中 signed=true 默认是false encoded是发送接收前要执行的动作
            var tester1 = new test_integer_inversion.testIntegerinversion();

            tester1.InitialParameter();

            tester1.InitialChannelCH_COM4(null);

            Assert.IsTrue(tester1.CH_COM4.Open());

            tester1.A_Recv();

            Assert.IsTrue(tester1.head.Value == 0x01);
            Assert.IsTrue(tester1.len.Value == 0x02);
            Assert.IsTrue(tester1.end.Value == -158);

        }

    }
}
