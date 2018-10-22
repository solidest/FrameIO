using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrameIO.Driver
{
    public  class TCPServerHelper
    {
        public Socket server = null;
        string localIP = String.Empty;
        EndPoint point = new IPEndPoint(IPAddress.Any, 0);

        private Socket serverTemp = null;
        private IPEndPoint serverEPoint = null;
        public Socket InitServer(Dictionary<string, object> config)
        {
            if(server==null)
            {
                string host = "" + config["serverip"];
                int port = Convert.ToInt32(config["port"]);
                IPAddress ip = IPAddress.Parse(host);
                serverEPoint = new IPEndPoint(ip, port);
                
            }
            return server;
        }
        public  bool Open()
        {
            if (server == null)
            {
                try
                {
                    serverTemp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    serverTemp.Bind(serverEPoint);
                    serverTemp.Listen(0);
                    serverTemp.BeginAccept(new AsyncCallback(AcceptConnection), serverTemp);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
            return false;
        }
        private  void AcceptConnection(IAsyncResult ar)
        {
            Socket mySserver = (Socket)ar.AsyncState;
            server = mySserver.EndAccept(ar);

        }
        private static int recvlen = 0;
        private static bool running = true;
        public Byte[] BeginReceive(int len)
        {
            Byte[] data = new byte[len];

            int dataleft = len;
            recvlen = 0;
            try
            {
                while (recvlen < len)
                {
                    running = true;
                    server.BeginReceive(data, recvlen, data.Length - recvlen, SocketFlags.None,
                    asyncResult =>
                    {
                        lock (this)
                        {
                            recvlen += server.EndReceive(asyncResult);
                            running = false;
                        }
                    }, null);

                    while (running)
                        Thread.Sleep(10);
                }
                return data;
            }
            catch (Exception)
            {
                throw new Exception("接收数据异常:");
            }
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
