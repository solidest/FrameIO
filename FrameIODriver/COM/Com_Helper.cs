using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Driver
{
    public  class ComHelper
    {
        private System.IO.Ports.SerialPort rs232 = null;

        public ComHelper()
        {
            rs232 = new System.IO.Ports.SerialPort();
        }
        public  System.IO.Ports.SerialPort RS232
        {
            get { return rs232; }
            set { rs232 = value; }
        }
        public int ReceiveTimeOut { get; set; } = 5000;

        public  bool Open()
        {
            try
            {
                rs232.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool IsOpen()
        {
            if (rs232.IsOpen)
                return true;
            return false;
        }
        public  void CloseCom()
        {
            if (rs232.IsOpen)
                rs232.Close();
        }
        public void InitPort(Dictionary<string, object> config)
        {
            if (!config.ContainsKey("portname") || !config.ContainsKey("baudrate") 
                || !config.ContainsKey("databits") || !config.ContainsKey("stopbits") 
                || !config.ContainsKey("parity") )
                throw new FrameIO.Interface.FrameIOException(Interface.FrameIOErrorType.ChannelErr, "初始化串口", "缺少初始化配置参数!");

            rs232.PortName = ""+config["portname"];
            rs232.BaudRate=Convert.ToInt32(config["baudrate"]);
            rs232.DataBits = Convert.ToInt32(config["databits"]);
            rs232.StopBits = (System.IO.Ports.StopBits)Convert.ToInt32(config["stopbits"]);
            rs232.Parity = (System.IO.Ports.Parity)Convert.ToInt32(config["parity"]);

            if(config.ContainsKey("waittimeout"))
                ReceiveTimeOut= Convert.ToInt32(config["waittimeout"]);

        }

        public bool DoWrite(Byte[] msg)
        {
            if(IsOpen())
            {
                RS232.Write(msg, 0, msg.Count());
                return true;
            }
            return false;
                
        }
    }
}
