using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace PriorMoney.DesktopApp.ViewModel.Converters
{
    public class VisibilityToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolVisibility = (bool)value;
            bool isParameterTrue = true;

            bool.TryParse((string)parameter, out isParameterTrue);
            if (!isParameterTrue)
            {
                boolVisibility = !boolVisibility;
            }

            return boolVisibility ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
