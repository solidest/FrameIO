using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_oneof_before
    {
        [TestMethod]
        public void E_type1()
        {
            var tester = new test_oneof_before.test_oneof_before();
            var tester1 = new test_oneof_before.test_oneof_before();

            tester.InitialParameter();
            tester1.InitialParameter();

            tester.InitialChannelCH_COM3(null);
            tester1.InitialChannelCH_COM4(null);

            Assert.IsTrue(tester.CH_COM3.Open());
            Assert.IsTrue(tester1.CH_COM4.Open());

            tester.a.Value = 369;
            tester.b.Value = 2;
            tester.c.Value = 6;

            tester.datetype.Value = 1;
            tester.name1.Value = 1;
            tester.name2.Value = 2;

            tester.A_Send_Type1(test_oneof_before.Enum_Type.enum_type1);


            tester1.A_Recv();

            Assert.IsTrue(tester1.a.Value == 369);
            Assert.IsTrue(tester1.b.Value == 2);
            Assert.IsTrue(tester1.c.Value == 6);
            Assert.IsTrue(tester1.datetype.Value == 1);
            Assert.IsTrue(tester1.name1.Value == 1);
            Assert.IsTrue(tester1.name2.Value == 2);

        }
        [TestMethod]
        public void E_type2()
        {
            var tester = new test_enum.testenum();

            tester.InitialParameter();
            tester.InitialChannelCH_COM3(null);
            tester.InitialChannelCH_COM4(null);

            Assert.IsTrue(tester.CH_COM3.Open());
            Assert.IsTrue(tester.CH_COM4.Open());

            tester.datetype.Value = 2;
            tester.name1.Value = 1;
            tester.name2.Value = 2;
            tester.age1.Value = 10;
            tester.age2.Value = 20;

            tester.A_Send_Type1(test_enum.Enum_Type.enum_type2);

            tester.A_Recv();

            Assert.IsTrue(tester.datetype.Value == 2);
            Assert.IsTrue(tester.age1.Value == 10);
            Assert.IsTrue(tester.age2.Value == 20);

        }
    }
}
