using System.Globalization;
using System;
using System.Windows.Data;

namespace CodeShare.Converters
{
    public class InvertibleNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = value != null;
            if (parameter is string invert && invert == "Inverted")
            {
                result = !result;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
