using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public interface IOutText
    {
        void OutText(string info, bool clear);

        string GetMainOutPath();
    }
}
