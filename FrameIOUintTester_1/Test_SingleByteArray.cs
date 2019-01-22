using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestProject_FrameIO;

namespace UnitTestProject_FrameIO
{
    [TestClass]
    public class Test_SingleByteArray
    {
        [TestMethod]
        public void SingleByteArray_Test_ByteArray_IsTrue()
        {
            var tester1 = new SingleByteArray.sub_SingleByteArray();
            var tester2 = new SingleByteArray.sub_SingleByteArray();

            tester1.InitialParameter();
            tester2.InitialParameter();

            tester1.InitialChannelCHS(null);
            tester2.InitialChannelCHC(null);

            Assert.IsTrue(tester1.CHS.Open());
            Assert.IsTrue(tester2.CHC.Open());
            Assert.IsTrue(tester1.end.Count == 0);

            tester1.end.Add(new SingleByteArray.Parameter<byte?>(10));
            tester1.end.Add(new SingleByteArray.Parameter<byte?>(98));
            tester1.end.Add(new SingleByteArray.Parameter<byte?>(2));

            tester1.head.Value = (byte)tester1.end.Count;

            tester1.len[0].Value = 2;
            tester1.len[1].Value = 3;



            tester1.A_Send();

            tester2.end.Add(new SingleByteArray.Parameter<byte?>(0));
            tester2.end.Add(new SingleByteArray.Parameter<byte?>(0));
            tester2.end.Add(new SingleByteArray.Parameter<byte?>(0));

            tester2.A_Recv();


            Assert.IsTrue(tester2.head.Value == 3);
            Assert.IsTrue(tester2.len[0].Value == 2);
            Assert.IsTrue(tester2.len[1].Value == 3);
            Assert.IsTrue(tester2.end[0].Value == 10);
            Assert.IsTrue(tester2.end[1].Value == 98);
            Assert.IsTrue(tester2.end[2].Value == 2);

        }
    }
}
