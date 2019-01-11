using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    public static class RunHelper
    {

        static public int GetMin(int n1, int n2)
        {
            return n1 > n2 ? n2 : n1;
        }

        static public ObservableCollection<Parameter<bool?>> new_arr_bool(int count)
        {
            var ret = new ObservableCollection<Parameter<bool?>>();
            for (int i = 0; i < count; i++)
            {
                ret.Add(new Parameter<bool?>());
            }
            return ret;
        }

    }
}
