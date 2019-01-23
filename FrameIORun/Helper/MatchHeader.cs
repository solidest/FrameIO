using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    internal class MatchHeader
    {
        private ulong _matchValue;
        private int _matchLen;
        private byte[] _header;
        private int _pos;

        public bool NeedMatch { get => _matchLen > 0; }
        public bool IsMatch { get => BitConverter.ToUInt64(_header, 0) == _matchValue; }
        public byte[] Header { get => GetHeader(); }

        public MatchHeader(ulong matchValue, int matchLen)
        {
            _matchValue = matchValue;
            _matchLen = matchLen;
            _header = new byte[8];
            _pos = 0;
        }

        public bool AppendAndMatch(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                if (_pos == 8) return DoMatch();
                _header[_pos++] = buffer[i];
            }
            return DoMatch();
        }

        //执行匹配，如不匹配推进1个字节
        private bool DoMatch()
        {
            if (IsMatch) return true;
            var newHeader = new byte[8];
            for (int i = 0; i < _pos; i++)
            {
                newHeader[i] = _header[i + 1];
            }
            _header = newHeader;
            _pos--;
            return false;
        }
        
        private byte[] GetHeader()
        {
            var ret = new byte[_matchLen];
            for (int i = 0; i < _matchLen; i++)
                ret[i] = _header[i];
            return ret;
        }

    }
}
