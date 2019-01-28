using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    interface IFrameBuffer
    {
        int GetBytePos(object token);

        byte[] GetBuffer();
    }

    interface IFrameWriteBuffer : IFrameBuffer
    {
        void Write(ulong rawValue, int bitLen, object token);
        void Write(byte[] buff, object token);

    }

    interface IFrameReadBuffer : IFrameBuffer
    {
        bool CanRead { get;  }

        ulong ReadBits(int bitLen, object token);

        byte[] ReadBytes(int byteLen, object token);

    }
}
