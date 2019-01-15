//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FrameIOUintTester
//{
//    [TestClass]
//    public class Test_array
//    {
//        //test_tcp
//        [TestMethod]
//        public void Test()
//        {
//            var tester = new test_array.testarray();

//            tester.InitialChannelCH_COM3(null);
//            tester.InitialChannelCH_COM4(null);

//            Assert.IsTrue(tester.CH_COM3.Open());
//            Assert.IsTrue(tester.CH_COM4.Open());

//            for (int i = 0; i < 5; i++)
//                tester.content[i].Value = (uint)i;


//            tester.A_Send();

//            tester.A_Recv();

//            Assert.IsTrue(tester.content[0].Value == 0);
//            Assert.IsTrue(tester.content[1].Value == 1);
//            Assert.IsTrue(tester.content[2].Value == 2);
//            Assert.IsTrue(tester.content[3].Value == 3);
//            Assert.IsTrue(tester.content[4].Value == 4);

//        }
//    }
//}
