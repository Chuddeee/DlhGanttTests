using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace GanttChartDataGridCustomDataBindingSample
{
    public class MainWindowViewModel
    {
        public ObservableCollection<TaskItem> Tasks { get; set; }
        public ObservableCollection<CustomGanttChartItem> Items { get; set; }

        public MainWindowViewModel()
        {
            Tasks = GetTasks(1000);
            Items = GetItems(1000);

        }

        private ObservableCollection<CustomGanttChartItem> GetItems(int n)
        {
            var result = new ObservableCollection<CustomGanttChartItem>();

            for (int i = 0; i < n; i++)
            {
                result.Add(new CustomGanttChartItem { Content = $"Task {i + 1}", Start = DateTime.Today, Finish = DateTime.Today.AddDays(1) });
            }

            return result;
        }

        private ObservableCollection<TaskItem> GetTasks(int n)
        {
            var res = new TaskItem[n];
            var array = new TaskItem[]
           {
                new TaskItem { Name = "Task 1", Description = "Description of custom task 1" },
                new TaskItem { Name = "Task 1.1", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.Add(TimeSpan.Parse("16:00:00")), CompletionCurrentDate = DateTime.Today.Add(TimeSpan.Parse("12:00:00")), AssignmentsString = "Resource 1", Description = "Description of custom task 1.1" },
                new TaskItem { Name = "Task 1.2", IndentLevel = 1, StartDate = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00")), AssignmentsString = "Resource 1, Resource 2", Description = "Description of custom task 1.2" },
                new TaskItem { Name = "Task 2", Description = "Description of custom task 2" },
                new TaskItem { Name = "Task 2.1", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.1" },
                new TaskItem { Name = "Task 2.2", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" },
                  new TaskItem { Name = "Task 2.2.1", IndentLevel = 2, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" },
                   new TaskItem { Name = "Task 2.2.2", IndentLevel = 2, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" },
                    new TaskItem { Name = "Task 3", IndentLevel = 0, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" },
                    new TaskItem { Name = "Task 3.1", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" },
                     new TaskItem { Name = "Task 3.1.1", IndentLevel = 2, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" },
                     new TaskItem { Name = "Task 3.1.1.1", IndentLevel = 3, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" },
                     new TaskItem { Name = "Task 3.1.1.1", IndentLevel = 3, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" },
                     new TaskItem { Name = "Task 3.1.2", IndentLevel = 2, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" },
                     new TaskItem { Name = "Task 3.1.2.1", IndentLevel = 3, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" },
                     new TaskItem { Name = "Task 3.1.2.2", IndentLevel = 3, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" },
                     new TaskItem { Name = "Task 4", IndentLevel = 0, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" }

           };

            for (int i = 0; i < n; i++)
            {
                res[i] = new TaskItem { Name = $"Task {i + 1}", StartDate = DateTime.Today, FinishDate = DateTime.Today.AddDays(1) };
            }
            return new ObservableCollection<TaskItem>(res);
        }
    }
}
