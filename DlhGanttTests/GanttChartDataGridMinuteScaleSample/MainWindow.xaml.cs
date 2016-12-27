﻿using System;
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
using DlhSoft.Windows.Data;
using System.Windows.Threading;

namespace GanttChartDataGridMinuteScaleSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Record the current date and time at minute level.
            DateTime now = DateTime.Now; now = now.Date.AddHours(now.Hour).AddMinutes(now.Minute);

            GanttChartItem item0 = GanttChartDataGrid.Items[0];

            GanttChartItem item1 = GanttChartDataGrid.Items[1];
            item1.Start = now.Add(TimeSpan.Parse("00:08:00"));
            item1.Finish = now.Add(TimeSpan.Parse("00:16:00"));
            item1.CompletedFinish = now.Add(TimeSpan.Parse("00:12:00"));
            item1.AssignmentsContent = "Resource 1";

            GanttChartItem item2 = GanttChartDataGrid.Items[2];
            item2.Start = now.Add(TimeSpan.Parse("00:16:00"));
            item2.Finish = now.Add(TimeSpan.Parse("00:32:00"));
            item2.AssignmentsContent = "Resource 1, Resource 2";
            item2.Predecessors.Add(new PredecessorItem { Item = item1 });

            GanttChartItem item3 = GanttChartDataGrid.Items[3];
            item3.Predecessors.Add(new PredecessorItem { Item = item0, DependencyType = DependencyType.StartStart });

            GanttChartItem item4 = GanttChartDataGrid.Items[4];
            item4.Start = now.Add(TimeSpan.Parse("00:08:00"));
            item4.Finish = now.Add(TimeSpan.Parse("00:28:00"));

            GanttChartItem item6 = GanttChartDataGrid.Items[6];
            item6.Start = now.Add(TimeSpan.Parse("00:08:00"));
            item6.Finish = now.Add(TimeSpan.Parse("00:36:00"));

            GanttChartItem item7 = GanttChartDataGrid.Items[7];
            item7.Start = now.Add(TimeSpan.Parse("00:36:00"));
            item7.IsMilestone = true;
            item7.Predecessors.Add(new PredecessorItem { Item = item4 });
            item7.Predecessors.Add(new PredecessorItem { Item = item6 });

            // Set working and visible time to 24 hours/day and 7 day/week, and nonworking time as not highlighted (as there is only working time).
            GanttChartDataGrid.WorkingDayStart = GanttChartDataGrid.VisibleDayStart = TimeOfDay.MinValue;
            GanttChartDataGrid.WorkingDayFinish = GanttChartDataGrid.VisibleDayFinish = TimeOfDay.MaxValue;
            GanttChartDataGrid.WorkingWeekStart = GanttChartDataGrid.VisibleWeekStart = DayOfWeek.Sunday;
            GanttChartDataGrid.WorkingWeekFinish = GanttChartDataGrid.VisibleWeekFinish = DayOfWeek.Saturday;
            GanttChartDataGrid.IsNonworkingTimeHighlighted = false;

            // Set timeline page start and displayed time to the numeric day origin.
            GanttChartDataGrid.SetTimelinePage(now.AddMinutes(-10), now.AddMinutes(60));
            GanttChartDataGrid.DisplayedTime = now.AddMinutes(-1);
        }

        private void GanttChartDataGrid_TimelinePageChanged(object sender, EventArgs e)
        {
            // Use Dispatcher.BeginInvoke in order to ensure that scale objects and their interval header items are properly created before setting their HeaderContent values.
            // Use DispatcherPriority.Render to apply the changes when rendering the view.
            Dispatcher.BeginInvoke((Action)delegate
            {
                // Scales use zero based indexes because non working highlighting special scale is not inserted at position zero during control initialization (behind the scenes), as we have set IsNonworkingTimeHighlighted to false.
                Scale minuteScale = GanttChartDataGrid.Scales[1];
                // Clear previous and add updated scale intervals according to the current timeline page settings.
                minuteScale.Intervals.Clear();
                for (DateTime dateTime = GanttChartDataGrid.TimelinePageStart; dateTime <= GanttChartDataGrid.TimelinePageFinish; dateTime = dateTime.AddMinutes(1))
                    minuteScale.Intervals.Add(new ScaleInterval(dateTime, dateTime.AddMinutes(1)) { HeaderContent = dateTime.ToString("mm") });
            },
            DispatcherPriority.Render);
        }
    }
}
