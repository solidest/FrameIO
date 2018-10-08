using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;

namespace FrameIO.Run
{
    public class UnpackFactory:IFrameUnpack
    {
        public int FirstBlockSize => throw new NotImplementedException();

        public int AppendBlock(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public FrameBase Unpack()
        {
            throw new NotImplementedException();
        }
    }
}
