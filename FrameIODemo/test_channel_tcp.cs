using FrameIO.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.Main
{
    public partial class MainWindow
    {
        int loop = 100;
        void AsyncResult(ISegmentGettor data, out bool isCompleted, object AsyncState)
        {
            loop = loop - 1;
            isCompleted = loop == 0;
            Debug.WriteLine(loop.ToString());
            //OutText(string.Format("AsyncResult {0}", loop), false);
        }

        private void test_tcp()
        {
            #region --配置文件内容--

            //配置文件内容
            /*
            //项目:test
            project main
            {

	            //分系统
                system SYS1
                {
                    channel CHS:tcpserver
                    {
                        serverip="127.0.0.1";
                        port = 8007;
                    }

                    channel CHC:tcpclient
                    {
                        serverip="127.0.0.1";
                        port = 8007;
                    }

                }
                //数据帧
                frame MSG1
                {
                    integer a bitcount=4 signed=true;
                    integer b bitcount=4 signed=false;
                    integer c bitcount=32 signed=false;
                    real d isdouble=true;
                    integer e bitcount=1 repeated=b;
                }
             }
            */



            #endregion

            #region --准备测试数据--

            DateTime beforDT = System.DateTime.Now;

            test += 6;

            //准备测试数据
            sbyte a = -8;
            byte b = 8;
            int c = -99876 + test;
            double d = -7.5633484450000007;
            bool?[] bool_arr = new bool?[8];
            bool_arr[5] = true;

           

            #endregion


 
            var CHS = Run.FrameIOFactory.GetChannel("SYS1", "CHS");
            CHS.Open();

            var CHC = Run.FrameIOFactory.GetChannel("SYS1", "CHC");
            CHC.Open();



            

            var unpack = Run.FrameIOFactory.GetFrameUnpack("MSG1");
            //var data = CHS.ReadFrame(unpack);
            

            CHS.BeginReadFrame(unpack, AsyncResult,null);

            for(int i=0;i<100;i++)
            {
                //获取打包接口
                var settor = Run.FrameIOFactory.GetFramePack("MSG1");
                settor.SetSegmentValue(1, a);
                settor.SetSegmentValue(2, b);
                settor.SetSegmentValue(3, c);
                settor.SetSegmentValue(4, d);
                settor.SetSegmentValue(5, bool_arr);

                CHC.WriteFrame(settor.GetPack());
            }


            //var buf = pack.Pack();

            //             var CH2 = Run.FrameIOFactory.GetChannel("SYS2", "CHA");
            //             CH2.Open();
            //             var unpack = Run.FrameIOFactory.GetFrameUnpack("MSG1");
            //             var data = CH2.ReadFrame(unpack);

            //var unpack = Run.FrameIOFactory.GetFrameUnpack("MSG1");
            //var data = CH1.ReadFrame(unpack);

            #region --验证收到的数据--

            //模拟驱动接收的数据
            //var buf1 = new byte[buf.Length - 1];
            //for (int i = 0; i < buf1.Length; i++)
            //    buf1[i] = buf[i];
            //var buf2 = new byte[1];
            //buf2[0] = buf[buf.Length - 1];

            //获取解包接口


            //模拟驱动调用解包接口
            //Debug.Assert(u.FirstBlockSize == buf.Length - 1);
            //int ii = u.AppendBlock(buf1);
            //Debug.Assert(ii == 1);
            //ii = u.AppendBlock(buf2);
            //Debug.Assert(ii == 0);
            //var data = u.Unpack();

            //读取数值
            //             var a1 = data.GetSByte("a");
            //             var b1 = data.GetByte("b");
            //             var c1 = data.GetInt("c");
            //             var d1 = data.GetDouble("d");
            //             var bool_arr1 = data.GetBoolArray("e");
            // 
            //             Debug.Assert(a == a1);
            //             Debug.Assert(b == b1);
            //             Debug.Assert(c == c1);
            //             Debug.Assert(d == d1);
            //             Debug.Assert(bool_arr1[5]);


            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);

            OutText(string.Format("测试通过，用时{0}毫秒", ts.TotalMilliseconds), false);

            #endregion

        }

    }
}
