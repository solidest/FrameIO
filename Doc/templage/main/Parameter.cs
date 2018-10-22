
using System.ComponentModel;

namespace main
{
    public class Parameter<T> : INotifyPropertyChanged
    {
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
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
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
    }
}
