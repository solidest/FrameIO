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
         public IPEndPoint ipe = null;

        private  bool IsConnectionSuccessful = false;
        private  Exception socketexception;
        private  System.Threading.ManualResetEvent TimeoutObject = new System.Threading.ManualResetEvent(false);
        private int timeoutMSec = 1000;

        public Socket InitClient(Dictionary<string, object> config)
        {
            if (client == null)
            {
                try
                {
                    string host = "" + config["serverip"];
                    int port = Convert.ToInt32(config["port"]);
                    if (config.Keys.Contains("timeout"))
                    {
                        timeoutMSec = Convert.ToInt32(config["timeout"]);
                    }

                    IPAddress ip = IPAddress.Parse(host);
                    ipe = new IPEndPoint(ip, port);

                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                catch
                {
                    throw new SocketException();
                }

                //client.Connect(ipe);
            }
            return client;
        }
        public bool Open()
        {
            if (client != null)
            {

                try
                {
                    if (Connect(ipe))
                        return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
        private  bool Connect(IPEndPoint remoteEndPoint)
        {
            TimeoutObject.Reset();
            socketexception = null;

            client.BeginConnect(remoteEndPoint, new AsyncCallback(CallBackMethod), client);

            if (TimeoutObject.WaitOne(timeoutMSec, false))
            {
                if (IsConnectionSuccessful)
                    return true;
                else
                    return false;
            }
            else
            {
                client.Close();
                //throw new TimeoutException("TimeOut Exception");
                return false;
            }

        }
        private  void CallBackMethod(IAsyncResult asyncresult)
        {
            try
            {
                IsConnectionSuccessful = false;
                Socket socketClient = asyncresult.AsyncState as Socket;

                if (socketClient != null)
                {
                    socketClient.EndConnect(asyncresult);
                    IsConnectionSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                IsConnectionSuccessful = false;
                socketexception = ex;
            }
            finally
            {
                TimeoutObject.Set();
            }
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
