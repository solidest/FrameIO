using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_SubSys_Array
    {
        //test_tcp
        [TestMethod]
        public void Test_SubSystem_Array()
        {
            var tester = new test_subsys_Array.testsubsys();
            tester.InitialParameter();

            tester.InitialChannelCH_UDP_SEND(null);
            tester.InitialChannelCH_UDP_RECV(null);

            Assert.IsTrue(tester.CH_UDP_SEND.Open());
            Assert.IsTrue(tester.CH_UDP_RECV.Open());

            tester.head.Value = 1;
            tester.len.Value = 2;
            tester.end.Value = 3;

            for(int i=0;i<3;i++)
            {
                tester.pos[i].jingdu.Value = 89.87f+i;
                tester.pos[i].weidu.Value = 92.53f+i;
            }

            tester.A_Send();

            tester.A_Recv();

            Assert.IsTrue(tester.head.Value == 1);
            Assert.IsTrue(tester.len.Value == 2);
            Assert.IsTrue(tester.end.Value == 3);
            Assert.IsTrue(tester.pos[0].jingdu.Value == 89.87f);
            Assert.IsTrue(tester.pos[0].weidu.Value == 92.53f);
            Assert.IsTrue(tester.pos[1].jingdu.Value == 90.87f);
            Assert.IsTrue(tester.pos[1].weidu.Value == 93.53f);
            Assert.IsTrue(tester.pos[2].jingdu.Value == 91.87f);
            Assert.IsTrue(tester.pos[2].weidu.Value == 94.53f);
        }
    }
}
