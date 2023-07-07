using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace AutomaticWebBrowser.Wpf.Common
{
    class Favicon2BitmapImageConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                try
                {
                    return new BitmapImage (new Uri (str));
                }
                catch (Exception)
                { }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException ();
        }
    }
}
