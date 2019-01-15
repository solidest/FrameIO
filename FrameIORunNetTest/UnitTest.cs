using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrameIO.Run;
using System.Threading;

namespace FrameIORunNetTest
{
    [TestClass]
    public class UnitTest
    {
        #region --基本测试--

        //初始化
        [TestMethod]
        public void A__FioNetRunnerInitial()
        {
            var ch = frame_test_onebyte.FioNetRunner.GetChannel(null);
            Assert.IsNull(ch);
        }

        //字节未对齐
        [TestMethod]
        public void A__NotAlignByte()
        {
            var sys1 = new frame_test_notaligbyte.subsys1();
            sys1.InitialParameter();
            sys1.InitialChanneltcp_recv(null);
            Assert.IsTrue(sys1.tcp_recv.Open());

            var sys2 = new frame_test_notaligbyte.subsys1();
            sys2.InitialParameter();
            sys2.InitialChanneltcp_send(null);
            Assert.IsTrue(sys2.tcp_send.Open());

            sys2.pro1[0].Value = true;
            sys2.pro1[6].Value = false;
            Exception ex = null;
            try
            {
                sys2.SendData();

                sys1.RecvData();

            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.IsNotNull(ex);
        }

        #endregion

        #region --字节收发--

        //单字节收发
        [TestMethod]
        public void A__FrameOneByte()
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

        //字节数组收发
        [TestMethod]
        public void A__FrameByteArray()
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

        #endregion

        #region --bool--

        //字节未对齐
        [TestMethod]
        public void A__FrameBool()
        {
            var sys1 = new frame_test_bool.subsys1();
            sys1.InitialParameter();
            sys1.InitialChanneltcp_recv(null);
            Assert.IsTrue(sys1.tcp_recv.Open());

            var sys2 = new frame_test_bool.subsys1();
            sys2.InitialParameter();
            sys2.InitialChanneltcp_send(null);
            Assert.IsTrue(sys2.tcp_send.Open());

            sys2.pro1[0].Value = true;
            sys2.pro1[6].Value = true;
            sys2.pro2.Value = true;
            Exception ex = null;

            sys2.SendData();
            sys1.RecvData();

            Assert.IsNull(ex);
            Assert.IsTrue((bool)sys1.pro1[0].Value);
            Assert.IsFalse((bool)sys1.pro1[3].Value);
            Assert.IsTrue((bool)sys1.pro1[6].Value);
            Assert.IsTrue((bool)sys1.pro2.Value);
        }


        #endregion


    }
}
