﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrameIO.Run;
using System.Threading;

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
        public void A_FioNetRunner_FrameOneByte()
        {
            var sys1 = new frame_test_onebyte.subsys1();
            sys1.InitialParameter();
            sys1.InitialChanneltcp_recv(null);
            Assert.IsTrue(sys1.tcp_recv.Open());

            var sys2 = new frame_test_onebyte.subsys1();
            sys2.InitialParameter();
            sys2.InitialChanneltcp_send(null);
            Assert.IsTrue(sys2.tcp_send.Open());

            sys2.pro1.Value = 55;
            sys2.SendData();

            sys1.RecvData();
            Assert.IsTrue(sys1.pro1.Value == 55);
        }

        [TestMethod]
        public void A_FioNetRunner_FrameByteArray()
        {
            var sys1 = new frame_test_bytearray.subsys1();
            sys1.InitialParameter();
            sys1.InitialChanneltcp_recv(null);
            Assert.IsTrue(sys1.tcp_recv.Open());

            var sys2 = new frame_test_bytearray.subsys1();
            sys2.InitialParameter();
            sys2.InitialChanneltcp_send(null);
            Assert.IsTrue(sys2.tcp_send.Open());

            sys2.pro1[0].Value = 55;
            sys2.pro1[1].Value = 66;
            sys2.SendData();

            sys1.RecvData();
            Assert.IsTrue(sys1.pro1[0].Value == 55);
            Assert.IsTrue(sys1.pro1[1].Value == 66);
        }

    }
}
