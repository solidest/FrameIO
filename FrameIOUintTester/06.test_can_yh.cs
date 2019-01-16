//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FrameIOUintTester
//{
//    [TestClass]
//    public class Test_CAN_YH
//    {
//        //test_tcp
//        [TestMethod]
//        public void CAN()
//        {
//            var tester = new test_can_yh.testcan();

//            tester.InitialChannelCH_CAN1(null);
//            tester.InitialChannelCH_CAN2(null);

//            Assert.IsTrue(tester.CH_CAN1.Open());
//            Assert.IsTrue(tester.CH_CAN2.Open());

//            tester.len.Value = 4;
//            tester.data1.Value = 0x01;
//            tester.data2.Value = 0x02;
//            tester.data3.Value = 0x03;
//            tester.data4.Value = 0x04;
//            tester.data5.Value = 0x05;
//            tester.data6.Value = 0x06;
//            tester.data7.Value = 0x07;
//            tester.data8.Value = 0x08;

//            tester.A_Send();

//            tester.A_Recv();

//            Assert.IsTrue(tester.data1.Value == 0x01);
//            Assert.IsTrue(tester.data2.Value == 0x02);
//            Assert.IsTrue(tester.data3.Value == 0x03);
//            Assert.IsTrue(tester.data4.Value == 0x04);
//            Assert.IsTrue(tester.data5.Value == 0x05);
//            Assert.IsTrue(tester.data6.Value == 0x06);
//            Assert.IsTrue(tester.data7.Value == 0x07);
//            Assert.IsTrue(tester.data8.Value == 0x08);



//        }
//    }
//}
