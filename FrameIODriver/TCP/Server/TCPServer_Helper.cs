using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Driver
{
    public  class TCPServerHelper
    {
        public Socket server = null;
        string localIP = String.Empty;
        EndPoint point = new IPEndPoint(IPAddress.Any, 0);

        private Socket serverTemp = null;
        public  Socket InitServer()
        {
            if (server == null)
            {
                string host = "127.0.0.1";
                int port = 8007;
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);
                
                serverTemp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverTemp.Bind(ipe);
                serverTemp.Listen(0);
                server = serverTemp.Accept();
            }
            return server;
        }
        public Socket InitServer(Dictionary<string, object> config)
        {
            if(server==null)
            {
                string host = "" + config["Host"];
                int port = (int)config["Port"];
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);
                
                serverTemp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverTemp.Bind(ipe);
                serverTemp.Listen(0);
                server = serverTemp.Accept();
            }
            return server;
        }

        public void CloseServer()
        {
            serverTemp.Shutdown(SocketShutdown.Both);
            serverTemp.Close();

            if(server.Connected)
            {
                server.Close();
            }
        }

    }

}
