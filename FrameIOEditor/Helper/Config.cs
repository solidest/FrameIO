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
        public static bool IsCoding { get; set; }
        public static int OutHeight { get; set; }

        public static void SaveConfig(IOutText it)
        {
            try
            {
                var cfg = new JObject();
                cfg.Add(nameof(LastFile), LastFile);
                cfg.Add(nameof(IsCoding), IsCoding);
                cfg.Add(nameof(OutHeight), OutHeight);
                
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
                    IsCoding = cfg?[nameof(IsCoding)]?.Value<bool>() ?? false;
                    OutHeight = cfg?[nameof(OutHeight)]?.Value<int>() ?? 160;
                }
            }
            catch (Exception ex)
            {
                it.OutText(ex.ToString(), false);
            }
        }
    }
}
