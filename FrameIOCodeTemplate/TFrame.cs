using FrameIO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPoject
{
    public class TFrame : FrameBase, IFramePack
    {
        public int ByteSize => throw new NotImplementedException();

        public byte[] Pack()
        {
            throw new NotImplementedException();
        }
    }

    public class TFrameParser : IFrameUnpack
    {
        private const int _block_count = 1;
        private const int _first_block_size = 0;

        private byte[][] _buflist = new byte[_block_count][];
        private int _pos_block = 0;
        private int _next_size = 0;

        public int FirstBlockSize => _first_block_size;


        private void ResetBuffers()
        {
            _pos_block = 0;
            for (int i = 0; i < _block_count; i++)
                _buflist[i] = null;
        }

        private int GetValue(int idx_block, int idx_bit, int len)
        {

        }

        public int AppendBlock(byte[] buffer)
        {
            _buflist[_pos_block] = buffer;
            _pos_block += 1;
            return -1;
        }

        public FrameBase Unpack()
        {
            var ret = new TFrame()
            {
                ///
            };
            ResetBuffers();
            return null;
        }
    }
}
