using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrameIO.Interface;
using FrameIO.Main;

namespace FrameIO.Run
{
    //解包类创建工厂
    public class UnpackFactory
    {
        static private Dictionary<string, FrameBlockInfo> _frlist;

        //初始化工厂
        public static void InitialFactory(string binfile)
        {
            _frlist = CodeFile.ReadFrameBinFile(binfile);
        }

        //获取数据帧的解析器
        public static IFrameUnpack GetFrameUnpack(string framname)
        {
            return new FrameUnpack(_frlist[framname].RootSegBlockGroupInfo);
        }

    }
}
