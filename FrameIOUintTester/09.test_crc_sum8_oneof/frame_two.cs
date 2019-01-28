
using System.Collections.ObjectModel;
using FrameIO.Run;

namespace test_crc_sum8_oneof
{
    public partial class frame_two
    {

        public Parameter<uint?> SegA { get; private set;}
        public Parameter<uint?> HEAD { get; private set;}
        public Parameter<uint?> LEN { get; private set;}
        public Parameter<uint?> SegTwo { get; private set;}

        public frame_two()
        {
            SegA = new Parameter<uint?>();
            HEAD = new Parameter<uint?>();
            LEN = new Parameter<uint?>();
            SegTwo = new Parameter<uint?>();
        }
    }
}
