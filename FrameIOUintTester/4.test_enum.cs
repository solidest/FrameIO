﻿//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FrameIOUintTester
//{
//    [TestClass]
//    public class Test_Enum
//    {
//        [TestMethod]
//        public void ENUM()
//        {
//            var tester = new test_enum.testenum();

//            tester.InitialParameter();
//            tester.InitialChannelCH_COM3(null);
//            tester.InitialChannelCH_COM4(null);

//            Assert.IsTrue(tester.CH_COM3.Open());
//            Assert.IsTrue(tester.CH_COM4.Open());

//            tester.datetype.Value = 1;
//            tester.name1.Value = 1;
//            tester.name2.Value = 2;

//            tester.A_Send_Type1();

//            tester.A_Recv();

//            Assert.IsTrue(tester.datetype.Value == 1);
//            Assert.IsTrue(tester.name1.Value == 1);
//            Assert.IsTrue(tester.name2.Value == 2);


//        }
//    }
//}
