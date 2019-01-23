using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_tcp_nolocalip
    {
        //test_tcp
        [TestMethod]
        public void TCP_nolocalip()
        {
            var tester = new test_tcp_nolocalip .test_tcp();
            var tester_c = new test_tcp_nolocalip.test_tcp();

            tester.InitialParameter();
            tester_c.InitialParameter();

            tester.InitialChannelCHS(null);
            tester_c.InitialChannelCHC(null);

            
            Assert.IsTrue(tester.CHS.Open());
            //Assert.IsTrue(tester_c.CHC.Open());

           // tester_c.CHC.Close();

            Exception ex = null;
            try
            {
                tester.A_Recv();
            }
            catch (Exception e)
            {

                ex = e;
            }

            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.GetType() == typeof(FrameIO.Interface.FrameIOException));

            //Assert.IsTrue(tester.head.Value == 0x55);
            //Assert.IsTrue(tester.len.Value == 1);
            //Assert.IsTrue(tester.end.Value == 0xaa);


        }
    }
}
