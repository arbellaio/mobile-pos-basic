using System;
using System.Globalization;
using Xamarin.Forms;

namespace RecompildPOS.ValueConverters
{
    public class CheckNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            if(value is int)
            {
                return (int)value != 0;
            }
            else if (value is string)
            {
                return !string.IsNullOrEmpty((string)value);
            }
            return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
