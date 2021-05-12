using System;
using System.Globalization;
using System.Windows.Data;

namespace Clicker.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Inverse(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Inverse(value);
        }

        private static bool Inverse(object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.GetType() != typeof(bool))
                throw new ArgumentException($"{nameof(value)} is not boolean type!", nameof(value));
            return !(bool) value;
        }
    }
}