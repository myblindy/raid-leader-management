using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace rlm.Converters
{
    class VisibleIfNonZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is double dValue && dValue != 0 ? Visibility.Visible :
            value is int iValue && iValue != 0 ? Visibility.Visible :
            value is bool bValue && bValue ? Visibility.Visible :
            Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
