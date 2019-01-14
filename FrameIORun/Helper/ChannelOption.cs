using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Run
{
    //通道参数
    public class ChannelOption
    {
        internal Dictionary<string, object> Options { get; private set; }

        internal ChannelOption(Dictionary<string, object> ops)
        {
            Options = ops;
        }


        public ChannelOption()
        {
            Options = new Dictionary<string, object>();
        }


        public void SetOption(string optionname, object optionvalue)
        {
            if (Options.Keys.Contains(optionname))
                Options[optionname] = optionvalue;
            else
                Options.Add(optionname, optionvalue);
        }

        public object GetOption(string optionname)
        {
            return Options[optionname];
        }

        public bool Contains(string optionname)
        {
            return Options.Keys.Contains(optionname);
        }

    }
}
