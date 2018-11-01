using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_com
    {
        //test_tcp
        [TestMethod]
        public void COM()
        {
            var tester = new test_com.testcom();

            tester.InitialChannelCH_COM3(null);
            tester.InitialChannelCH_COM4(null);

            Assert.IsTrue(tester.CH_COM3.Open());
            Assert.IsTrue(tester.CH_COM4.Open());

            tester.head.Value = 0x55;
            tester.len.Value = 1;
            tester.end.Value = 0xaa;

            tester.A_Send();

            tester.A_Recv();

            Assert.IsTrue(tester.head.Value == 0x55);
            Assert.IsTrue(tester.len.Value == 1);
            Assert.IsTrue(tester.end.Value == 0xaa);


        }
    }
}
