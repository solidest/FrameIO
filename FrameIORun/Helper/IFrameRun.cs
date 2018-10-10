using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    public interface IFrameRun
    {
        int ByteSizeOf(string segfullname);
        double GetSegValue(string segfullname);

    }

    public interface IUnpackFrameRun:IFrameRun
    {

        void AddUnpackSeg(string idfullname, SegRunUnpack seg);

        SegRunUnpack FindUnpackSegRun(string fullname);
    }

    public interface IPackFrameRun : IFrameRun
    {
        SegRunPack FindPackSegRun(string fullname);
    }

}
