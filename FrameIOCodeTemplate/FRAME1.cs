﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;

namespace PROJECT1.Frame
{
    public class FRAME1 : FrameBase, IFramePack, IFrameUnpack
    {
        public ushort SEGMENTA { get; set; }
        public byte[] SEGMENTB { get; set; } = new byte[4];
        public double SEGMENTC { get; set; }
        public byte[] Pack()
        {
            return null;
        }
        public FRAME1 Unpack()
        {
            throw new NotImplementedException();
        }


        public int FrameCount => throw new NotImplementedException();

        public int FirstBlockSize => throw new NotImplementedException();

        public int BlockCount => throw new NotImplementedException();

        public int AppendBlock(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public byte[] GetAllPack()
        {
            throw new NotImplementedException();
        }

        public byte[] NextPack()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        FrameBase IFrameUnpack.Unpack()
        {
            throw new NotImplementedException();
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

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public FrameBase Unpack()
        {
            throw new NotImplementedException();
        }
    }
}