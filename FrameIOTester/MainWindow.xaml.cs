using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using demo;

namespace FrameIOTester
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //模拟一些分系统1要发送的数据值
            CUSTOM_SYS1 = new demo.SYS1();
            CUSTOM_SYS1.PROPERTYa.Value = 10;
            CUSTOM_SYS1.PROPERTYb.Value = -8;
            CUSTOM_SYS1.PROPERTYc.Value = 98765;
            CUSTOM_SYS1.PROPERTYd.Value = -123567.9870001;
            for (int i = 0; i < 8; i++)
            {
                CUSTOM_SYS1.PROPERTYe.Add(new Parameter<bool?>(i % 2 == 0 ? true : false));
            }

            //将分系统1绑定到UI上
            ctrSYS1.DataContext = CUSTOM_SYS1;
            listctr1.ItemsSource = CUSTOM_SYS1.PROPERTYe;

            //将分系统2绑定到UI上
            CUSTOM_SYS2 = new demo.SYS2();
            ctrSYS2.DataContext = CUSTOM_SYS2;
            listctr2.ItemsSource = CUSTOM_SYS2.PROPERTY2e;

            //打开通道
            CUSTOM_SYS2.CHA.Open();
            CUSTOM_SYS1.CH1.Open();

        }

        private static int send_iframe = 0;
        private static int recv_iframe = 0;

        public demo.SYS1 CUSTOM_SYS1 { get; set; }
        public demo.SYS2 CUSTOM_SYS2 { get; set; }

        //向分系统1发送数据
        private void SendData(object sender, RoutedEventArgs e)
        {
            CUSTOM_SYS1.SendData();
            send_iframe += 1;
            labSYS1.Content = string.Format("发送第{0}帧数据", send_iframe);

        }


        //接收分系统2数据
        private void RecvData(object sender, RoutedEventArgs e)
        {
            if (recv_iframe < send_iframe)
            {
                CUSTOM_SYS2.RecvData();
                recv_iframe += 1;
                labSYS2.Content = string.Format("接收第{0}帧数据", recv_iframe);
            }
        }


        //关闭
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //关闭通道
            CUSTOM_SYS1.CH1.Close();
            CUSTOM_SYS2.CHA.Close();
        }


        //输出文本
        private void OutText(string text, bool isReset)
        {
            MessageBox.Show(text);

        }
    }
}
