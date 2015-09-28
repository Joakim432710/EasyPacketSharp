using System;
using System.Globalization;
using System.Windows.Data;

namespace ChatClient
{
    public class IsSelfToColumnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return (value is bool && (bool)value) ? 1 : 0;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value is int && (int)value == 1;
        }
    }

}
