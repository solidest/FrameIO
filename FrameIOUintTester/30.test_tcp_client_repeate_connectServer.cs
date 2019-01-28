using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_tcp_close_open
    {
        [TestMethod]
        public void TCP_Client_Repeate_Close_ByAssistant()
        {
            //调试助手持续发送数据情况下 任务管理器里中断进程 
            var tester = new test_tcp_client_repeate_connectServer.testtcpclientrepeateconnectServer();

            tester.InitialParameter();

            tester.InitialChannelCHS(null);

            Assert.IsTrue(tester.CHS.Open());

            while (true)
            {
                System.Threading.Thread.Sleep(20);
                FrameIO.Interface.FrameIOException ex;
                try
                {
                    tester.A_Recv();
                    Assert.IsTrue(tester.head.Value == 1);

                }
                catch (Exception e)
                {
                    ex = (FrameIO.Interface.FrameIOException)e;
                    Assert.AreEqual(ex.ErrInfo, "客户端连接已断开!");
                    return;
                }
            }
        }
        [TestMethod]
        public void TCP_Client_Repeate_Open_Close()
        {
            //客户端重复断开连接
            var tester = new test_tcp_client_repeate_connectServer.testtcpclientrepeateconnectServer();

            tester.InitialParameter();

            tester.InitialChannelCHS(null);

            Assert.IsTrue(tester.CHS.Open());
            int loop = 2;
            while (loop>0)
            {
                loop--;
                System.Threading.Thread.Sleep(20);
                FrameIO.Interface.FrameIOException ex;
                try
                {
                    tester.A_Recv();
                    Assert.IsTrue(tester.head.Value == 1);
                    tester.head.Value = 0;

                }
                catch (Exception e)//断开客户端
                {
                    ex = (FrameIO.Interface.FrameIOException)e;
                    Assert.AreEqual(ex.ErrInfo, "客户端连接已断开!");
                    System.Threading.Thread.Sleep(100);
                    tester.A_Recv();
                    Assert.IsTrue(tester.head.Value == 1);
                    tester.head.Value = 0;
                }
            }
        }
    }
}
