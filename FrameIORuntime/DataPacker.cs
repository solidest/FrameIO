using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;
namespace FrameIO.Runtime
{
    class DataPacker : IFramePack
    {
        private byte[] _data;

        public DataPacker(byte[] data)
        {
            _data = data;
        }
        public byte[] Pack()
        {
            return _data;
        }
    }
}
