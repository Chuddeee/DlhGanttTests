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

namespace GanttChartDataGridBarTemplatingSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string applicationName = Application.Current.GetType().Namespace;

            CustomGanttChartItem item0 = GanttChartDataGrid.Items[0] as CustomGanttChartItem;

            CustomGanttChartItem item1 = GanttChartDataGrid.Items[1] as CustomGanttChartItem;
            item1.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item1.Finish = DateTime.Today.Add(TimeSpan.Parse("16:00:00"));
            item1.CompletedFinish = DateTime.Today.Add(TimeSpan.Parse("12:00:00"));
            item1.AssignmentsContent = "Resource 1";
            item1.Icon = new BitmapImage(new Uri(string.Format("/{0};component/Check.png", applicationName), UriKind.Relative));
            item1.EstimatedStart = DateTime.Today.AddDays(-1).Add(TimeSpan.Parse("08:00:00"));
            item1.EstimatedFinish = DateTime.Today.AddDays(-1).Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem item2 = GanttChartDataGrid.Items[2] as CustomGanttChartItem;
            item2.Start = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00"));
            item2.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00"));
            item2.AssignmentsContent = "Resource 1, Resource 2";
            item2.Predecessors.Add(new PredecessorItem { Item = item1 });
            item2.Icon = new BitmapImage(new Uri(string.Format("/{0};component/Person.png", applicationName), UriKind.Relative));
            item2.Note = "This task is very important.";
            item2.EstimatedStart = DateTime.Today.AddDays(1 - 1).Add(TimeSpan.Parse("08:00:00"));
            item2.EstimatedFinish = DateTime.Today.AddDays(2 + 1).Add(TimeSpan.Parse("16:00:00"));

            CustomGanttChartItem item3 = GanttChartDataGrid.Items[3] as CustomGanttChartItem;
            item3.Predecessors.Add(new PredecessorItem { Item = item0, DependencyType = DependencyType.StartStart });

            CustomGanttChartItem item4 = GanttChartDataGrid.Items[4] as CustomGanttChartItem;
            item4.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item4.Finish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"));
            item4.EstimatedStart = DateTime.Today.AddDays(+1).Add(TimeSpan.Parse("08:00:00"));
            item4.EstimatedFinish = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00"));
            item4.Markers.Add(new Marker { DateTime = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("09:00:00")), Icon = new BitmapImage(new Uri(string.Format("/{0};component/Warning.png", applicationName), UriKind.Relative)), Note = "Validation required" });
            item4.Markers.Add(new Marker { DateTime = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("14:00:00")), Icon = new BitmapImage(new Uri(string.Format("/{0};component/Error.png", applicationName), UriKind.Relative)), Note = "Impossible to finish the task" });

            CustomGanttChartItem item6 = GanttChartDataGrid.Items[6] as CustomGanttChartItem;
            item6.Start = DateTime.Today.Add(TimeSpan.Parse("08:00:00"));
            item6.Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00"));
            item6.EstimatedStart = DateTime.Today.AddDays(+1).Add(TimeSpan.Parse("08:00:00"));
            item6.EstimatedFinish = DateTime.Today.AddDays(3 - 1).Add(TimeSpan.Parse("12:00:00"));
            item6.Interruptions.Add(new Interruption { Start = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("14:00:00")), Finish = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("10:00:00")) });

            CustomGanttChartItem item7 = GanttChartDataGrid.Items[7] as CustomGanttChartItem;
            item7.Start = DateTime.Today.AddDays(4);
            item7.IsMilestone = true;
            item7.Predecessors.Add(new PredecessorItem { Item = item4 });
            item7.Predecessors.Add(new PredecessorItem { Item = item6 });
        }
    }
}
