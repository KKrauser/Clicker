using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Clicker.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.GetType() != typeof(bool))
                throw new ArgumentException($"{nameof(value)} is not boolean type!", nameof(value));
            return (bool) value ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.GetType() != typeof(Visibility))
                throw new ArgumentException($"{nameof(value)} is not of type Visibility!", nameof(value));
            return (Visibility) value == Visibility.Visible;
        }
    }
}