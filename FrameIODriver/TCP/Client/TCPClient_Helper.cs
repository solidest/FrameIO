using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Driver
{
    public  class TCPClientHelper
    {
         public Socket client = null;
         string localIP = String.Empty;
         EndPoint point = new IPEndPoint(IPAddress.Any, 0);

        public  Socket InitClient()
        {
            if (client == null)
            {
                string host = "127.0.0.1";
                int port = 8009;
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);

                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(ipe);
            }
            return client;
        }
        public Socket InitClient(Dictionary<string, object> config)
        {
            if (client == null)
            {
                string host = "" + config["serverip"];
                int port = (int)config["port"];
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);

                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(ipe);
            }
            return client;
        }
        public void CloseClient()
        {
            if (client != null)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }

        }
    }

}
