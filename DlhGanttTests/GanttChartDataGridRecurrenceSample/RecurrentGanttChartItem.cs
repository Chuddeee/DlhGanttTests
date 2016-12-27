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
using DlhSoft.Windows.Controls;

namespace GanttChartDataGridRecurrenceSample
{
    public class RecurrentGanttChartItem : GanttChartItem
    {
        private RecurrenceType recurrenceType = RecurrenceType.Weekly;
        public RecurrenceType RecurrenceType { get { return recurrenceType; } set { recurrenceType = value; OnPropertyChanged("RecurrenceType"); } }

        private int occurrenceCount = 1;
        public int OccurrenceCount { get { return occurrenceCount; } set { occurrenceCount = Math.Max(1, value); OnPropertyChanged("OccurrenceCount"); } }
    }
}
