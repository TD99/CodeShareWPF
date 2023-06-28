using System.Globalization;
using System.Windows.Data;
using System;

namespace CodeShare.Converters
{
    public class AddValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double actualHeight && parameter is string additionalValueString && double.TryParse(additionalValueString, out double additionalValue))
            {
                return actualHeight + additionalValue;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
