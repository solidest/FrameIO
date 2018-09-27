using System.Collections.ObjectModel;
using System.ComponentModel;
using FrameIO.CodeTemplate;
using PROJECT1.Frame;
using System.Collections.Generic;

namespace PROJECT1.SYS1
{
    public class SYS1
    {

        public void Initialization()
        {

            CH1 = new CANChannel();
            var _cfg = new Dictionary<string, object>();
            _cfg.Add("config1", "value1");
            _cfg.Add("config2", "value2");
            CH1.Open(_cfg);

            CH2 = new COMChannel();
            _cfg.Clear();
            _cfg.Add("configa", "valuea");
            _cfg.Add("configb", "valueb");
            CH2.Open(_cfg);

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
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(this.PROPERTYA)));
                }
            }
        }

        private CANChannel CH1;
        private COMChannel CH2;

        public void send_FRAME1_CH1()
        {
            var data = new MSG1();
            data.SEGMENTA = _PROPERTYA.Value ?? 0;
            for(int i=0; i<4; i++)
            {
                data.SEGMENTB[i] = _PROPERTYB[i].Value ?? 0;
            }
            data.SEGMENTC = _PROPERTYC.Value ?? 0;
            CH1.WriteFrame(data);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
