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
        private IPEndPoint localEndPoint = null; 
        public IPEndPoint remoteEndPoint = null;
        private int ReceiveTimeOut = 5000;
        private bool IsRunning = false;
        public UdpClient InitClient(Dictionary<string,object> config)
        {
            if (UdpClient == null)
            {
                if(!config.ContainsKey("localip") || !config.ContainsKey("localport") || !config.ContainsKey("remoteip") || !config.ContainsKey("remoteport"))
                {
                    throw new FrameIO.Interface.FrameIOException(Interface.FrameIOErrorType.ChannelErr, "初始化UDP", "缺少初始化配置参数!");
                }
                UdpClient = new UdpClient();
                localEndPoint = new IPEndPoint(IPAddress.Parse("" + config["localip"]), Convert.ToInt32(config["localport"]));
                remoteEndPoint = new IPEndPoint(IPAddress.Parse("" + config["remoteip"]), Convert.ToInt32(config["remoteport"]));

                //UdpClient.Client.Bind(localEndPoint);
                

                if (config.ContainsKey("waittimeout"))
                    ReceiveTimeOut = Convert.ToInt32(config["waittimeout"]);
                UdpClient.Client.ReceiveTimeout= ReceiveTimeOut;

            }
            return UdpClient;
        }
        public bool Open()
        {
            try
            {
                if(UdpClient!=null && !IsRunning)
                {
                    UdpClient.Client.Bind(localEndPoint);
                    IsRunning = true;
                }
                    
                return true;
            }catch(Exception)
            {
                return false;
            }
            
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
            try
            {
                return UdpClient.Receive(ref p);
            }catch(Exception )
            {
                throw new FrameIO.Interface.FrameIOException(Interface.FrameIOErrorType.RecvErr, "接收UDP消息", "接收UDP消息超时");
            }
            
        }
    }
}
