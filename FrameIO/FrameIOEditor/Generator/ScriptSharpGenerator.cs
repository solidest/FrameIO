using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //c# 代码生成驱动
    public class SharpScriptGenerator : ScriptGenerator
    {

        public SharpScriptGenerator(IOProject pj, IOutText tout) : base(pj, tout)
        {

        }

        protected override string Token => "cs";
        protected override string DefaultExtension { get => "cs";}

        protected override IList<string> ConvertFramesCode(IList<string> base64List)
        {
            return base64List.Select(p => "\"" + p + "\",").ToList();
        }

        protected override void CreateSharedFile()
        {
            //HACK
        }

        protected override StringBuilder GetInnerSubsysFileContent(InnerSubsys innersys)
        {
            //HACK
            var code = new StringBuilder();

            return code;
        }

        protected override StringBuilder GetSubsysFileContent(Subsys subsys)
        {
            //HACK
            var code = new StringBuilder();

            return code;
        }
    }
}
