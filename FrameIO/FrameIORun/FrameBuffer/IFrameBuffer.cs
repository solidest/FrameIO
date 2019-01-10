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
        void Write(byte[] buff, object token);
        int GetBytePos(object token);

        byte[] GetBuffer();
    }

    interface IFrameReadBuffer
    {
        bool CanRead { get;  }

        ulong ReadBits(int bitLen, object token);

        byte[] ReadBytes(int byteLen, object token);

        int GetBytePos(object token);

        byte[] GetBuffer();
    }
}
