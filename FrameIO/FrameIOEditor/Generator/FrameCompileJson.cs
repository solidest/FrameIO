
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace FrameIO.Main
{
    public class FrameCompileJson
    {
        static public string ToJson(IOProject pj)
        {
            //var js = new JsonSerializer();
            //var sw = new StreamWriter("_temp.txt");
            //js.Serialize(sw, pj, pj.GetType());
            //sw.Close();
            //var ret =  File.ReadAllText("_temp.txt");
            //File.Delete("_temp.txt");
            //return ret;
            return JsonConvert.SerializeObject(pj, Formatting.Indented);
        }
    }
}
