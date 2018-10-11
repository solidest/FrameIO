using Automation.BDaq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameIO.Driver
{
    partial class DIHelper
    {
        private ErrorCode errorCode = ErrorCode.Success;

        private DeviceInformation DevInformation;
        private Object DiCtrl;

        private static Dictionary<UInt32, StateMutex> ChannelState = new Dictionary<UInt32, StateMutex>();

        private Object DoCtrl;

        private UInt32 CardNO;
        private UInt32 ChNo;
        private System.Boolean OpenDIO()
        {
            if (!DoOpenDevice()) return false;
            if (!DoOpenDIChannel()) return false;
            if (!DoOpenDOChannel()) return false;

            return true;
        }
        public System.Boolean OpenDIO(Dictionary<string,object> config)
        {
            CardNO = (UInt32)config["DeviceNo"];
            ChNo = (UInt32)config["ChannelNo"];

            return OpenDIO();
        }
        private System.Boolean DoOpenDevice()
        {
            try
            {
                DevInformation = new DeviceInformation((int)CardNO);
            }
            catch (Exception ex)
            {
                String errStr = BioFailed(errorCode) ? " 发生错误. 错误是：" + errorCode.ToString()
                                                       : ex.Message;
                Console.WriteLine(errStr);
                return false;
            }
            return true;
        }
        private System.Boolean DoOpenDIChannel()
        {
            try
            {
                InstantDiCtrl diCtrl = new InstantDiCtrl();
                diCtrl.SelectedDevice = DevInformation;
                DiCtrl = diCtrl;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private System.Boolean ReadValue()
        {
            Byte[] buffer = new Byte[4];
            try
            {
                InstantDiCtrl diCtrl = DiCtrl as InstantDiCtrl;
                if (diCtrl == null)
                    throw new Exception("研华DIO板卡.DI 输出失败：diCtrl转换失败");

                errorCode = diCtrl.Read((int)(ChNo / 8), out buffer[ChNo / 8]);

                if (BioFailed(errorCode)) throw new Exception();
                System.Collections.BitArray array = new System.Collections.BitArray(buffer);
                return array[(int)ChNo];
            }
            catch (Exception e)
            {
                String errStr = BioFailed(errorCode) ? " 发生错误. 错误是： " + errorCode.ToString()
                                       : e.Message;
                throw new Exception(errStr);
            }
        }
        static Boolean BioFailed(ErrorCode err)
        {
            if (err >= ErrorCode.Success) return false;
            if (err < ErrorCode.ErrorHandleNotValid) return false;
            return true;
        }

        private System.Boolean DoOpenDOChannel()
        {
            try
            {
                InstantDoCtrl doCtrl = new InstantDoCtrl();
                doCtrl.SelectedDevice = DevInformation;
                DoCtrl = doCtrl;
                if (ChannelState.Keys.Contains(CardNO)) return true;
                ChannelState.Add(CardNO, new StateMutex());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("研华DIO板卡 DO 打开通道失败：" + ex.Message);
                return false;
            }
        }

        public System.Boolean WriteValue(bool value)
        {
            ErrorCode errorCode = ErrorCode.Success;
            try
            {
                InstantDoCtrl doCtrl = DoCtrl as InstantDoCtrl;
                if (doCtrl == null)
                {
                    Console.WriteLine("研华DIO板卡.DO 输出失败：doCtrl转换失败");
                    return false;
                }

                ChannelState[CardNO].SetState((int)ChNo % 8, value);

                errorCode = doCtrl.Write((int)(ChNo / 8), ChannelState[CardNO].State);
                Console.WriteLine("DO 输出成功");
                return true;
            }
            catch (Exception e)
            {
                String errStr = BioFailed(errorCode) ? " 发生错误. 错误是： " + errorCode.ToString()
                                                       : e.Message;
                Console.WriteLine(errStr);
                return false;
            }
        }
    }
}
