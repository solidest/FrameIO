
using System.IO;
using System.IO.Compression;
using System;
using FrameIO.Runtime;
using FrameIO.Interface;


namespace test_udp_receivetimeout
{
    public class FrameBase
    {

        static FrameBase()
        {
            var config = string.Concat(
                "H4sIAAAAAAAEAGNjYARDQoAHqIYVSDs2QPjoNC9UHgD2fvJHUAAAAA==");

            using (var compressStream = new MemoryStream(Convert.FromBase64String(config)))
            {
                using (var zipStream = new GZipStream(compressStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        FrameIO.Runtime.FrameIOFactory.Initial(resultStream.ToArray());
                    }
                }
            }

            var symbols = string.Concat(
                "H4sIAAAAAAAEAOVUzUrDQBDemDRNqdVXyMFjCK32xx56kDZYUUox2ksJdBtG",
                "XbpJdLMR4mP4iCI+gIiIFHS3bKGnnhSUzh5md77ZYWa+2UUaQuhLiNRSDLl5",
                "0vw85RC53YRSCDlJ4tQ9hhgYCd0eWRgwyyf747FyvDyJea3p2FEaJoySqWOP",
                "gKXCrVN3q3I5djejPGPQiSHjDFPHHmZTSsJTyC+SGcSdaauFG2GjWWsf1KF6",
                "2A6cZXCfMxJf/2zwwBB1FlUcq5tEt5gBs/o4vfHJA1TEzRGmGQwxYSnSkW49",
                "rmuK0t5dhinh+TLepParHQqsl3VJrdbwr7kaB6Zgq7QlKZMbXQ6qPP1FTuQr",
                "KsoMkXxKMnH9eSNYMubiH9mMUuXs6TPIC/eyHKTtCKILC8o/RQ/mi//UlKNa",
                "vGI4Av9c+1D2LWSWhH1b2d2+d9TT3hWoI7MswPISPPMG2pvCDGRWVjFv0NNe",
                "FVZA5q7ALIXtlb8B+CoeDdcFAAA=");
            using (var compressStream = new MemoryStream(Convert.FromBase64String(symbols)))
            {
                using (var zipStream = new GZipStream(compressStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        FrameIO.Runtime.FrameIOFactory.InitializeSymbols(resultStream.ToArray());
                    }
                }
            }
        }

        protected static void InitialBase(){}
    }

        
    public class frameSRGettor : FrameBase
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack Unpacker
        {
            get { InitialBase(); return FrameIOFactory.GetFrameUnpacker(1); }
        }

        public frameSRGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

        public uint? HEAD { get => _gettor.GetUInt(2); }
        public uint? LEN { get => _gettor.GetUInt(3); }
        public uint? END { get => _gettor.GetUInt(4); }
    }
    public class frameSRSettor : FrameBase
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public frameSRSettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(1);
        }

        public frameSRSettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        public uint? HEAD { set => _settor.SetSegmentValue(2, value); }
        public uint? LEN { set => _settor.SetSegmentValue(3, value); }
        public uint? END { set => _settor.SetSegmentValue(4, value); }     
    }

}
