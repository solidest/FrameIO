﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrameIO.Main;
using System.Collections.Generic;
using FrameIO.Runtime;

namespace FrameIO.Tester
{
    [TestClass]
    public class FrameCompileTester
    {
        #region --Prepare Segments--

        //创建整数字段
        private FrameSegmentInteger GetNewIntegerSegment()
        {
            var seg = new FrameSegmentInteger();
            seg.Name = "segint";
            seg.BitCount = 8;
            seg.ByteOrder = ByteOrderType.Big;
            seg.Encoded = EncodedType.Inversion;
            seg.VMax = "100.99";
            seg.VMin = "-25";
            return seg;
        }

        //创建整数数组
        private FrameSegmentInteger GetNewIntegerArraySegment()
        {
            var seg = new FrameSegmentInteger();
            seg.Repeated = new Exp { Op = exptype.EXP_INT, ConstStr = "8" };
            seg.Name = "segintarray";
            seg.BitCount = 4;
            seg.Signed = true;
            seg.ByteOrder = ByteOrderType.Big;
            seg.Encoded = EncodedType.Complement;
            seg.VMax = "100.99";
            seg.VMin = "-25";
            return seg;
        }

        //创建浮点数字段
        private FrameSegmentReal GetNewRealSegment()
        {
            var seg = new FrameSegmentReal();
            seg.Name = "segreal";
            seg.IsDouble = true;
            seg.ByteOrder = ByteOrderType.Big;
            seg.Encoded = EncodedType.Primitive;
            seg.Value = new Exp() { Op = exptype.EXP_REAL, ConstStr = "10.8987" };
            seg.VMax = "1000";
            seg.VMin = "-999";
            return seg;
        }

        //创建浮点数组
        private FrameSegmentReal GetNewRealArraySegment()
        {
            var seg = new FrameSegmentReal();
            seg.Repeated = new Exp { Op = exptype.EXP_INT, ConstStr = "8" };
            seg.Name = "segrealarray";
            seg.IsDouble = false;
            seg.ByteOrder = ByteOrderType.Big;
            seg.Encoded = EncodedType.Complement;
            seg.VMax = "100.99";
            seg.VMin = "-25";
            return seg;
        }

        //编译数据帧
        private byte[] GetCompiledData()
        {
            //准备测试数据
            var frm = new Frame("frametest");
            frm.Segments = new System.Collections.ObjectModel.ObservableCollection<FrameSegmentBase>();
            frm.Segments.Add(GetNewIntegerSegment());
            frm.Segments.Add(GetNewRealSegment());
            frm.Segments.Add(GetNewIntegerArraySegment());
            frm.Segments.Add(GetNewRealArraySegment());

            var pj = new IOProject("projecttest");
            pj.FrameList = new System.Collections.ObjectModel.ObservableCollection<Frame>();
            pj.FrameList.Add(frm);

            //编译分析结果
            var cfrms = FrameCompiledFile.Compile(pj);
            Assert.IsNotNull(cfrms);
            var config = cfrms.GetBytes();
            Assert.IsNotNull(config);

            return config;
        }

        #endregion

        //初始化运行时库
        [TestMethod]
        public void RuntimeInitialTest()
        {
            var config = GetCompiledData();
            FrameIOFactory.Initialize(config);
            Assert.IsTrue(FrameIOFactory.GetFrameSettor(1) != null);
        }

        //打包数据测试
        [TestMethod]
        public void PackTest()
        {
            RuntimeInitialTest();
            var settor = FrameIOFactory.GetFrameSettor(1);
            Assert.IsNotNull(settor);
            settor.SetSegmentValue(2, 99);
            var pack = settor.GetPack();
            Assert.IsTrue(pack.Pack()!=null);
        }

        //解包数据测试
        [TestMethod]
        public void UnpackTest()
        {
            RuntimeInitialTest();
            var settor = FrameIOFactory.GetFrameSettor(1);

            //字段1 整数
            settor.SetSegmentValue(2, 99);

            //字段2 小数
            settor.SetSegmentValue(3, -999.000999);

            //字段3 整数数组
            var barr = new sbyte?[8];
            for (int i = 0; i < 8; i++)
                barr[i] = (i % 2 == 0 ?(sbyte)-4 : (sbyte)6);
            settor.SetSegmentValue(4, barr);

            //字段4 浮点数组
            var rarr = new float?[7];
            for (int i=0; i<7; i++)
                rarr[i] = (i % 2 == 0 ? (float)-9.65342 : (float)9999.7776);
            settor.SetSegmentValue(5, rarr);

            var data = settor.GetPack().Pack();

            var unpacker = FrameIOFactory.GetFrameUnpacker(1);
            Assert.IsNotNull(unpacker);
            var res = unpacker.AppendBlock(data);
            Assert.IsTrue(res == 0);
            var gettor = unpacker.Unpack();
            Assert.IsTrue(gettor.GetUInt(2) == 99);
            Assert.IsTrue(gettor.GetDouble(3) == -999.000999);
            var barr2 = gettor.GetSByteArray(4);
            for (int i = 0; i < 8; i++)
                Assert.IsTrue(barr[i] == barr2[i]);
            var rarr2 = gettor.GetFloatArray(5);
            for (int i = 0; i < 7; i++)
                Assert.IsTrue(rarr[i] == rarr2[i]);

        }

        //用户接受测试--基本
        [TestMethod]
        public void UserAcceptTestBase()
        {
            var sys1 = new demo.SYS1();
            var sys2 = new demo.SYS2();

            sys1.InitialChannelCH1(null);
            sys2.InitialChannelCHA(null);

            sys2.CHA.Open();
            sys1.CH1.Open();

            sys1.PROPERTYa.Value = 1;
            sys1.PROPERTYb.Value = -2;
            sys1.PROPERTYc.Value = 3;
            sys1.PROPERTYd.Value = -4.5;
            sys1.PROPERTYe.Add(new demo.Parameter<bool?>() { Value = true });
            sys1.PROPERTYe.Add(new demo.Parameter<bool?>() { Value = true });

            sys1.SendData();
            sys2.RecvData();

            Assert.IsTrue(sys2.PROPERTY2a.Value == 1);
            Assert.IsTrue(sys2.PROPERTY2b.Value == -2);
            Assert.IsTrue(sys2.PROPERTY2c.Value == 3);
            Assert.IsTrue(sys2.PROPERTY2d.Value == -4.5);
            Assert.IsTrue((bool)sys2.PROPERTY2e[0].Value);
            Assert.IsTrue((bool)sys2.PROPERTY2e[1].Value);
            Assert.IsFalse((bool)sys2.PROPERTY2e[2].Value); //以下为默认值
            Assert.IsFalse((bool)sys2.PROPERTY2e[3].Value);
            Assert.IsFalse((bool)sys2.PROPERTY2e[4].Value);
            Assert.IsFalse((bool)sys2.PROPERTY2e[5].Value);
            Assert.IsFalse((bool)sys2.PROPERTY2e[6].Value);
            Assert.IsFalse((bool)sys2.PROPERTY2e[7].Value); 

            sys1.CH1.Close();
            sys2.CHA.Close();

        }

        [TestMethod]
        public void FrameRefTest()
        {
            var sys1 = new test_frame_ref.SYS1();
            var sys2 = new test_frame_ref.SYS2();

            sys1.InitialChannelCH1(null);
            sys2.InitialChannelCH1(null);

            sys2.CH1.Open();
            sys1.CH1.Open();

            sys1.count.Value = 2;
            sys1.dataarr.Add(new test_frame_ref.Parameter<double?>() { Value = 9876.9993 });
            sys1.dataarr.Add(new test_frame_ref.Parameter<double?>() { Value = 9876.9994 });

            sys1.SendData();
            sys2.RecvData();

            Assert.IsTrue(sys2.dataarr[1].Value == 9876.9994);

            sys1.CH1.Close();
            sys2.CH1.Close();
        }

        //oneof 分支测试
        [TestMethod]
        public void OneOfTest()
        {
            var sys1 = new test_oneof.SYS1();
            var sys2 = new test_oneof.SYS2();

            sys1.InitialChannelCH1(null);
            sys2.InitialChannelCH1(null);

            sys2.CH1.Open();
            sys1.CH1.Open();

            sys1.one.Add(new test_oneof.Parameter<double?>() { Value = 100.99 });
            sys1.one.Add(new test_oneof.Parameter<double?>() { Value = 99901.99 });

            sys1.b.Value = 12;
            sys1.count.Value = 2;

            sys1.SendData();
            sys2.RecvData();

            Assert.IsTrue(sys2.one[1].Value == 99901.99);

            sys1.CH1.Close();
            sys2.CH1.Close();
        }

    }
}