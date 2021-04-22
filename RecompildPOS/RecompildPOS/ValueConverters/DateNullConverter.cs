using System;
using System.Globalization;
using Xamarin.Forms;
namespace RecompildPOS.ValueConverters
{
    public class DateNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DateTime.Now;
            else
            {
                if (((DateTime)value) <= new DateTime(1900, 01, 01))
                    return DateTime.Now;
                else
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
