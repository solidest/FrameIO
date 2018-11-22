
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace FrameIO.Main
{
    public class FrameCompileJson
    {
        static public string ToJson(IOProject pj)
        {
            var sett = new JsonSerializerSettings();
            sett.TypeNameHandling = TypeNameHandling.Auto;
            return JsonConvert.SerializeObject(pj, Formatting.Indented, sett);
        }
    }
}
