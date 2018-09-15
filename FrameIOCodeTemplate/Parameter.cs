using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.CodeTemplate
{
    public class Parameter<T>
    {
        private T _t;

        public string Name { get; set; }
        public T TValue { get => _t; set => _t = value; }
    }
}
