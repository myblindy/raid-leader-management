using System;
using System.Globalization;
using System.Windows.Data;

namespace rlm.Converters
{
    class IndexStringToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
            values[0] is int idx && values[1] is string[] vals ? vals[idx] : null;

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
