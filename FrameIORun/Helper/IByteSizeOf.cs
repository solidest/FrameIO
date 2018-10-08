using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    public interface ICalcuValue
    {
        int ByteSizeOf(string segname);
        double GetIdValue(string idfullname);

        void AddIdSeg(string idfullname, SegRun seg);
    }
}
