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

namespace GanttChartDataGridSummaryValuesSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CustomGanttChartItem item0 = GanttChartDataGrid.Items[0] as CustomGanttChartItem;

            CustomGanttChartItem item1 = GanttChartDataGrid.Items[1] as CustomGanttChartItem;
            item1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item1.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item1.AssignmentsContent = "Resource 1";

            CustomGanttChartItem item2 = GanttChartDataGrid.Items[2] as CustomGanttChartItem;
            item2.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item2.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item2.AssignmentsContent = "Resource 1, Resource 2";
            item2.Predecessors.Add(new PredecessorItem { Item = item1 });

            CustomGanttChartItem item3 = GanttChartDataGrid.Items[3] as CustomGanttChartItem;
            item3.Predecessors.Add(new PredecessorItem { Item = item0, DependencyType = DependencyType.StartStart });

            CustomGanttChartItem item4 = GanttChartDataGrid.Items[4] as CustomGanttChartItem;
            item4.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item4.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"));

            CustomGanttChartItem item6 = GanttChartDataGrid.Items[6] as CustomGanttChartItem;
            item6.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item6.Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));

            CustomGanttChartItem item7 = GanttChartDataGrid.Items[7] as CustomGanttChartItem;
            item7.Start = DateTime.Today.AddDays(4);
            item7.IsMilestone = true;
            item7.Predecessors.Add(new PredecessorItem { Item = item4 });
            item7.Predecessors.Add(new PredecessorItem { Item = item6 });

            // Turn off asynchronous presentation and apply template before setting ExtraCosts property values, in order for items to be aware of their container (GanttChartView.Items collection) and therefore to be able to aggregate summary (parent item) values automatically at initialization time also.
            GanttChartDataGrid.IsAsyncPresentationEnabled = false;
            GanttChartDataGrid.ApplyTemplate();

            item1.ExtraCosts = 200;
            item2.ExtraCosts = 300;
            item6.ExtraCosts = 600;
        }
    }
}
