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
    public class CAN_Test_Send
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
        public void Test_CAN_SendOnePacket()
        {
            //data:[2 1 0 5 6 0 0 0 0 0 0]

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
            dic.Add("writetimeout", 3000);

            chcan.InitConfig(dic);
            chcan.Open();

            var ret=chcan.WriteFrame(pack);

            if (ret != 0)
                chcan.Close();

        }

        [TestMethod]
        public void Test_CAN_SendTwoPacket()
        {
            //data:[2 1 0 5 6 0 0 0 0 0 0] [2 1 0 6 7 0 0 0 0 0 0]

            Initialize();
            var settors = new FrameIO.Interface.ISegmentSettor[2];
            for (int i = 0; i<2; i++)
            {
                settors[i] = FrameIOFactory.GetFrameSettor(1);

                settors[i].SetSegmentValue(2, 2);   //len
                settors[i].SetSegmentValue(4, false);    //frametype
                settors[i].SetSegmentValue(5, false);   //frameformat
                settors[i].SetSegmentValue(6, 1);     //ID

                settors[i].SetSegmentValue(7, 0x05+i);    //data0
                settors[i].SetSegmentValue(8, 0x06+i);    //data0
            }


            var packs = new FrameIO.Interface.IFramePack[2];
            for (int i = 0; i < 2; i++)
                packs[i] = settors[i].GetPack();
            var chcan = new FrameIO.Driver.YH_CAN_Impl();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("vender", "yh");
            dic.Add("channelind", "2");
            dic.Add("baudrate", "125Kbps");
            dic.Add("writetimeout", 3000);

            chcan.InitConfig(dic);
            chcan.Open();
            int ret=chcan.WriteFrameList(packs, settors.Length);

            if (ret != 0)
                chcan.Close();

        }
        
    }
}
