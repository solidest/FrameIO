using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    class SegmentTextArrayRun : SegmentTextRun
    {
        private ushort _repeated_idx;
        private int _repeated_const = -1;

        public SegmentTextArrayRun(ulong token, IRunInitial ir) : base(token, ir)
        {
            const byte pos_repeated = 32;

            _repeated_idx = GetTokenUShort(token, pos_repeated);
            if (_repeated_idx == 0) throw new Exception("runtime 空数组引用");
            if (ir.IsConst(_repeated_idx)) _repeated_const = (int)ir.GetConstValue(_repeated_idx);
        }
    }
}
