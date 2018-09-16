using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public class foobar
    {
        [DllImport("FrameParser")]
        private extern static int add(int a, int b);

        public static int Add(int a, int b)
        {
            return add(a, b);
        }
    }
}
