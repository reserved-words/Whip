using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Whip.Converters
{
    public class NullVisibilityConverter : IValueConverter
    {
        public NullVisibilityConverter()
        {
            FalseVisibility = Visibility.Collapsed;
        }

        public Visibility FalseVisibility { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? FalseVisibility : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
