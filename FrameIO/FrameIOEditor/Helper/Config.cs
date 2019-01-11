using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    //配置文件
    public static class FioConfig
    {

        public static string LastFile { get; set; }

        public static void SaveConfig(IOutText it)
        {
            try
            {
                var cfg = new JObject();
                cfg.Add(nameof(LastFile), LastFile);
                
                var cf = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\fio.config";
                if(File.Exists(cf)) File.Delete(cf);
                File.WriteAllText(cf, cfg.ToString());
            }
            catch (Exception ex)
            {
                it.OutText(ex.ToString(), false);
            }

        }

        public static void LoadConfig(IOutText it)
        {
            try
            {
                var cf = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\fio.config";
                if (File.Exists(cf))
                {
                    var cfg = JObject.Parse(File.ReadAllText(cf));
                    LastFile = cfg?[nameof(LastFile)]?.Value<string>();
                }
            }
            catch (Exception ex)
            {
                it.OutText(ex.ToString(), false);
            }
        }
    }
}
