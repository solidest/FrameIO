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
                string host = "" + config["serverip"];
                int port = Convert.ToInt32(config["port"]);
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);
                
                serverTemp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverTemp.Bind(ipe);
                serverTemp.Listen(0);

                //server = serverTemp.Accept();
                serverTemp.BeginAccept(new AsyncCallback(AcceptConnection), serverTemp);
            }
            return server;
        }
        private  void AcceptConnection(IAsyncResult ar)
        {
            Socket mySserver = (Socket)ar.AsyncState;
            server = mySserver.EndAccept(ar);

        }
        private  void SendData(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            try
            {
                server.EndSend(ar);
            }
            catch
            {
                client.Close();
                serverTemp.BeginAccept(new AsyncCallback(AcceptConnection), serverTemp);
            }
        }
        public  void Send(byte[] msg)
        {
            server.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(SendData), server);

        }
        public void CloseServer()
        {
            //serverTemp.Shutdown(SocketShutdown.Both);
            //serverTemp.Close();

            if(server!=null)
            {
                if (server.Connected)
                {
                    server.Close();
                }
            }

        }

    }

}
