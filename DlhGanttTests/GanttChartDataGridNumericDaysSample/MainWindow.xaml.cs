using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DlhSoft.Windows.Controls;
using System.Windows.Threading;

namespace GanttChartDataGridNumericDaysSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GanttChartItem item0 = GanttChartDataGrid.Items[0];

            GanttChartItem item1 = GanttChartDataGrid.Items[1];
            // Use the numeric day origin (defined as a static value) for date and time values of Gantt Chart items.
            item1.Start = NumericDayOrigin;
            item1.Finish = NumericDayOrigin.AddDays(1);
            item1.CompletedFinish = NumericDayOrigin.AddDays(1);
            item1.AssignmentsContent = "Resource 1";

            GanttChartItem item2 = GanttChartDataGrid.Items[2];
            item2.Start = NumericDayOrigin.AddDays(1);
            item2.Finish = NumericDayOrigin.AddDays(3);
            item2.CompletedFinish = NumericDayOrigin.AddDays(1);
            item2.AssignmentsContent = "Resource 1, Resource 2";
            item2.Predecessors.Add(new PredecessorItem { Item = item1 });

            GanttChartItem item3 = GanttChartDataGrid.Items[3];
            item3.Predecessors.Add(new PredecessorItem { Item = item0, DependencyType = DependencyType.StartStart });

            GanttChartItem item4 = GanttChartDataGrid.Items[4];
            item4.Start = NumericDayOrigin;
            item4.Finish = NumericDayOrigin.AddDays(3);
            item4.CompletedFinish = NumericDayOrigin;

            GanttChartItem item6 = GanttChartDataGrid.Items[6];
            item6.Start = NumericDayOrigin;
            item6.Finish = NumericDayOrigin.AddDays(3);
            item6.CompletedFinish = NumericDayOrigin;

            GanttChartItem item7 = GanttChartDataGrid.Items[7];
            item7.Start = NumericDayOrigin.AddDays(4);
            item7.IsMilestone = true;
            item7.Predecessors.Add(new PredecessorItem { Item = item4 });
            item7.Predecessors.Add(new PredecessorItem { Item = item6 });

            // Set timeline page start and displayed time to the numeric day origin.
            GanttChartDataGrid.SetTimelinePage(NumericDayOrigin, NumericDayOrigin.AddDays(45));
            GanttChartDataGrid.DisplayedTime = NumericDayOrigin;
        }

        private static DateTime NumericDayOrigin { get { return NumericDayStringConverter.Origin; } }

        private void GanttChartDataGrid_TimelinePageChanged(object sender, EventArgs e)
        {
            // Use Dispatcher.BeginInvoke in order to ensure that scale objects and their interval header items are properly created before setting their HeaderContent values.
            // Use DispatcherPriority.Render to apply the changes when rendering the view.
            Dispatcher.BeginInvoke((Action)delegate
            {
                // Scales use one based indexes because a special scale (non working highlighting) is inserted at position zero during control initialization (behind the scenes).
                Scale weekScale = GanttChartDataGrid.Scales[1];
                foreach (ScaleInterval i in weekScale.Intervals)
                    i.HeaderContent = i.TimeInterval.Start.Date >= NumericDayOrigin ? string.Format("Week {0}", (int)(i.TimeInterval.Start.Date - NumericDayOrigin).TotalDays / 7 + 1) : string.Empty;
                Scale dayScale = GanttChartDataGrid.Scales[2];
                foreach (ScaleInterval i in dayScale.Intervals)
                    i.HeaderContent = i.TimeInterval.Start.Date >= NumericDayOrigin ? string.Format("{0:00}", ((int)(i.TimeInterval.Start.Date - NumericDayOrigin).TotalDays + 1) % 100) : string.Empty;
            }, 
            DispatcherPriority.Render);
        }
    }
}
