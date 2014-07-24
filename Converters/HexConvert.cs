using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF_LED_Controller.Converters
{
    [ValueConversion(typeof(byte), typeof(string))]
    public class HexConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            int returnValue = (int)value;
            string newst = returnValue.ToString("X").PadLeft(2, '0');
                return newst;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
