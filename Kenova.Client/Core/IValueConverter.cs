using System;
using System.Globalization;

namespace Kenova.Client.Core
{
    public interface IValueConverter
    {
        object Convert(object value, Type targetType, object parameter, CultureInfo language);

        object ConvertBack(object value, Type targetType, object parameter, CultureInfo language);
    }
}