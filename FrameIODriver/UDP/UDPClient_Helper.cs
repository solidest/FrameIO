using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Driver
{
    public class UDPHelper
    {
        public UdpClient UdpClient = null;
        private string localIP = "127.0.0.1";
        private string remoteIP = "127.0.0.1";
        private int localPort = 8008;
        private int remotePort = 8009;

        private IPEndPoint localEndPoint = null;
        public IPEndPoint remoteEndPoint = null;

        public UdpClient InitClient(Dictionary<string,object> config)
        {
            if (UdpClient == null)
            {
                //                 IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("" +config["localip"]), Convert.ToInt32(config["localport"]));
                //                 client = new Socket(ipep.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                //                 client.ExclusiveAddressUse = false;
                //                 client.Bind(ipep);
                // 
                //                 remoteEp = new IPEndPoint(IPAddress.Parse("" + config["remoteip"]), Convert.ToInt32(config["remoteport"]));
                UdpClient = new UdpClient();
                localEndPoint = new IPEndPoint(IPAddress.Parse("" + config["localip"]), Convert.ToInt32(config["localport"]));
                remoteEndPoint = new IPEndPoint(IPAddress.Parse("" + config["remoteip"]), Convert.ToInt32(config["remoteport"]));

                UdpClient.Client.Bind(localEndPoint);

            }
            return UdpClient;
        }

        public void CloseUDPClient()
        {
            if(UdpClient != null)
            {
                UdpClient.Client.Close();
                UdpClient.Close();
            }
        }
        public  int sendMsg(byte[] msg)
        {
            return UdpClient.Send(msg, msg.Length, remoteEndPoint);
        }

        public byte[] ReceiveMsg()
        {
            var p = remoteEndPoint;
            if (p == null)
                p = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0);

            return UdpClient.Receive(ref p); 
        }
    }
}
