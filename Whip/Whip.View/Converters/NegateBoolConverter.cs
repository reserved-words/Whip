using System;
using System.Globalization;
using System.Windows.Data;

namespace Whip.Converters
{
    public class NegateBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue;
            if (!bool.TryParse(value.ToString(), out boolValue))
            {
                return value;
            }

            return !boolValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
