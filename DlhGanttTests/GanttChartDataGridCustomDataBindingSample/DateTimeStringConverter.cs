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
    /// Converts a date time to/from a string value.
    /// </summary>
    public sealed class DateTimeStringConverter : IValueConverter
    {
        private static string nullIndicator = "N/A";

        static DateTimeStringConverter()
        {
            Instance = new DateTimeStringConverter();
        }

        public static DateTimeStringConverter Instance { get; private set; }

        /// <summary>
        /// Gets or sets the text to identify unspecified date and time values (by default "N/A").
        /// </summary>
        public static string NullIndicator
        {
            get
            {
                return DateTimeStringConverter.nullIndicator;
            }
            set
            {
                DateTimeStringConverter.nullIndicator = value;
            }
        }

        /// <summary>
        /// Converts a date and time value to a string value.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DateTimeStringConverter.nullIndicator;
            }
            DateTime dateTime = (DateTime)value;
            string format = (parameter != null) ? ((string)parameter) : "g";
            return dateTime.ToString(format);
        }

        /// <summary>
        /// Converts a string value to a date and time value.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = (string)value;
            if (string.IsNullOrEmpty(text) || (DateTimeStringConverter.nullIndicator != null && text.ToLowerInvariant() == DateTimeStringConverter.nullIndicator.ToLowerInvariant()))
            {
                return null;
            }
            text = text.Trim();
            DateTime dateTime;
            if (DateTime.TryParse(text, out dateTime))
            {
                return dateTime;
            }
            return null;
        }
    }
}
