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
        public void A__FrameRunnerInitial()
        {
            var ch = frame_test_onebyte.FioNetRunner.GetChannel(null);
            Assert.IsNull(ch);
        }

        //字节未对齐
        [TestMethod]
        public void A__FrameNotAlignByte()
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

        #region --byte收发--

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
            sys2.pro1[7].Value = 99;
            sys2.SendData();

            sys1.RecvData();
            Assert.IsTrue(sys1.pro1[0].Value == 55);
            Assert.IsTrue(sys1.pro1[1].Value == 66);
            Assert.IsTrue(sys1.pro1[7].Value == 99);
        }

        #endregion

        #region --bool收发--

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

            sys2.SendData();
            sys1.RecvData();

            Assert.IsTrue((bool)sys1.pro1[0].Value);
            Assert.IsFalse((bool)sys1.pro1[3].Value);
            Assert.IsTrue((bool)sys1.pro1[6].Value);
            Assert.IsTrue((bool)sys1.pro2.Value);
        }


        #endregion

        #region --group收发--


        //group字段组
        [TestMethod]
        public void A__FrameGroup()
        {
            var sys1 = new frame_test_seggroup.subsys1();
            sys1.InitialParameter();
            sys1.InitialChanneltcp_recv(null);
            Assert.IsTrue(sys1.tcp_recv.Open());

            var sys2 = new frame_test_seggroup.subsys1();
            sys2.InitialParameter();
            sys2.InitialChanneltcp_send(null);
            Assert.IsTrue(sys2.tcp_send.Open());

            sys2.pro1[0].Value = true;
            sys2.pro1[6].Value = true;
            sys2.pro2.Value = true;
            sys2.pro3_1.Value = 55;
            sys2.pro3_2.Value = 66;

            sys2.SendData();
            sys1.RecvData();

            Assert.IsTrue((bool)sys1.pro1[0].Value);
            Assert.IsFalse((bool)sys1.pro1[3].Value);
            Assert.IsTrue((bool)sys1.pro1[6].Value);
            Assert.IsTrue((bool)sys1.pro2.Value);
            Assert.IsTrue(sys1.pro3_1.Value == 55);
            Assert.IsTrue(sys1.pro3_2.Value == 66);

        }

        #endregion

        #region --子系统收发--
     
        //子系统收发
        [TestMethod]
        public void A__FrameSubsys()
        {
            var sys1 = new frame_test_subsys.subsys1();
            sys1.InitialParameter();
            sys1.InitialChanneltcp_recv(null);
            Assert.IsTrue(sys1.tcp_recv.Open());

            var sys2 = new frame_test_subsys.subsys1();
            sys2.InitialParameter();
            sys2.InitialChanneltcp_send(null);
            Assert.IsTrue(sys2.tcp_send.Open());

            sys2.pro1[0].Value = true;
            sys2.pro1[6].Value = true;
            sys2.pro2.Value = true;
            sys2.pro3.seg3_1.Value = 55;
            sys2.pro3.seg3_2.Value = 66;

            sys2.SendData();
            sys1.RecvData();

            Assert.IsTrue((bool)sys1.pro1[0].Value);
            Assert.IsFalse((bool)sys1.pro1[3].Value);
            Assert.IsTrue((bool)sys1.pro1[6].Value);
            Assert.IsTrue((bool)sys1.pro2.Value);
            Assert.IsTrue(sys1.pro3.seg3_1.Value == 55);
            Assert.IsTrue(sys1.pro3.seg3_2.Value == 66);
        }


        //子系统数组收发
        [TestMethod]
        public void A__FrameSubsysArray()
        {
            var sys1 = new frame_test_subsysarray.subsys1();
            sys1.InitialParameter();
            sys1.InitialChanneltcp_recv(null);
            Assert.IsTrue(sys1.tcp_recv.Open());

            var sys2 = new frame_test_subsysarray.subsys1();
            sys2.InitialParameter();
            sys2.InitialChanneltcp_send(null);
            Assert.IsTrue(sys2.tcp_send.Open());

            sys2.pro1[0].Value = true;
            sys2.pro1[6].Value = true;
            sys2.pro2.Value = true;
            sys2.pro3[0].seg3_1.Value = 55;
            sys2.pro3[1].seg3_2.Value = 66;
            sys2.pro3[6].seg3_2.Value = 1;
            sys2.pro3[7].seg3_2.Value = 23;
            sys2.pro3[8].seg3_1.Value = 45;

            sys2.SendData();
            sys1.RecvData();

            Assert.IsTrue((bool)sys1.pro1[0].Value);
            Assert.IsFalse((bool)sys1.pro1[3].Value);
            Assert.IsTrue((bool)sys1.pro1[6].Value);
            Assert.IsTrue((bool)sys1.pro2.Value);
            Assert.IsTrue(sys1.pro3[0].seg3_1.Value == 55);
            Assert.IsTrue(sys1.pro3[1].seg3_2.Value == 66);
            Assert.IsTrue(sys1.pro3[6].seg3_2.Value == 1);
            Assert.IsTrue(sys1.pro3[7].seg3_2.Value == 23);
            Assert.IsTrue(sys1.pro3[8].seg3_1.Value == 45);

        }


        #endregion

        #region --OneOf分支测试--
        //子系统收发
        [TestMethod]
        public void A__FrameOneof()
        {
            var sys1 = new frame_test_oneof.SYS1();
            var sys2 = new frame_test_oneof.SYS2();

            sys1.InitialParameter();
            sys2.InitialParameter();

            sys1.InitialChannelCH1(null);
            sys2.InitialChannelCH1(null);

            sys2.CH1.Open();
            sys1.CH1.Open();

            sys1.one.Add(new frame_test_oneof.Parameter<double?>() { Value = 100.99 });
            sys1.one.Add(new frame_test_oneof.Parameter<double?>() { Value = 99901.90 });
            sys1.one.Add(new frame_test_oneof.Parameter<double?>() { Value = 99901.09 });

            sys1.b.Value = 12;
            sys1.count.Value = 3;

            sys1.SendData(frame_test_oneof.emtest.one, frame_test_oneof.emtest.one);
            sys2.RecvData();

            Assert.IsTrue(sys2.one[0].Value == 100.99);
            Assert.IsTrue(sys2.one[1].Value == 99901.90);
            Assert.IsTrue(sys2.one[2].Value == 99901.09);


            sys1.two.Value = 9996667;
            sys1.b.Value = 0;
            sys1.SendData(frame_test_oneof.emtest.two, frame_test_oneof.emtest.two);
            sys2.RecvData();
            Assert.IsTrue(sys2.two.Value == 9996667);

            sys1.CH1.Close();
            sys2.CH1.Close();
        }




        #endregion

    }
}
