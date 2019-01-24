using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_Integer_max_min
    {
        //test_tcp
        [TestMethod]
        public void TestIntegerMaxMin()
        {
            var tester1 = new test_integer_max_min.testIntegerMaxMin();
            var tester2 = new test_integer_max_min.testIntegerMaxMin();

            tester1.InitialParameter();
            tester2.InitialParameter();

            tester1.InitialChannelCHS(null);
            tester2.InitialChannelCHC(null);

            Assert.IsTrue(tester1.CHS.Open());
            Assert.IsTrue(tester2.CHC.Open());

            tester1.head.Value = 0x50;
            tester1.len.Value = 2;
            tester1.end.Value = 101;

            Exception ex=null;
            try
            {
                tester1.A_Send();
                tester2.A_Recv();//接收时抛出异常
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsNotNull(ex);

        }

    }
}
