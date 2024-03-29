﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace rlm.Converters
{
    class RaidHoursToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is int hours && hours > 0 ? Brushes.Green : Brushes.Red;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
