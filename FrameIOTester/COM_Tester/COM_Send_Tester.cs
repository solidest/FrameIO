using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOTester
{
    [TestClass]
    public class COM_Send_Tester
    {

        [TestMethod]
        public void Com_Send_One_Byte()
        {
            byte msg = 0x01;

            var settor = FrameIO.Runtime.FrameIOFactory.GetFramePack("MSG1");
            
        }
    }
}
