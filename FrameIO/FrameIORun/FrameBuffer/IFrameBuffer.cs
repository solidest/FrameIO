using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    interface IFrameBuffer
    {

    }

    interface IFrameWriteBuffer
    {
        void Write(ulong rawValue, int bitLen, object token);

        int GetPos(object token);

        byte[] GetBuffer();
    }

    interface IFrameReadBuffer
    {
        ulong Read(int bitLen, object token);

        int GetPos(object token);

        byte[] GetBuffer();
    }
}
