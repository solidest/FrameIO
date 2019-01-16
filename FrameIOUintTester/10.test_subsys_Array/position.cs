
using System.Collections.ObjectModel;
using FrameIO.Run;

namespace test_subsys_Array
{
    public partial class position
    {

        public Parameter<float?> jingdu { get; private set;}
        public Parameter<double?> weidu { get; private set;}

        public position()
        {
            jingdu = new Parameter<float?>();
            weidu = new Parameter<double?>();
        }
    }
}
