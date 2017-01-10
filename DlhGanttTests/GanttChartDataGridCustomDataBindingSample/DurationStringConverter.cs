using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace GanttChartDataGridCustomDataBindingSample
{
    /// <summary>
	/// Converts a time span to/from a duration string value.
	/// The parameter may be of this form: {StringFormat}[{Scale}]. For example, 0.##h for hours, or 0.##d/8 for working days of 8 hours.
	/// </summary>
	public sealed class DurationStringConverter : IValueConverter
    {
        static DurationStringConverter()
        {
            Instance = new DurationStringConverter();
        }

        public static DurationStringConverter Instance { get; private set; }

        /// <summary>
        /// Converts a time span value to a duration string value.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            double num = ((TimeSpan)value).TotalHours;
            string text = (parameter != null) ? ((string)parameter) : "0.##h";
            int num2 = text.IndexOf('/');
            if (num2 >= 0)
            {
                double num3 = double.Parse(text.Substring(num2 + 1));
                num /= num3;
                text = text.Substring(0, num2);
            }
            return num.ToString(text);
        }

        /// <summary>
        /// Converts a duration string value to a time span value.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = (string)value;
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            text = text.Trim();
            string text2 = (parameter != null) ? ((string)parameter) : "0.##h";
            int num = text2.IndexOf('/');
            double num2 = 1.0;
            if (num >= 0)
            {
                num2 = double.Parse(text2.Substring(num + 1));
                text2 = text2.Substring(0, num);
            }
            string text3 = "h";
            if (char.IsLetter(text2, text2.Length - 1))
            {
                text3 = text2.Substring(text2.Length - 1);
            }
            if (text.EndsWith(text3))
            {
                text = text.Substring(0, text.Length - text3.Length);
            }
            double num3;
            if (double.TryParse(text, out num3))
            {
                return TimeSpan.FromHours(num3 * num2);
            }
            return null;
        }
    }
}
