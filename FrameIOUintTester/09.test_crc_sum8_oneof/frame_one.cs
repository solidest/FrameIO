
using System.Collections.ObjectModel;
using FrameIO.Run;

namespace test_crc_sum8_oneof
{
    public partial class frame_one
    {

        public Parameter<uint?> SegA { get; private set;}
        public Parameter<uint?> HEAD { get; private set;}
        public Parameter<uint?> LEN { get; private set;}
        public Parameter<uint?> SegOne { get; private set;}

        public frame_one()
        {
            SegA = new Parameter<uint?>();
            HEAD = new Parameter<uint?>();
            LEN = new Parameter<uint?>();
            SegOne = new Parameter<uint?>();
        }
    }
}
