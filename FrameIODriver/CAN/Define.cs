
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameIO.Interface;

namespace FrameIO.Driver
{
    struct AsyncWriteInfo
    {
        public IFramePack packer;
        public AsyncWriteCallback callback;
        public object AsyncState;
    }
    struct AsyncWriteInfoList
    {
        public IFramePack[] packer;
        public AsyncWriteCallback callback;
        public object AsyncState;
        public int len;
    }
    struct AsyncReadInfo
    {
        public IFrameUnpack packer;
        public AsyncReadCallback callback;
        public object AsyncState;
    }
    struct AsyncReadInfoList
    {
        public IFrameUnpack packer;
        public AsyncReadListCallback callback;
        public object AsyncState;
        public int framecount;
        public bool isloop;
    }
}
