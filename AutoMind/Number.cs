using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AutoMind
{
    public class Number : FormulaElement, INotifyPropertyChanged
    {
        private double _value = 1;
        public override string DataType => "NUMBER";

        public double Value
        {
            get => _value;
            set
            {
                if (value.Equals(_value)) return;
                _value = value;
                OnPropertyChanged();
            }
        }
        
        public override double GetValue()
        {
            return Value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }

        public override string ToView()
        {
            return Value.ToString();
        }
        public override string ToValue()
        {
            return Value.ToString();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
