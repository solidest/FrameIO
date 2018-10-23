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
        public void Test_CAN_SendOnePacket_ReceiveOnePacket()
        {
            Initialize();
            var settor = FrameIOFactory.GetFrameSettor(1);
            settor.SetSegmentValue(2, 2);   //len
            settor.SetSegmentValue(4, false);    //frametype
            settor.SetSegmentValue(5, false);   //frameformat
            settor.SetSegmentValue(6, 1);     //ID

            settor.SetSegmentValue(7, 0x05);    //data0
            settor.SetSegmentValue(8, 0x06);    //data0


            var pack = settor.GetPack();

            var chcan = new FrameIO.Driver.YH_CAN_Impl();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("vender", "yh");
            dic.Add("channelind", "2");
            dic.Add("baudrate", "125Kbps");
            dic.Add("writetimeout", 30000);

            chcan.InitConfig(dic);
            chcan.Open();

            chcan.WriteFrame(pack);

            var unpacker = FrameIOFactory.GetFrameUnpacker(1);
            var gettor = chcan.ReadFrame(unpacker);

            Assert.IsTrue(gettor.GetByte(2) == 2);
            Assert.IsTrue(gettor.GetBool(4) == false);
            Assert.IsTrue(gettor.GetBool(5) == false);
            Assert.IsTrue(gettor.GetUShort(6) == 1);

            Assert.IsTrue(gettor.GetByte(7) == 0x05);
            Assert.IsTrue(gettor.GetByte(14) == 0x06);

        }

        [TestMethod]

    }
}
