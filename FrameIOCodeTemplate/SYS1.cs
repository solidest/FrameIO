using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.CodeTemplate;
using FrameIO.Run;
using FrameIO.Interface;
using System.Diagnostics;

namespace PROJECT1.SYS1
{
    public class SYS1
    {
        private IChannelBase CH1;
        private IChannelBase CH2;

        public void Initialization()
        {
            CH1 = FrameIOFactory.GetChannel("SYS1", "CH1");
            CH2 = FrameIOFactory.GetChannel("SYS1", "CH2");

        }

        #region -- Property --


        private Parameter<ushort?> _PROPERTYA = new Parameter<ushort?>();
        private ObservableCollection<Parameter<byte?>> _PROPERTYB =new ObservableCollection<Parameter<byte?>>(new Parameter<byte?>[4]);
        private Parameter<double?> _PROPERTYC = new Parameter<double?>();
        public Parameter<ushort?> PROPERTYA
        {
            get
            {
                return _PROPERTYA;
            }
            set
            {
                if(_PROPERTYA!=value)
                {
                    _PROPERTYA = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(this.PROPERTYA)));
                }
            }
        }

        public ObservableCollection<Parameter<byte?>> PROPERTYB
        {
            get
            {
                return _PROPERTYB;
            }
        }

        public Parameter<double?> PROPERTYC
        {
            get
            {
                return _PROPERTYC;
            }
            set
            {
                if (_PROPERTYC != value)
                {
                    _PROPERTYC = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(this.PROPERTYA)));
                }
            }
        }

        #endregion

        //模板中的异常处理接口
        private void HandleFrameIOError(FrameIOException ex)
        {
            switch(ex.ErrType)
            {
                case FrameIOErrorType.ChannelErr:
                case FrameIOErrorType.SendErr:
                case FrameIOErrorType.RecvErr:
                case FrameIOErrorType.CheckDtaErr:
                    Debug.WriteLine("位置：{0}    错误：{1}", ex.ErrInfo, ex.ErrInfo);
                    break;
            }
        }

               
        public void SEND_ACTION_EXCAMPLE()
        {
            try
            {
                var funpack = FrameIOFactory.GetFrameUnpack("FRAME1");
                var data = CH1.ReadFrame(funpack);
                PROPERTYA.Value = data.GetUShort("SEG1");
            }
            catch (FrameIOException ex)
            {
                HandleFrameIOError(ex);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
