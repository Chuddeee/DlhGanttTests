using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GanttChartDataGridCustomDataBindingSample
{
    /// <summary>
	/// Converts a double (or generic number) to/from a number string value, with percent formatting support ('%').
	/// </summary>
	public sealed class NumberStringConverter : IValueConverter
    {
        static NumberStringConverter()
        {
            Instance = new NumberStringConverter();
        }

        public static NumberStringConverter Instance { get; private set; }

        /// <summary>
        /// Converts a double value to a number string value.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            double num = System.Convert.ToDouble(value);
            string format = (parameter != null) ? ((string)parameter) : "0.##";
            return num.ToString(format);
        }

        /// <summary>
        /// Converts a number string value to a double value.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0.0;
            }
            string text = System.Convert.ToString(value).Trim();
            string text2 = (parameter != null) ? ((string)parameter) : "0.##";
            bool flag = text2.EndsWith("%");
            if (flag && text.EndsWith("%"))
            {
                text = text.Substring(0, text.Length - 1);
            }
            bool flag2 = !flag && (text2.StartsWith("$") || text2.StartsWith("€") || text2.StartsWith("£") || text2.StartsWith("¥") || text2.StartsWith("¤"));
            if (flag2)
            {
                if (text.StartsWith(text2[0].ToString()))
                {
                    text = text.Substring(1).Trim();
                }
                else if (text.StartsWith("-" + text2[0].ToString()) || text.StartsWith("+" + text2[0].ToString()))
                {
                    text = text[0] + text.Substring(2).Trim();
                }
            }
            double num;
            if (double.TryParse(text, out num))
            {
                return flag ? (num / 100.0) : num;
            }
            return 0.0;
        }
    }
}
