using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

using HandyControl.Controls;

namespace AutomaticWebBrowser.Wpf.Common
{
    class FaviconElement
    {
        public static readonly DependencyProperty FaviconUriProperty = DependencyProperty.RegisterAttached ("FaviconUri", typeof (string), typeof (FaviconElement), new PropertyMetadata (default (Geometry)));

        public static void SetFaviconUri (DependencyObject element, string value)
        {
            element.SetValue (FaviconUriProperty, value);
        }

        public static string GetGeometry (DependencyObject element)
        {
            return (string)element.GetValue (FaviconUriProperty);
        }

        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached ("Width", typeof (double), typeof (FaviconElement), new PropertyMetadata (double.NaN));

        public static void SetWidth (DependencyObject element, double value)
        {
            element.SetValue (WidthProperty, value);
        }

        public static double GetWidth (DependencyObject element)
        {
            return (double)element.GetValue (WidthProperty);
        }

        public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached ("Height", typeof (double), typeof (FaviconElement), new PropertyMetadata (double.NaN));

        public static void SetHeight (DependencyObject element, double value)
        {
            element.SetValue (HeightProperty, value);
        }

        public static double GetHeight (DependencyObject element)
        {
            return (double)element.GetValue (HeightProperty);
        }
    }
}
