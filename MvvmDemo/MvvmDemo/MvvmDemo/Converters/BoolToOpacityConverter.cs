using System;
using System.Globalization;
using Xamarin.Forms;

namespace MvvmDemo.Converters
{
    public class BoolToOpacityConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;
        public double TrueValue { get; set; } = 1;
        public double FalseValue { get; set; } = 0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b && b != Invert ? TrueValue : FalseValue;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}