using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_max_min1
    {
        //test_tcp
        [TestMethod]
        public void Test_Max_Min()
        {
            var tester1 = new SingleByteArray.sub_SingleByteArray();
            var tester2 = new SingleByteArray.sub_SingleByteArray();

            tester1.InitialParameter();
            tester2.InitialParameter();

            tester1.InitialChannelCHS(null);
            tester2.InitialChannelCHC(null);

            Assert.IsTrue(tester1.CHS.Open());
            Assert.IsTrue(tester2.CHC.Open());
            Assert.IsTrue(tester1.end.Count==0);

            tester1.end.Add(new SingleByteArray.Parameter<byte?>(10));
            tester1.end.Add(new SingleByteArray.Parameter<byte?>(98));
            tester1.end.Add(new SingleByteArray.Parameter<byte?>(2));

            tester1.head.Value = (byte)tester1.end.Count;

            tester1.len[0].Value = 2;
            tester1.len[1].Value = 3;

            tester1.A_Send();

            tester2.end.Add(new SingleByteArray.Parameter<byte?>());
            tester2.end.Add(new SingleByteArray.Parameter<byte?>());
            tester2.end.Add(new SingleByteArray.Parameter<byte?>());

            tester2.A_Recv();

            Assert.IsTrue(tester2.head.Value == 3);
            Assert.IsTrue(tester2.len[0].Value == 2);
            Assert.IsTrue(tester2.len[1].Value == 3);
            Assert.IsTrue(tester2.end[0].Value == 10);
            Assert.IsTrue(tester2.end[1].Value == 98);
            Assert.IsTrue(tester2.end[2].Value == 2);
        }
        [TestMethod]
        public void Test_byte()
        {
            var sys1 = new SingleByteArray.sub_SingleByteArray();
            var sys2 = new SingleByteArray.sub_SingleByteArray();

            sys1.InitialParameter();
            sys2.InitialParameter();

            sys1.InitialChannelCHS(null);
            sys2.InitialChannelCHC(null);

            sys1.CHS.Open();
            sys2.CHC.Open();

            sys1.end.Add(new SingleByteArray.Parameter<byte?>(1));
            sys1.end.Add(new SingleByteArray.Parameter<byte?>(2));
            sys1.end.Add(new SingleByteArray.Parameter<byte?>(3));

            sys1.head.Value = (byte)sys1.end.Count;
            sys1.len[0].Value = 2;
            sys1.len[1].Value = 3;

            sys1.A_Send();

            sys2.end.Add(new SingleByteArray.Parameter<byte?>());
            sys2.end.Add(new SingleByteArray.Parameter<byte?>());
            sys2.end.Add(new SingleByteArray.Parameter<byte?>());

            sys2.A_Recv();

            Assert.IsTrue(sys2.head.Value == 3);
            Assert.IsTrue(sys2.len[0].Value == 2);
            Assert.IsTrue(sys2.len[1].Value == 3);
            Assert.IsTrue(sys2.end[0].Value == 1);
            Assert.IsTrue(sys2.end[1].Value == 2);
            Assert.IsTrue(sys2.end[2].Value == 3);
        }
    }
}
