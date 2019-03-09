using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_tcp
    {
        //test_tcp
        [TestMethod]
        public void TCP()
        {
            var tester = new test_tcp.test_tcp();
            tester.InitialParameter();
            tester.InitialChannelCHS(null);
            tester.InitialChannelCHC(null);

            Assert.IsTrue(tester.CHS.Open());
            //Assert.IsTrue(tester.CHC.Open());

            //tester.head.Value = 0x55;
            //tester.len.Value = 1;
            //tester.end.Value = 0xaa;

            //tester.A_Send();

            tester.A_Recv();

            Assert.IsTrue(tester.head.Value == 0x55);
            Assert.IsTrue(tester.len.Value == 1);
            Assert.IsTrue(tester.end.Value == 0xaa);


        }
        /// <summary>
        /// TCP 先启动客户端，后启动服务器
        /// </summary>
        [TestMethod]
        public void Test_TCP_Client_StartClientBeforeServer()
        {
            var tester = new test_tcp.test_tcp();
            tester.InitialParameter();
            tester.InitialChannelCHC(null);

            Assert.IsFalse(tester.CHC.Open());

            tester.InitialChannelCHC(null);
            Assert.IsTrue(tester.CHC.Open());

        }
        /// <summary>
        /// TCP  客户端断开重连
        /// </summary>
        [TestMethod]
        public void Test_TCP_Client_repeatConnectAfterDown()
        {
            var tester = new test_tcp.test_tcp();
            tester.InitialParameter();

            tester.InitialChannelCHC(null);
            Assert.IsTrue(tester.CHC.Open());

            //TODO:此时断开服务器
            tester.InitialChannelCHC(null);
            Assert.IsFalse(tester.CHC.Open());

            //TODO:此时再次启动服务器
            tester.InitialChannelCHC(null);
            Assert.IsTrue(tester.CHC.Open());

            Assert.IsTrue(true);
        }
    }
}
