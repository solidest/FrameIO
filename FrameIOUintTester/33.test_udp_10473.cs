using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOUintTester
{
    [TestClass]
    public class Test_udp_10473
    {
        [TestMethod]
        public void TCP_tcp_10473()
        {
            //调试助手持续发送数据情况下 任务管理器里中断进程 
            var tester = new test_udp_10473.testtcp10473();

            tester.InitialParameter();

            tester.InitialChannelCH_UDP(null);

            Assert.IsTrue(tester.CH_UDP.Open());
            int loop = 1;
            while (loop > 0)
            {
                loop--;
                System.Threading.Thread.Sleep(20);
                FrameIO.Interface.FrameIOException ex;
                try
                {
                    tester.A_Recv();
                    Assert.IsTrue(tester.head.Value == 0x0102);

                }
                catch (Exception e)
                {
                    ex = (FrameIO.Interface.FrameIOException)e;
                    System.Diagnostics.Debug.WriteLine(ex.ErrInfo);
                    return;
                }
            }
        }

    }
}
