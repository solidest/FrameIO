using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_udp_project
    {
        //test_tcp
        [TestMethod]
        public void UDP()
        {
            var tester = new Project.Subsystem();
            tester.InitialParameter();

            tester.InitialChannelUdp(null);

            Assert.IsTrue(tester.Udp.Open());


            tester.ReceiveFrame();
            tester.ReceiveFrame();
            tester.ReceiveFrame();
            tester.ReceiveFrame();
            tester.ReceiveFrame();

            //tester.YiJianTiaoPing
            //Assert.IsTrue(tester.YiJianTiaoPingFanKui.Value == Project.Subsystem_GongZuoKongZhi.GuanBi );


        }
    }
}
