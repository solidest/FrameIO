using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrameIO.Main;
using System.Collections.Generic;
using FrameIO.Runtime;

namespace FrameIO.Tester
{
    [TestClass]
    public class FrameCompileTester
    {

        //创建整数字段
        private FrameSegmentInteger GetNewInteger()
        {
            var seg = new FrameSegmentInteger();
            seg.Name = "seg1";
            seg.BitCount = 8;
            seg.ByteOrder = ByteOrderType.Big;
            seg.Encoded = EncodedType.Inversion;
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
            frm.Segments.Add(GetNewInteger());

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


        //初始化运行时库
        [TestMethod]
        public void RuntimeInitialTest()
        {
            var config = GetCompiledData();
            FrameIOFactory.Initial(config);
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
            Assert.IsTrue(pack.Pack().Length == 1);
        }

        //解包数据测试
        [TestMethod]
        public void UnpackTest()
        {
            RuntimeInitialTest();
            var settor = FrameIOFactory.GetFrameSettor(1);
            settor.SetSegmentValue(2, 99);
            var data = settor.GetPack().Pack();

            var unpacker = FrameIOFactory.GetFrameUnpacker(1);
            Assert.IsNotNull(unpacker);
            var res = unpacker.AppendBlock(data);
            Assert.IsTrue(res == 0);
            var gettor = unpacker.Unpack();
            Assert.IsTrue(gettor.GetUInt(2) == 99);
        }

    }
}
