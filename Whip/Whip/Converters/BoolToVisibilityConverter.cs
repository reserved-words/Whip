using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Whip.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public BoolToVisibilityConverter()
        {
            FalseVisibility = Visibility.Collapsed;
        }

        public bool Negate { get; set; }
        public Visibility FalseVisibility { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue;
            if (!bool.TryParse(value.ToString(), out boolValue))
            {
                return value;
            }

            return GetVisibility(boolValue != Negate);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Visibility GetVisibility(bool visible)
        {
            return visible ? Visibility.Visible : FalseVisibility;
        }
    }
}
