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
using DlhSoft.Windows.Shapes;

namespace GanttChartDataGridMouseEventHandlingSample
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
            item1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item1.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item1.AssignmentsContent = "Resource 1";

            GanttChartItem item2 = GanttChartDataGrid.Items[2];
            item2.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item2.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item2.AssignmentsContent = "Resource 1, Resource 2";
            item2.Predecessors.Add(new PredecessorItem { Item = item1 });

            GanttChartItem item3 = GanttChartDataGrid.Items[3];
            item3.Predecessors.Add(new PredecessorItem { Item = item0, DependencyType = DependencyType.StartStart });

            GanttChartItem item4 = GanttChartDataGrid.Items[4];
            item4.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item4.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"));

            GanttChartItem item6 = GanttChartDataGrid.Items[6];
            item6.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item6.Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));

            GanttChartItem item7 = GanttChartDataGrid.Items[7];
            item7.Start = DateTime.Today.AddDays(4);
            item7.IsMilestone = true;
            item7.Predecessors.Add(new PredecessorItem { Item = item4 });
            item7.Predecessors.Add(new PredecessorItem { Item = item6 });
        }

        private void GanttChartDataGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point controlPosition = e.GetPosition(GanttChartDataGrid);
            Point contentPosition = e.GetPosition(GanttChartDataGrid.ChartContentElement);

            DateTime dateTime = GanttChartDataGrid.GetDateTime(contentPosition.X);
            GanttChartItem itemRow = GanttChartDataGrid.GetItemAt(contentPosition.Y);

            GanttChartItem item = null;
            PredecessorItem predecessorItem = null;
            FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
            if (frameworkElement != null)
            {
                item = frameworkElement.DataContext as GanttChartItem;
                DependencyArrowLine.LineSegment dependencyLine = frameworkElement.DataContext as DependencyArrowLine.LineSegment;
                if (dependencyLine != null)
                    predecessorItem = dependencyLine.Parent.DataContext as PredecessorItem;
            }

            if (controlPosition.X < GanttChartDataGrid.ActualWidth - GanttChartDataGrid.GanttChartView.ActualWidth)
                return;
            if (controlPosition.Y < GanttChartDataGrid.HeaderHeight)
                MessageBox.Show(string.Format("You have clicked the chart scale header at date and time {0:g}.", dateTime), "Mouse Click Details", MessageBoxButton.OK);
            else if (item != null && item.HasChildren)
                MessageBox.Show(string.Format("You have clicked the summary task item '{0}' at date and time {1:g}.", item, dateTime < item.Start ? item.Start : (dateTime > item.Finish ? item.Finish : dateTime)), "Mouse Click Details", MessageBoxButton.OK);
            else if (item != null && item.IsMilestone)
                MessageBox.Show(string.Format("You have clicked the milestone task item '{0}' at date and time {1:g}.", item, item.Start), "Mouse Click Details", MessageBoxButton.OK);
            else if (item != null)
                MessageBox.Show(string.Format("You have clicked the standard task item '{0}' at date and time {1:g}.", item, dateTime > item.Finish ? item.Finish : dateTime), "Mouse Click Details", MessageBoxButton.OK);
            else if (predecessorItem != null)
                MessageBox.Show(string.Format("You have clicked the task dependency line between '{0}' and '{1}'.", predecessorItem.DependentItem, predecessorItem.Item), "Mouse Click Details", MessageBoxButton.OK);
            else if (itemRow != null)
                MessageBox.Show(string.Format("You have clicked at date and time {0:g} within the row of item '{1}'.", dateTime, itemRow), "Mouse Click Details", MessageBoxButton.OK);
            else
                MessageBox.Show(string.Format("You have clicked at date and time {0:g} within an empty area of the chart.", dateTime), "Mouse Click Details", MessageBoxButton.OK);
        }
    }
}
