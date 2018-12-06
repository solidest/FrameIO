
using System.IO;
using System.IO.Compression;
using System;
using FrameIO.Runtime;
using FrameIO.Interface;


namespace test_udp_receivetimeout
{
    public class FrameBase
    {
        protected static void InitialBase(){}
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
        }
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
