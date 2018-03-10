using System;
using System.Globalization;
using System.Windows.Data;
using Whip.Common.ExtensionMethods;

namespace Whip.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var en = value as Enum;

            return en?.GetDisplayName() ?? value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
