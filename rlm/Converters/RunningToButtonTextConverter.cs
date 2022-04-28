using System;
using System.Globalization;
using System.Windows.Data;

namespace rlm.Converters
{
    class RunningToButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool running && !running ? "▶" : "❚❚";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
