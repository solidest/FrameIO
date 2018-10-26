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
    public class TCPServerHelper
    {
        public Socket client = null;
        private String listenerIp;
        private Int32 listenerPort;
        private String clientIp;
        private IPEndPoint serverEPoint = null;


        public static Dictionary<string, Socket> clients = new Dictionary<string, Socket>();
        private static Socket serverTemp = null;
        private static bool IsRunning = false;
        public Socket InitServer(Dictionary<string, object> config)
        {
            if (client == null)
            {
                listenerIp = "" + config["serverip"];
                listenerPort = Convert.ToInt32(config["port"]);
                clientIp = "" + config["clientip"];

                IPAddress ip = IPAddress.Parse(listenerIp);
                serverEPoint = new IPEndPoint(ip, listenerPort);

            }
            return client;
        }
        public bool Open()
        {
            try
            {
                if (!IsRunning)
                {
                    IsRunning = true;
                    serverTemp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    serverTemp.Bind(serverEPoint);
                    serverTemp.Listen(1);
                }
                foreach(var c in clients)
                {
                    if (c.Value.RemoteEndPoint.ToString().Contains(clientIp))
                        return false;
                }

                serverTemp.BeginAccept(new AsyncCallback(AcceptConnection), serverTemp);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void AcceptConnection(IAsyncResult ar)
        {
            Socket mySserver = (Socket)ar.AsyncState;

            var newClient = mySserver.EndAccept(ar);
            lock (clients)
            {
                clients.Add(newClient.RemoteEndPoint.ToString(), newClient);
                client = newClient;
            }

        }
        private static int recvlen = 0;
        private static bool running = true;
        public Byte[] BeginReceive(int len)
        {
            Byte[] data = new byte[len];

            int dataleft = len;
            recvlen = 0;

            var dicclient = clients.ToArray().FirstOrDefault(c => c.Value.RemoteEndPoint.ToString().Contains(clientIp));

            try
            {
                while (recvlen < len)
                {
                    running = true;
                    dicclient.Value.BeginReceive(data, recvlen, data.Length - recvlen, SocketFlags.None,
                    asyncResult =>
                    {
                        lock (this)
                        {
                            recvlen += dicclient.Value.EndReceive(asyncResult);
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
        private void SendData(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            try
            {
                client.EndSend(ar);
            }
            catch
            {
                client.Close();
                serverTemp.BeginAccept(new AsyncCallback(AcceptConnection), serverTemp);
            }
        }
        public void Send(byte[] msg)
        {
            var client = clients.ToArray().FirstOrDefault(c => c.Value.RemoteEndPoint.ToString() == c.Key);

            client.Value.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(SendData), client);

        }

        public void CloseServer()
        {
            foreach (var client in clients)
            {
                if (client.Value != null)
                {
                    if (client.Value.Connected)
                    {
                        client.Value.Close();
                    }
                }
            }


        }

    }

}
