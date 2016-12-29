using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _003.ClearDateTimeColumnInGrid
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GanttChartDataGrid.DataContext = new TaskItem[]
         {
                new TaskItem { Name = "Task 1", Description = "Description of custom task 1" },
                new TaskItem { Name = "Task 1.1", IndentLevel = 1, StartDate = null, FinishDate = DateTime.Today.Add(TimeSpan.Parse("16:00:00")), CompletionCurrentDate = DateTime.Today.Add(TimeSpan.Parse("12:00:00")), AssignmentsString = "Resource 1", Description = "Description of custom task 1.1" },
                new TaskItem { Name = "Task 1.2", IndentLevel = 1, StartDate =null, FinishDate = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00")), AssignmentsString = "Resource 1, Resource 2", Description = "Description of custom task 1.2" },
                new TaskItem { Name = "Task 2", Description = "Description of custom task 2" },
                new TaskItem { Name = "Task 2.1", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.1" },
                new TaskItem { Name = "Task 2.2", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" }
         };
        }
    }
}
