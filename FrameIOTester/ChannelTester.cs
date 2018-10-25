using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOTester
{
    class ChannelTester
    {
        //oneof 分支测试
        [TestMethod]
        public void TcpServerTest()
        {
            var client = new tcp_server_test.SYS1();
            var server = new tcp_server_test.SYS2();

            server.InitialChannelCHA(null);
            server.InitialChannelCHB(null);
            client.InitialChannelCH1(null);

            Assert.IsTrue(server.CHA.Open());
            Assert.IsTrue(server.CHB.Open());
            //Assert.IsFalse(server.CHA.Open());

            client.PROPERTYa.Value = 100;

            client.SendData();

            server.RecvDataCHB();
            Assert.IsTrue(server.PROPERTY2a.Value == 200);

            server.RecvDataCHA();
            Assert.IsTrue(server.PROPERTY2a.Value == 100);

            //var client2 = new tcp_server_test.SYS3();
            //client2.InitialChannelCH1(null);
            //client2.CH1.Open();
            //client2.PROPERTYa.Value = 200;
            //client2.SendData();

        }
    }
}
