using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;

namespace GanttChartDataGridRecurrenceSample
{
    public class UnlimitedIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue = (int)value;
            return intValue < int.MaxValue ? intValue.ToString() : "Unlimited";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = (string)value;
            if (stringValue == string.Empty || stringValue.ToLowerInvariant() == "unlimited")
                return int.MaxValue;
            int intValue;
            if (int.TryParse(stringValue, out intValue))
                return intValue;
            return 0;
        }
    }
}
