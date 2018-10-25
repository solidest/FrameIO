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
    public class CAN_Test_Receive
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
        public void Test_CAN_ReceiveOnePacket()
        {
            Initialize();
            var settor = FrameIOFactory.GetFrameSettor(1);
            settor.SetSegmentValue(2, 2);   //len
            settor.SetSegmentValue(4, false);    //frametype
            settor.SetSegmentValue(5, false);   //frameformat
            settor.SetSegmentValue(6, 1);     //ID

            settor.SetSegmentValue(7, 0x05);    //data0
            settor.SetSegmentValue(8, 0x06);    //data0


            var chcan = new FrameIO.Driver.YH_CAN_Impl();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("vender", "yh");
            dic.Add("channelind", "2");
            dic.Add("baudrate", "125Kbps");
            dic.Add("writetimeout", 3000);

            chcan.InitConfig(dic);
            chcan.Open();


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
        public void Test_CAN_ReceiveTwoPacket()
        {
            Initialize();
            var settors = new FrameIO.Interface.ISegmentSettor[2];
            for (int i = 0; i < 2; i++)
            {
                settors[i] = FrameIOFactory.GetFrameSettor(1);

                settors[i].SetSegmentValue(2, 2);   //len
                settors[i].SetSegmentValue(4, false);    //frametype
                settors[i].SetSegmentValue(5, false);   //frameformat
                settors[i].SetSegmentValue(6, 1);     //ID

                settors[i].SetSegmentValue(7, 0x05);    //data0
                settors[i].SetSegmentValue(8, 0x06);    //data0
            }



            var chcan = new FrameIO.Driver.YH_CAN_Impl();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("vender", "yh");
            dic.Add("channelind", "2");
            dic.Add("baudrate", "125Kbps");
            dic.Add("writetimeout", 3000);

            chcan.InitConfig(dic);
            chcan.Open();

            //read
            var unpacks = FrameIOFactory.GetFrameUnpacker(1);
            var gettors = chcan.ReadFrameList(unpacks, 2);


            for (int i = 0; i < gettors.Length; i++)
            {
                Assert.IsTrue(gettors[i].GetByte(2) == 2);
                Assert.IsTrue(gettors[i].GetBool(4) == false);
                Assert.IsTrue(gettors[i].GetBool(5) == false);
                Assert.IsTrue(gettors[i].GetUShort(6) == 1);

                Assert.IsTrue(gettors[i].GetByte(7) == 0x06);
                Assert.IsTrue(gettors[i].GetByte(8) == 0x07);
            }

            chcan.Close();

        }
    }
}
