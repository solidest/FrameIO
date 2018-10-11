using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrameIO.Driver
{
    public sealed class StateMutex
    {
        public byte State = 0;
        Mutex Lockor = new Mutex();

        public void SetState(System.Int32 chNo, Boolean state)
        {
            try
            {
                Lockor.WaitOne();
                State = SetBit(State, chNo + 1, state);
            }
            finally
            {
                Lockor.ReleaseMutex();
            }
        }

        private byte SetBit(Byte data, Int32 index, Boolean flag)
        {
            if (index > 8 || index < 1)
                throw new ArgumentOutOfRangeException();
            Int32 v = index < 2 ? index : (2 << (index - 2));
            return flag ? (System.Byte)(data | v) : (System.Byte)(data & ~v);

        }
    }
}
