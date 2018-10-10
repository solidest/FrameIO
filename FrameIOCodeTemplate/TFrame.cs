using FrameIO.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPoject
{
    public class TFrame : IFrameData, IFramePack
    {
        public int ByteSize => throw new NotImplementedException();

        public byte[] Pack()
        {
            throw new NotImplementedException();
        }
    }

    public class /*%first_block_size%*/TFrameParser : IFrameUnpack
    {
        //数据帧解析指令
        static ulong[] CODE = new ulong[] 
        {
            0x0000000000000000,
            0x42F0E1EBA9EA3693
        };

        //初始化
        public TFrameParser()
        {
            _getsize_code_pos = /*%first_getsize_pos%*/-1;
        }

        //首块内存大小
        public int FirstBlockSize => /*%first_block_size%*/0;

        private MemoryStream _cach = new MemoryStream();

        //下一内存块大小
        private int _nextSize = -1;

        //取内存大小的指令
        private int _getsize_code_pos = -1;

        //运行指令返回下一条指令的位置
        static private int RunCode(int code_pos)
        {
            return 0;
        }


        //计算下一块内存块大小
        private int GetNextBlockSize()
        {
            
            return 0;
        }

        //添加内存块 返回下一块内存所需大小
        public int AppendBlock(byte[] buffer)
        {
            if (_nextSize <= 0 || buffer.Length!=_nextSize)
                throw new Exception("AppendBlock的错误调用");
            _cach.Write(buffer, 0, buffer.Length);
            _nextSize = GetNextBlockSize();
            return _nextSize;
        }


        //解包全部报文
        public IFrameData Unpack()
        {
            throw new NotImplementedException();
        }
    }
}
