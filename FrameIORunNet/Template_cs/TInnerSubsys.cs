

using System.Collections.ObjectModel;
using FrameIO.Run;

namespace PROJECT1
{

    public class FeedbackData
    {
        public Parameter<bool?> name1 { get; private set; }
        public Parameter<bool?> name5 { get; private set; }
        public ObservableCollection<Parameter<bool?>> arrname1 { get; private set; }
        public ObservableCollection<Parameter<bool?>> arrname2 { get; private set; }
        public FeedbackData name2 { get; private set; }
        public ObservableCollection<FeedbackData> name3 { get; private set; }
        public FeedbackData()
        {
            name1 = new Parameter<bool?>();
            arrname1 = new ObservableCollection<Parameter<bool?>>();
            arrname2 = new ObservableCollection<Parameter<bool?>>(); for (int i = 0; i < 100; i++) arrname2.Add(new Parameter<bool?>());
            name2 = new FeedbackData();
            name3 = new ObservableCollection<FeedbackData>(); for(int i=0; i<100; i++) name3.Add(new FeedbackData());

        }
    }

}
