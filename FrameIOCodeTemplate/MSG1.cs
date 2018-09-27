using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;

namespace PROJECT1.Frame
{
    public class MSG1 : FrameBase, IFramePack
    {

        public long SEGMENTA { get; set; }
        public bool[] SEGMENTB { get; set; } = new bool[8];
        public double SEGMENTC { get; set; }
        public ushort SEGMENTD { get; set; }


        public int ByteSize => 19;


        byte[] IFramePack.Pack()
        {
            
        }
    }


    public class FRAME1Parser : IFrameUnpack
    {
        public int FirstBlockSize => throw new NotImplementedException();

        public int BlockCount => throw new NotImplementedException();

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
