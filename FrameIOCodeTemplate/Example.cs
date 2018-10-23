using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using FrameIO.Interface;
using System.IO;
using System.IO.Compression;
using System;
using FrameIO.Runtime;

namespace PROJECT1
{

    public class FrameBase
    {

        static FrameBase()
        {
            var config = string.Concat(
                        "H4sIAAAAAAAEAH1PMQ7CMAx0k0AppAg2KpBAMMDMUjZSCVS+wG8ysPMlNngO",
                "T+ASG9ouVHLOPtu98476NCJN3a90jO+jEFJfGQ93951MEQmij9CSJ5IrvElE",
                "+otGsNdycNur2Del7vAWkxPgOFYLqlTYTehkTGTOiqer6FBRBg8DvOHGIRVy",
                "a4Fc/GvSD1C1dtrr5r8X1GF0EPrQyEXX0pRmwKehnoPUOV4eMI34Au8N3xJ0",
                "T56Vctmz0J7//K/Ac22huQy+Pftq+sxb5OtWP5fa0oa2wDpzmc++ewVdUEMe",
                "c9z/AJr2JbroAQAA");


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

    }

    public class TFrameGettor : FrameBase
    {
        private ISegmentGettor _gettor;

        public static IFrameUnpack Unpacker
        {
            get => FrameIOFactory.GetFrameUnpacker(2);
        }
        public TFrameGettor(ISegmentGettor gettor)
        {
            _gettor = gettor;
        }

        public bool? SegmentA { get => _gettor.GetBool(1); }

        public int?[] SegmentD { get => _gettor.GetIntArray(2); }

        //引用类
        private TFrameGettor _SegmentB;
        public TFrameGettor SegmentB { get { if (_SegmentB == null) _SegmentB = new TFrameGettor(_gettor.GetSubFrame(1)); return _SegmentB; } }

        //内置类
        private TInnerSegmentC _SegmnetC;
        public TInnerSegmentC SegmnetC
        {
            get
            {
                if (_SegmnetC == null) _SegmnetC = new TInnerSegmentC(this);
                return _SegmnetC;
            }
        }


        public class TInnerSegmentC
        {
            TFrameGettor _parent;
            public TInnerSegmentC(TFrameGettor parent)
            {
                _parent = parent;
            }
            public bool? InnerSegmengAA { get=>_parent._gettor.GetBool(1); }
        }

    }

    public class TFrameSettor : FrameBase
    {
        private ISegmentSettor _settor;

        public IFramePack GetPacker()
        {
            return _settor.GetPack();
        }

        public TFrameSettor()
        {
            _settor = FrameIOFactory.GetFrameSettor(1);
        }

        public TFrameSettor(ISegmentSettor packer)
        {
            _settor = packer;
        }

        

        public bool? SegmentA { set => _settor.SetSegmentValue(2, value); }

        //引用类
        private TFrameSettor _SegmentB;
        public TFrameSettor SegmentB
        {
            get
            {
                if(_SegmentB==null) _SegmentB = new TFrameSettor(_settor.GetSubFrame(3));
                return _SegmentB;
            }
        }

        //内置类
        private TInnerSegmentC _SegmnetC;
        public TInnerSegmentC SegmnetC
        {
            get
            {
                if (_SegmnetC == null) _SegmnetC = new TInnerSegmentC(this);
                return _SegmnetC;
            }
        }

        public class TInnerSegmentC
        {
            TFrameSettor _parent;
            public TInnerSegmentC(TFrameSettor parent)
            {
                _parent = parent;
            }
            public bool? InnerSegmengAA{ set=> _parent._settor.SetSegmentValue(2, value); }
        }
    }


}
