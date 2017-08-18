using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DynamicTabItems
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        static BoolToVisibilityConverter()
        {
            Instance = new BoolToVisibilityConverter();
        }

        public static BoolToVisibilityConverter Instance { get; private set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return Visibility.Collapsed;

            bool objValue = (bool)value;

            if (objValue)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
