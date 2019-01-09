using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{

    interface IFrameWriteBuffer
    {
        void Write(ulong rawValue, int bitLen, object token);

        int GetBytePos(object token);

        byte[] GetBuffer();
    }

    interface IFrameReadBuffer
    {

        bool CanRead { get;  }
        ulong Read(int bitLen, object token);

        int GetBytePos(object token);

        object StopPosition { get; set; }

        byte[] GetBuffer();
    }
}
