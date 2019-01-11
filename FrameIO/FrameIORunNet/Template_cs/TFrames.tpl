

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
