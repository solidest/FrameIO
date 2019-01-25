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
            var tester1 = new frame_test_oneof.testenum();
            var tester2 = new frame_test_oneof.testenum();


            tester2.InitialParameter();
            tester2.InitialChanneltcp_recv(null);

            tester1.InitialParameter();
            tester1.InitialChanneltcp_send(null);

            Assert.IsTrue(tester2.tcp_recv.Open());
            Assert.IsTrue(tester1.tcp_send.Open());

            tester1.datetype.Value = 1;
            tester1.name1.Value = 1;
            tester1.name2.Value = 2;
            tester1.end1.Value = true;
            tester1.end7.Value = 100;
            tester1.pro1.Value = 6765;
            tester1.pro2.Value = 234;
            tester1.pro3.Value = 78;
            tester1.age1.Value = 60;
            tester1.age2.Value = 80;



            tester1.A_Send_Type1(frame_test_oneof.Enum_Type.enum_type1);

            tester2.A_Recv();

            Assert.IsTrue(tester2.datetype.Value == 1);
            Assert.IsTrue(tester2.name1.Value == 1);
            Assert.IsTrue(tester2.name2.Value == 2);
            Assert.IsTrue(tester2.end1.Value == true);
            Assert.IsTrue(tester2.end7.Value == 100);
            Assert.IsTrue(tester2.pro1.Value == 6765);
            Assert.IsTrue(tester2.pro2.Value == 234);
            Assert.IsTrue(tester2.pro3.Value == 78);

            tester1.A_Send_Type1(frame_test_oneof.Enum_Type.enum_type2);

            tester2.A_Recv();
            Assert.IsTrue(tester2.datetype.Value == 2);
            Assert.IsTrue(tester2.age1.Value == 60);
            Assert.IsTrue(tester2.age2.Value == 80);
            Assert.IsTrue(tester2.end1.Value == true);
            Assert.IsTrue(tester2.end7.Value == 100);
            Assert.IsTrue(tester2.pro1.Value == 6765);
            Assert.IsTrue(tester2.pro2.Value == 234);
            Assert.IsTrue(tester2.pro3.Value == 78);
        }




        #endregion

        #region --动态数组--
        [TestMethod]
        public void A__FrameDynamicArray()
        {
            var tester1 = new frame_test_dynamicarray.sub_SingleByteArray();
            var tester2 = new frame_test_dynamicarray.sub_SingleByteArray();

            tester1.InitialParameter();
            tester2.InitialParameter();

            tester1.InitialChannelCHS(null);
            tester2.InitialChannelCHC(null);

            Assert.IsTrue(tester1.CHS.Open());
            Assert.IsTrue(tester2.CHC.Open());
            Assert.IsTrue(tester1.end.Count == 0);

            tester1.end.Add(new frame_test_dynamicarray.Parameter<byte?>(10));
            tester1.end.Add(new frame_test_dynamicarray.Parameter<byte?>(98));
            tester1.end.Add(new frame_test_dynamicarray.Parameter<byte?>(2));

            tester1.head.Value = (byte)tester1.end.Count;

            tester1.len[0].Value = 2;
            tester1.len[1].Value = 3;
           


            tester1.A_Send();

            tester2.end.Add(new frame_test_dynamicarray.Parameter<byte?>(0));
            tester2.end.Add(new frame_test_dynamicarray.Parameter<byte?>(0));
            tester2.end.Add(new frame_test_dynamicarray.Parameter<byte?>(0));

            tester2.A_Recv();


            Assert.IsTrue(tester2.head.Value == 3);
            Assert.IsTrue(tester2.len[0].Value == 2);
            Assert.IsTrue(tester2.len[1].Value == 3);
            Assert.IsTrue(tester2.end[0].Value == 10);
            Assert.IsTrue(tester2.end[1].Value == 98);
            Assert.IsTrue(tester2.end[2].Value == 2);
        }


        #endregion

        #region --验证--

        [TestMethod]
        public void A__FrameValidate()
        {

            var tester1 = new frame_test_validate.SYS1();
            var tester2 = new frame_test_validate.SYS2();

            tester1.InitialParameter();
            tester2.InitialParameter();

            tester2.InitialChannelCHA(null);
            tester1.InitialChannelCH1(null);

            Assert.IsTrue(tester2.CHA.Open());
            Assert.IsTrue(tester1.CH1.Open());

            tester1.PROPERTYa.Value = 2;

            Exception ex = null;

            try
            {
                tester1.SendData();
                tester2.RecvData();
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.IsNotNull(ex);

        }

        #endregion

        #region --ByteSizeOf--


        [TestMethod]
        public void A__FrameByteSizeOf()
        {

            var tester1 = new frame_test_bytesizeof.SYS1();
            var tester2 = new frame_test_bytesizeof.SYS2();

            tester1.InitialParameter();
            tester2.InitialParameter();

            tester2.InitialChannelCHA(null);
            tester1.InitialChannelCH1(null);

            Assert.IsTrue(tester2.CHA.Open());
            Assert.IsTrue(tester1.CH1.Open());

            tester1.PROPERTYa.Value = 2;


            tester1.SendData();
            tester2.RecvData();
            Assert.IsTrue(tester2.len.Value == 47);

        }


        #endregion

        #region --Crc--

        [TestMethod]
        public void A__FrameCRC()
        {

            var tester1 = new frame_test_crc.SYS1();
            var tester2 = new frame_test_crc.SYS2();

            tester1.InitialParameter();
            tester2.InitialParameter();

            tester2.InitialChannelCHA(null);
            tester1.InitialChannelCH1(null);

            Assert.IsTrue(tester2.CHA.Open());
            Assert.IsTrue(tester1.CH1.Open());

            tester1.PROPERTYa.Value = 4;
            tester1.PROPERTYb.Value = 9;
            tester1.PROPERTYc.Value = 9;
            tester1.PROPERTYd.Value = 19.563;
            tester1.PROPERTYe[0].Value  = true;

            tester1.SendData();
            tester2.RecvData();
            Assert.IsTrue(tester2.check_value.Value == 168);

        }

        #endregion

        #region --Match--

        [TestMethod]
        public void A__FrameMatch()
        {

            var sender = new frame_test_match.subsys1();
            var recver = new frame_test_match.subsys1();

            sender.InitialParameter();
            recver.InitialParameter();

            recver.InitialChanneltcp_recv(null);
            sender.InitialChanneltcp_send(null);

            Assert.IsTrue(recver.tcp_recv.Open());
            Assert.IsTrue(sender.tcp_send.Open());

            sender.pro0.Value = 99;
            sender.pro1.Value = 0x55;
            sender.pro2.Value = 0xAA;
            sender.pro3.Value = 0x55;
            sender.pro4.Value = 2;
            sender.pro5.Value = 3;

            sender.SendData();
            recver.RecvData();

            Assert.IsTrue(recver.header3.Value == 3);

        }

        #endregion

        #region --Int--
        [TestMethod]
        public void A__FrameInt()
        {

            var tester1 = new frame_test_long.subsys1();
            var tester2 = new frame_test_long.subsys1();

            tester1.InitialParameter();
            tester2.InitialParameter();

            tester2.InitialChanneltcp_recv(null);
            tester1.InitialChanneltcp_send(null);

            Assert.IsTrue(tester2.tcp_recv.Open());
            Assert.IsTrue(tester1.tcp_send.Open());

            tester1.pro1.Value = -9955584;
            tester1.pro2.Value = -6.555;
            tester1.pro3.Value = -765.345f;

            tester1.SendData();
            tester2.RecvData();
            Assert.IsTrue(tester2.pro1.Value == -9955584);
            Assert.IsTrue(tester2.pro2.Value == -6.555);
            Assert.IsTrue(tester2.pro3.Value == -765.345f);

        }

        #endregion

    }

}
