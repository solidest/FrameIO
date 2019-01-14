using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrameIO.Run;

namespace FrameIORunNetTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void A_FioNetRunner_Initial()
        {
            var ch = frame_test_onebyte.FioNetRunner.GetChannel(null);
            Assert.IsNull(ch);
        }

        [TestMethod]
        public void A_FioNetRunner_SendOneByte()
        {
            var sys1 = new frame_test_onebyte.subsys1();
            sys1.InitialParameter();
            sys1.InitialChanneltcp_recv(null);

            var sys2 = new frame_test_onebyte.subsys1();
            sys2.InitialParameter();
            sys2.InitialChanneltcp_send(null);

            sys2.pro1.Value = 55;
            sys2.SendData();
            sys1.RecvData();
            Assert.IsTrue(sys1.pro1.Value == 55);
        }

    }
}
