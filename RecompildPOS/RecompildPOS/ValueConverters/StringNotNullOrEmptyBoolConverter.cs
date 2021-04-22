using System;
using Xamarin.Forms;

namespace RecompildPOS.ValueConverters
{
    public class StringNotNullOrEmptyBoolConverter : IValueConverter
    {
        /// <summary>
        /// Returns true if string is not null or empty
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>True if string is not null or empty</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is string s ? !string.IsNullOrEmpty(s) : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
