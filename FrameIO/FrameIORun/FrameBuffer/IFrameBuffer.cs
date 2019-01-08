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
        void Append(byte[] cach);

        bool CanRead { get;  }
        ulong Read(int bitLen, object token);

        int GetBytePos(object token);

        void SaveRepeated(object token, int index);
        int LoadRepeated(object token);

        byte[] GetBuffer();
    }
}
