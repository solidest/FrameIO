using FrameIO.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIOTester
{
    [TestClass]
    public class CANTester
    {
        #region --Initialize--

        public static void Initialize()
        {
            var config = string.Concat(
                "H4sIAAAAAAAEABNgYARDQoAHqIYfSDsKQPiOHFCaBY12gNIKlNG8UPsAtuqg",
                "W6AAAAA=");


            using (var compressStream = new MemoryStream(Convert.FromBase64String(config)))
            {
                using (var zipStream = new GZipStream(compressStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        FrameIO.Runtime.FrameIOFactory.Initialize(resultStream.ToArray());
                    }
                }
            }
        }

        #endregion


        [TestMethod]
        public void TestCAN()
        {
            Initialize();
            var settor = FrameIOFactory.GetFrameSettor(1);
            settor.SetSegmentValue(2, 8);   //len
            settor.SetSegmentValue(3, 1);   //h1
            settor.SetSegmentValue(4, true);    //frametype
            settor.SetSegmentValue(5, false);   //frameformat
            settor.SetSegmentValue(6, 100);     //ID

            settor.SetSegmentValue(7, 255);    //data0
            settor.SetSegmentValue(14, 233);   //data7


            var pack = settor.GetPack();
            //发送pack 

            var data = pack.Pack();


            var unpacker = FrameIOFactory.GetFrameUnpacker(1);


            unpacker.AppendBlock(data);
            var gettor = unpacker.Unpack();

            Assert.IsTrue(gettor.GetByte(2) == 8);
            Assert.IsTrue(gettor.GetByte(3) == 1);
            Assert.IsTrue(gettor.GetBool(4) == true);
            Assert.IsTrue(gettor.GetBool(5) == false);
            Assert.IsTrue(gettor.GetUShort(6) == 100);

            Assert.IsTrue(gettor.GetByte(7) == 255);
            Assert.IsTrue(gettor.GetByte(14) == 233);


        }


    }
}
