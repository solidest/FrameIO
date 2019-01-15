using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    interface IFrameBuffer
    {

        void Write(Slice s, object token);

        int GetPos(object token);

        byte[] GetBuffer();
    }
}
