using System;


namespace FrameIO.Main
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts <see cref="Complex" /> instances to <see cref="string" /> instances.
    /// </summary>
    [ValueConversion(typeof(Exp), typeof(string))]
    public class ComplexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Exp)
            {
                var c = (Exp)value;
                if (targetType == typeof(string))
                {
                    return c.ToString();
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var c = (string)value;
                if (targetType == typeof(Exp))
                {
                    return new Exp() { ConstStr = c, Op = exptype.EXP_ID };
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}