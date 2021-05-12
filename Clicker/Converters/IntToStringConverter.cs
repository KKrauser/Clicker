using System;
using System.Globalization;
using System.Windows.Data;

namespace Clicker.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));

            return int.TryParse(value.ToString(), out _)
                ? value.ToString()
                : throw new ArgumentException($"{nameof(value)} is not of type int", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrWhiteSpace(value.ToString())) return 0;
            if (int.TryParse(value.ToString(), out var result)) return result;
            throw new ArgumentException($"{nameof(value)} cannot be parsed to int", nameof(value));
        }
    }
}