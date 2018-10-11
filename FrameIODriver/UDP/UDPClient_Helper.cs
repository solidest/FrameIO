﻿using System;
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
        public Socket client = null;
        string localIP = String.Empty;
        EndPoint point = new IPEndPoint(IPAddress.Any, 0);
        public EndPoint remoteEp = null;

        public  Socket InitClient()
        {
            if (client == null)
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 8008);
                client = new Socket(ipep.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                client.ExclusiveAddressUse = false;
                client.Bind(ipep);

                remoteEp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8007);

            }
            return client;
        }
        public Socket InitClient(Dictionary<string,object> config)
        {
            if (client == null)
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("" +config["LocalIP"]), (int)config["LocalPort"]);
                client = new Socket(ipep.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                client.ExclusiveAddressUse = false;
                client.Bind(ipep);

                remoteEp = new IPEndPoint(IPAddress.Parse("" + config["RemoteIP"]), (int)config["RemotePort"]);

            }
            return client;
        }

        public void CloseUDPClient()
        {
            if(client !=null)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }
        public  int sendMsg(byte[] msg)
        {
            return client.SendTo(msg, SocketFlags.None, point);
        }

        public void ReceiveMsg(int len)
        {
            byte[] buff = new byte[len];
            byte[] recvBuf = new byte[len];

            int dataLeft = len;
            int start = 0;
            while (dataLeft > 0)
            {
                int recv = client.ReceiveFrom(recvBuf, ref remoteEp);

                if (recv > len - start)
                    Array.Copy(recvBuf, 0, buff, start, len - start);
                else
                    Array.Copy(recvBuf, 0, buff, start, recv);

                start += recv;
                dataLeft -= recv;
            }

        }
    }
}