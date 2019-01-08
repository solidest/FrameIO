
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //二进制缓冲流
    internal class FrameBuffer : IFrameBuffer
    {
        private MemoryStream _cach;
        private Slice _odd;
        private Dictionary<object, int> _pos;

        public FrameBuffer()
        {
            _cach = new MemoryStream();
            _odd = Slice.Empty;
            _pos = new Dictionary<object, int>();
        }

        #region --For Pack-- 

        //写入到缓冲
        public void Write(Slice s, object token)
        {
            _odd = _odd.WriteAppend(_cach, s);
            if(token!=null) _pos.Add(token, GetBytePos());
        }

        #endregion

        #region --For Unpack--

        //添加

        #endregion

        #region --Helper--

        //当前字节位置
        public int GetBytePos()
        {
            if(!_odd.IsEmpty) throw new Exception("runtime 数据帧字段未能整字节对齐");
            return (int)_cach.Position;
        }

        public int GetPos(object token)
        {
            return _pos[token];
        }

        public byte[] GetBuffer()
        {
            return _cach.GetBuffer();
        }

        #endregion


    }
}
