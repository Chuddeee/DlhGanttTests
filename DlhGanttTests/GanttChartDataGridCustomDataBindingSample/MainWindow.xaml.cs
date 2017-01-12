using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GanttChartDataGridCustomDataBindingSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;

            //GanttChartDataGrid.DataContext = new CustomTaskItem[] 
            //{
            //    new CustomTaskItem { Name = "Task 1", Description = "Description of custom task 1" },
            //    new CustomTaskItem { Name = "Task 1.1", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.Add(TimeSpan.Parse("16:00:00")), CompletionCurrentDate = DateTime.Today.Add(TimeSpan.Parse("12:00:00")), AssignmentsString = "Resource 1", Description = "Description of custom task 1.1" },
            //    new CustomTaskItem { Name = "Task 1.2", IndentLevel = 1, StartDate = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00")), AssignmentsString = "Resource 1, Resource 2", Description = "Description of custom task 1.2" },
            //    new CustomTaskItem { Name = "Task 2", Description = "Description of custom task 2" },
            //    new CustomTaskItem { Name = "Task 2.1", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.1" },
            //    new CustomTaskItem { Name = "Task 2.2", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" }
            //};

            GanttChartDataGrid.AssignableResources = new ObservableCollection<string> { "Resource 1", "Resource 2", "Resource 3",
                                                                                        "Material 1", "Material 2" };

            

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new MainWindowViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GanttChartDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Space)
            {
                GanttChartDataGrid.BeginEdit();
            }
        }
    }
}
