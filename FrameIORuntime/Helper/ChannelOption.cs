using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Runtime
{
    //通道参数
    public class ChannelOption
    {
        private Dictionary<string, object> _options;

        internal Dictionary<string, object> GetOptions ()
        {
            var ret = _options;
            _options = null;
            return ret;
        }

        public void SetOption(string optionname, object optionvalue)
        {
            if (_options == null) _options = new Dictionary<string, object>();
            if (_options.Keys.Contains(optionname))
                _options[optionname] = optionvalue;
            else
                _options.Add(optionname, optionvalue);
        }

        public bool Contains(string optionname)
        {
            if (_options == null) return false;
            return _options.Keys.Contains(optionname);
        }

    }
}
