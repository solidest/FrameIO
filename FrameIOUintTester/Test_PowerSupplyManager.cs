using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_PowerSupplyManager
    {
        //PowerSupplyManager
        [TestMethod]
        public void PowerSupplyManager()
        {
            var tester = new test_PowerSupplyManager.PowerSupplyManager();

            tester.InitialChannelCHServer(null);


            Assert.IsTrue(tester.CHServer.Open());


            tester.SetpointVoltage.Value = 10;
            tester.SetpointCurrent.Value = 20;

            tester.Send_SetPowerSupply();

            tester.Recv_RecvData();

            Assert.IsTrue(tester.OutputVoltage.Value == 10);
            Assert.IsTrue(tester.OutputCurrent.Value == 20);


        }
    }
}
