
using System.ComponentModel;
using FrameIO.Run;

namespace Tcpserver
{
    public class Parameter<T> : INotifyPropertyChanged
    {
        public delegate void ValueUpdatedEventHandler(object sender, PropertyChangedEventArgs e);
        private T _value;

        public Parameter(T value)
        {
            _value = value;
        }

        public Parameter()
        {

        }

        public string Name { get; set; }
        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
                ValueUpdated?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        public bool IsNull
        {
            get
            {
                return Equals(_value, null);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event ValueUpdatedEventHandler ValueUpdated;
    }

}
