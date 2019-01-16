//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FrameIOUintTester
//{
//    [TestClass]
//    public class Test_CRC
//    {
//        //test_tcp
//        [TestMethod]
//        public void _CRC()
//        {
//            var tester = new test_crc.test_crc();

//            tester.InitialChannelCHS(null);
//            tester.InitialChannelCHC(null);

//            Assert.IsTrue(tester.CHS.Open());
//            Assert.IsTrue(tester.CHC.Open());

//            tester.head.Value = 0x02;
//            tester.len.Value = 0x06;

//            tester.A_Send();

//            tester.A_Recv();

//            Assert.IsTrue(tester.head.Value == 0x02);
//            Assert.IsTrue(tester.len.Value == 0x06);
//            Assert.IsTrue(tester.end.Value == 0x08);

//        }
//    }
//}
