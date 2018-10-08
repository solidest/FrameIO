using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //代码文件操作类
    public class CodeFile
    {

        //保存二进制数据帧文件
        static public void SaveFrameBinFile(string filename, Dictionary<string, FrameBlockInfo> frlist)
        {

            using (var fs = new FileStream(filename, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, frlist);
            }
        }


        //读取数据帧文件
        static public Dictionary<string, FrameBlockInfo> ReadFrameBinFile(string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(fs) as Dictionary<string, FrameBlockInfo>;
            }
        }
    }
}
