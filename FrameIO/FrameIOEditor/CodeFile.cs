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
        static public void SaveFrameBinFile(string filename, ProjectInfo pj)
        {

            using (var fs = new FileStream(filename, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, pj);
            }
        }

        //读取数据帧文件
        static public ProjectInfo ReadFrameBinFile(string filename)
        {
            using (var fs = new FileStream(filename, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(fs) as ProjectInfo;
            }
        }
    }

    //项目信息
    public class ProjectInfo
    {
        public Dictionary<string, SysInfo> DicSys { get; set; } = new Dictionary<string, SysInfo>();
        public Dictionary<string, FrameBlockInfo> DicFrame { get; set; } = new Dictionary<string, FrameBlockInfo>();
        public Dictionary<string, EnumInfo> DicEnum { get; set; } = new Dictionary<string, EnumInfo>();
    }

    //分系统信息
    public class SysInfo
    {
        public string Name { get; set; }
        public Dictionary<string, Channel> DicChannel { get; set; } = new Dictionary<string, Channel>();
    }

    //配置通道信息
    public class Channel
    {
        public string Name { get; set; }
        public syschanneltype ChType { get; set; }
        public Dictionary<string, object> DicOption { get; set; } = new Dictionary<string, object>();
    }

    //枚举定义信息
    public class EnumInfo
    {
        public string Name { get; set; }
        public Dictionary<string, ulong> EnumItems { get; set; } = new Dictionary<string, ulong>();
    }

}
