using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace rlm.Converters
{
    class NameToImageSourceConverter : IValueConverter
    {
        static readonly string ApplicationPath = AppContext.BaseDirectory;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is not null ? new BitmapImage(new(Path.Combine(ApplicationPath, @$"Data\Images\{value}.png"))) : DependencyProperty.UnsetValue;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
