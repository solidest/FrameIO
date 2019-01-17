using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_Enum
    {
        [TestMethod]
        public void E_type1()
        {
            var tester = new test_enum.testenum();

            tester.InitialParameter();
            tester.InitialChannelCH_COM3(null);
            tester.InitialChannelCH_COM4(null);

            Assert.IsTrue(tester.CH_COM3.Open());
            Assert.IsTrue(tester.CH_COM4.Open());

            tester.datetype.Value = 1;
            tester.name1.Value = 1;
            tester.name2.Value = 2;

            tester.A_Send_Type1(test_enum.Enum_Type.enum_type1);


            tester.A_Recv();

            Assert.IsTrue(tester.datetype.Value == 1);
            Assert.IsTrue(tester.name1.Value == 1);
            Assert.IsTrue(tester.name2.Value == 2);

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
