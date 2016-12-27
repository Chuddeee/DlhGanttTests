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

namespace GanttChartDataGridCustomDataBindingSample
{
    public class CustomTaskItem
    {
        public string Name { get; set; }
        public int IndentLevel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime CompletionCurrentDate { get; set; }
        public string AssignmentsString { get; set; }
        public string Description { get; set; }
    }
}
