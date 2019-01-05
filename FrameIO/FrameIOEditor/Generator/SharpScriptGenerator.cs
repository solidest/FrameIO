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
            Token = "sharp";
            DefaultExtension = "cs";
        }

        protected override string Token { get;  set; }
        protected override string FrameFileName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string DefaultExtension { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override void CreateShareFile()
        {
            throw new NotImplementedException();
        }


        protected override StringBuilder GetEnumFileContent(Enumdef em)
        {
            throw new NotImplementedException();
        }

        protected override StringBuilder GetFramesFileContent(IList<string> framesjson)
        {
            throw new NotImplementedException();
        }

        protected override StringBuilder GetInnerSubsysFileContent(InnerSubsys innersys)
        {
            throw new NotImplementedException();
        }

        protected override StringBuilder GetSubsysFileContent(Subsys subsys)
        {
            throw new NotImplementedException();
        }
    }
}
