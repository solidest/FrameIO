using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameIO.CodeTemplate
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
                if(!Equals(_value, value))
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
