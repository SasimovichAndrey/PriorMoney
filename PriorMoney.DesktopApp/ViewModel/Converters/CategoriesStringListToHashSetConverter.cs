using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PriorMoney.DesktopApp.ViewModel.Converters
{
    public class CategoriesStringListToHashSetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hashSetValue = (IEnumerable<object>)value;
            return string.Join<object>(", ", hashSetValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hashSet = ((string)value).Trim()
                            .Split(',')
                            .Select(s => s.Trim().ToUpper())
                            .ToHashSet();

            return hashSet;
        }
    }
}
