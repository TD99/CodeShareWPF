using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CodeShare.Converters
{
    public class RemoveAlphaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return Color.FromRgb(color.R, color.G, color.B);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
