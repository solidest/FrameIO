
using System.Collections.ObjectModel;
using FrameIO.Run;

namespace test_crc_sum8_oneof_array
{
    public partial class frame_one
    {

        public Parameter<uint?> SegA { get; private set;}
        public ObservableCollection<Parameter<uint?>> HEAD { get; private set; }
        public ObservableCollection<Parameter<uint?>> LEN { get; private set; }
        public Parameter<uint?> SegOne { get; private set;}

        public frame_one()
        {
            SegA = new Parameter<uint?>();
            HEAD = new ObservableCollection<Parameter<uint?>>(); for (int i = 0; i < 2; i++) HEAD.Add(new Parameter<uint?>());
            LEN = new ObservableCollection<Parameter<uint?>>(); for (int i = 0; i < 3; i++) LEN.Add(new Parameter<uint?>());
            SegOne = new Parameter<uint?>();
        }
    }
}
