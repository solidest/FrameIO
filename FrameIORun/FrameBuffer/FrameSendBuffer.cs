
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //二进制缓冲流
    internal class FrameSendBuffer : IFrameWriteBuffer
    {
        private MemoryStream _cach;
        private SliceWriter _odd;
        private Dictionary<object, int> _pos;
        private object _lasttoken;

        public FrameSendBuffer()
        {
            _cach = new MemoryStream();
            _odd = SliceWriter.Empty;
            _pos = new Dictionary<object, int>();
        }

        #region --Writer-- 

        //写入到缓冲
        public void Write(ulong rawValue, int bitLen, object token)
        {
            if (token != _lasttoken)
            {
                _pos.Add(token, (int)_cach.Position * 8 + _odd.BitLen);
                _lasttoken = token;
            }
            _odd = _odd.WriteAppend(_cach, SliceWriter.GetSlice(rawValue, bitLen));
        }

        public void Write(byte[] buff, object token)
        {
            if(!_odd.IsEmpty) throw new Exception("runtime 数据帧字段未能整字节对齐");
            _pos.Add(token, (int)_cach.Position * 8 + _odd.BitLen);
            _cach.Write(buff, 0, buff.Length);
        }

        //结束写入
        public byte[] Flush()
        {
            if (!_odd.IsEmpty) throw new Exception("runtime 数据帧字段未能整字节对齐");
            _cach.Close();
            return _cach.ToArray();
        }

        #endregion

        #region --Helper--


        public int GetBytePos(object token)
        {
            var bitpos =  _pos[token];
            if(bitpos%8 != 0) throw new Exception("runtime 数据帧字段未能整字节对齐");
            return bitpos / 8;
        }

        public byte[] GetBuffer()
        {
            return _cach.GetBuffer();
        }

        


        #endregion


    }
}
