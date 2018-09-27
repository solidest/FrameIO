using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.CodeTemplate;
using PROJECT1.Frame;
using System.Collections.Generic;
using FrameIO.Interface;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace PROJECT1.SYS2
{
    public class SYS2:INotifyPropertyChanged
    {

        public void Initialization()
        {

            CHA = new CANChannel();
            var _cfg = new Dictionary<string, object>();
            _cfg.Add("config1", "value1");
            _cfg.Add("config2", "value2");
            CHA.Open(_cfg);

        }
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
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(this.PROPERTYC)));
                }
            }
        }

        private CANChannel CHA;

        public void recv_FRAME1_CHA()
        {
            var _p = new FRAME1Parser();
            var data = (MSG1)CHA.ReadFrame(_p);
            UpdatePropertys(data);
        }

        public void recvloop_FRAME1_CHA(AsyncReadListCallback callback)
        {
            var _p = new FRAME1Parser();
            CHA.BeginReadFrameList(_p, 1, true, callback);
        }

        //TODO 复制以下内容到UI类内部执行
        /*
        private delegate void UpdatePropertysHandler(FRAME1 fdata);

        private void recvloop_FRAME1_CHA_callback(FrameBase[] data, out bool isCompleted, object AsyncState)
        {
            var fdata = (FRAME1[])data;
            isCompleted = false;
            if (fdata != null && fdata.Length > 0)
            {
                Dispatcher.BeginInvoke(new UpdatePropertysHandler(SYS2.UpdatePropertys), fdata[data.Length - 1]);
            }
        }
        */

        public void UpdatePropertys(MSG1 fdata)
        {
            _PROPERTYA.Value = fdata.SEGMENTA;
            for (int i = 0; i < 4; i++)
            {
                _PROPERTYB[i].Value = fdata.SEGMENTB[i];
            }
            _PROPERTYC.Value = fdata.SEGMENTC;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
