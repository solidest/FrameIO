using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FrameIO.Main
{
    //测试使用
    public partial class MainWindow
    {


        static private int test = 6;

        private void testchannel(object sender, RoutedEventArgs e)
        {

            //Run.FrameIOFactory.InitialFactory("FrameIO.bin");

            //test_Com();//测试串口
            test_tcpclient();//测试TCPClient
            //test_tcpserver();//测试服务器
            //test_tcp();//即包括服务器也包括客户端
            //test_udp();

        }

    }
}
