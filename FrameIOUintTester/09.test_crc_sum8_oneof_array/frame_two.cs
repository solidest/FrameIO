
using System.Collections.ObjectModel;
using FrameIO.Run;

namespace test_crc_sum8_oneof_array
{
    public partial class frame_two
    {

        public Parameter<uint?> SegA { get; private set;}
        public ObservableCollection<Parameter<uint?>> HEAD { get; private set; }
        public ObservableCollection<Parameter<uint?>> LEN { get; private set; }
        public Parameter<uint?> SegTwo { get; private set;}

        public frame_two()
        {
            SegA = new Parameter<uint?>();
            HEAD = new ObservableCollection<Parameter<uint?>>(); for (int i = 0; i < 2; i++) HEAD.Add(new Parameter<uint?>());
            LEN = new ObservableCollection<Parameter<uint?>>(); for (int i = 0; i < 3; i++) LEN.Add(new Parameter<uint?>());
            SegTwo = new Parameter<uint?>();
        }
    }
}
