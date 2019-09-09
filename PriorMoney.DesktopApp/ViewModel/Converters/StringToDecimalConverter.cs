using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PriorMoney.DesktopApp.ViewModel.Converters
{
    public class StringToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var decValue = (decimal)value;

            return decValue.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value as string;
            decimal decValue;
            if(decimal.TryParse(strValue, out decValue))
            {
                return decValue;
            }

            return null;
        }
    }
}
