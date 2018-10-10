using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    public interface IFrameRun
    {
        int ByteSizeOf(string segname);
        double GetIdValue(string idfullname);

    }

    public interface IUnpackFrameRun:IFrameRun
    {

        void AddUnpackSeg(string idfullname, SegUnpack seg);

        SegUnpack FindUnpackSegRun(string fullname);
    }

}
