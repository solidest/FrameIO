using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrameIO.Run;

namespace FrameIORunNetTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void A_FioNetRunner_Initial()
        {
            var ch = FioNetRunner.GetChannel(null);
        }

        [TestMethod]
        public void A_FioNetRunner_SendOneByte()
        {
            
        }

    }
}
