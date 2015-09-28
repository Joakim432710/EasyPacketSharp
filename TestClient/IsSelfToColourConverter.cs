using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ChatClient
{
    public class IsSelfToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return (value is bool && (bool)value) ? new SolidColorBrush(Color.FromRgb(0xe5, 0xf7, 0xfd)) : new SolidColorBrush(Color.FromRgb(0xc7, 0xed, 0xfc));
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Unsupproted operation");
        }
    }

}
