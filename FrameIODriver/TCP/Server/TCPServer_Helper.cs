﻿using FrameIO.Interface;
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
        private int ReceiveTimeOut = 5000;


        private Socket serverTemp = null;
        private bool IsRunning = false;

        public Socket InitServer(Dictionary<string, object> config)
        {
            if (client == null)
            {
                if (!config.ContainsKey("serverip") || !config.ContainsKey("port") || !config.ContainsKey("clientip"))
                    throw new FrameIO.Interface.FrameIOException(Interface.FrameIOErrorType.ChannelErr, "初始化TCP Client", "缺少初始化配置参数!");

                listenerIp = "" + config["serverip"];
                listenerPort = Convert.ToInt32(config["port"]);
                clientIp = "" + config["clientip"];

                if (config.ContainsKey("waittimeout"))
                    ReceiveTimeOut = Convert.ToInt32(config["waittimeout"]);

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
                    serverTemp.ReceiveTimeout = ReceiveTimeOut;
                    serverTemp.Listen(5);
                }

                serverTemp.BeginAccept(new AsyncCallback(AcceptConnection), serverTemp);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void AcceptConnection(IAsyncResult ar)
        {
            Socket mySserver = (Socket)ar.AsyncState;

            var newClient = mySserver.EndAccept(ar);
            System.Diagnostics.Debug.WriteLine(newClient.RemoteEndPoint.ToString());
            client = newClient;

            serverTemp.BeginAccept(new AsyncCallback(AcceptConnection), serverTemp);

        }
        private static int recvlen = 0;
        private static bool running = true;
        public Byte[] BeginReceive2(int len)
        {
            if (null == client || !client.Connected)
                throw new FrameIO.Interface.FrameIOException(FrameIOErrorType.RecvErr, "TCP服务器端", "客户端未连接!");

            Byte[] data = new byte[len];

            int pos = 0;
            int irecv = 0;

            while (pos<len)
            {
                try
                {
                    irecv = client.Receive(data, pos, len - pos, SocketFlags.None);
                }
                catch (Exception)
                {
                    throw new FrameIO.Interface.FrameIOException(FrameIOErrorType.RecvErr, "TCP服务器端", len.ToString());
                }

                if (irecv == 0)
                    throw new FrameIO.Interface.FrameIOException(FrameIOErrorType.RecvErr, "TCP服务器端", "客户端连接已断开!");
                else
                    pos += irecv;
            }
            return data;
        }

        public Byte[] BeginReceive(int len)
        {
            if (null == client || !client.Connected)
                throw new FrameIO.Interface.FrameIOException(FrameIOErrorType.RecvErr, "TCP服务器端", "客户端未连接!");

            Byte[] data = new byte[len];

            int dataleft = len;
            recvlen = 0;

            var dicclient = client;
            var errinfo = "";
            try
            {
                while (recvlen < len && errinfo.Length==0)
                {
                    running = true;
                    
                    //dicclient.BeginReceive(data, recvlen, data.Length - recvlen, SocketFlags.None,
                    //asyncResult =>
                    //{
                    //    lock (this)
                    //    {
                    //        //try
                    //        //{
                    //        //    recvlen += dicclient.EndReceive(asyncResult);
                    //        //    running = false;
                    //        //}
                    //        //catch (Exception ex)
                    //        //{
                    //        //    errinfo = ex.Message;
                    //        //}
                    //        recvlen += dicclient.EndReceive(asyncResult);
                    //        running = false;
                    //    }

                    //}, null);

                    while (running)
                        Thread.Sleep(10);
                }
                if(errinfo.Length==0)
                    return data;
            }
            catch (Exception)
            {
                throw new FrameIO.Interface.FrameIOException(FrameIOErrorType.RecvErr, "TCP服务器端", "接收数据超时!");
            }

            throw new FrameIO.Interface.FrameIOException(FrameIOErrorType.RecvErr, "TCP服务器端", errinfo);

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
            //var client = clients.ToArray().FirstOrDefault(c => c.Value.RemoteEndPoint.ToString() == c.Key);

            client.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(SendData), client);

        }

        public void CloseServer()
        {

            if (client.Connected)
                client.Close();



        }

    }

}
