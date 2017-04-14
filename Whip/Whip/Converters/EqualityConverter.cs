using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Whip.Converters
{
    public class EqualityConverter : IMultiValueConverter
    {
        public EqualityConverter()
        {
            FalseVisibility = Visibility.Hidden;
        }

        public Visibility FalseVisibility { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Distinct().Count() == 1 ? Visibility.Visible : Visibility.Hidden;
        }

        public object[] ConvertBack(
            object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
