
using System.IO;
using System.IO.Compression;
using System;

namespace main
{
    public class FrameIORuntime
    {

        public static void Initialize()
        {
            var config = string.Concat(
				"H4sIAAAAAAAEABNgYARDQoAHqIYfSDsKQPiOHFCaBY12gNIKlNG8UPsAtuqg",
				"W6AAAAA=");


			using (var compressStream = new MemoryStream(Convert.FromBase64String(config)))
            {
                using (var zipStream = new GZipStream(compressStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        FrameIO.Runtime.FrameIOFactory.Initial(resultStream.ToArray());
                    }
                }
            }
        }

    }
}
