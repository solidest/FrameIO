using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_udp_project_com
    {
        //test_tcp
        [TestMethod]
        public void com()
        {
            var tester_s = new Project_com.Subsystem();
            var tester_r = new Project_com.Subsystem();

            tester_r.InitialParameter();

            tester_r.InitialChannelCH_COM4(null);

            Assert.IsTrue(tester_r.CH_COM4.Open());


            tester_r.ReceiveFrame();
            tester_r.ReceiveFrame();
            tester_r.ReceiveFrame();
            tester_r.ReceiveFrame();
            tester_r.ReceiveFrame();

            //tester.YiJianTiaoPing
            //Assert.IsTrue(tester.YiJianTiaoPingFanKui.Value == Project.Subsystem_GongZuoKongZhi.GuanBi );


        }
    }
}
