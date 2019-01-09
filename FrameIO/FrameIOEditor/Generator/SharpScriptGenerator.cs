﻿using System;
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
        protected override string FramesFileName => "Frames";
        protected override string DefaultExtension { get => "cs";}

        protected override void CreateShareFile()
        {
            return;
        }


        protected override StringBuilder GetEnumFileContent(Enumdef em)
        {
            var ret = new StringBuilder();

            return ret;
        }

        protected override StringBuilder GetFramesFileContent(IList<string> framesjson)
        {
            var ret = new StringBuilder();

            return ret;
        }

        protected override StringBuilder GetInnerSubsysFileContent(InnerSubsys innersys)
        {
            var ret = new StringBuilder();

            return ret;
        }

        protected override StringBuilder GetSubsysFileContent(Subsys subsys)
        {
            var ret = new StringBuilder();

            return ret;
        }
    }
}