using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using FrameIO.Interface;
using System.IO;
using System.IO.Compression;
using System;
using FrameIO.Run;

namespace <%project%>
{

    public static class FioRunner
    {

        static FioRunner()
        {
            var config = string.Concat(
                <%framesconfig%>);

            IORunner.InitialFromGZipBase64(config);
        }

    }
}
