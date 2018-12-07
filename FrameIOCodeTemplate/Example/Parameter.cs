
using System.ComponentModel;

namespace PROJECT1
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
                    if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
                if (ValueUpdated != null) ValueUpdated.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
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

    public class Helper
    {
        static public int GetMin(int n1, int n2)
        {
            return n1 > n2 ? n2 : n1;
        }
    }
}
