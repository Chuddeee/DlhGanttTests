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


        public MainWindowViewModel()
        {
            Tasks = GetTasks();
        }
        private ObservableCollection<TaskItem> GetTasks()
        {
            var array = new TaskItem[]
           {
                new TaskItem { Name = "Task 1", Description = "Description of custom task 1" },
                new TaskItem { Name = "Task 1.1", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.Add(TimeSpan.Parse("16:00:00")), CompletionCurrentDate = DateTime.Today.Add(TimeSpan.Parse("12:00:00")), AssignmentsString = "Resource 1", Description = "Description of custom task 1.1" },
                new TaskItem { Name = "Task 1.2", IndentLevel = 1, StartDate = DateTime.Today.AddDays(1).Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("16:00:00")), AssignmentsString = "Resource 1, Resource 2", Description = "Description of custom task 1.2" },
                new TaskItem { Name = "Task 2", Description = "Description of custom task 2" },
                new TaskItem { Name = "Task 2.1", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(2).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.1" },
                new TaskItem { Name = "Task 2.2", IndentLevel = 1, StartDate = DateTime.Today.Add(TimeSpan.Parse("08:00:00")), FinishDate = DateTime.Today.AddDays(3).Add(TimeSpan.Parse("12:00:00")), Description = "Description of custom task 2.2" }
           };

            return new ObservableCollection<TaskItem>(array);
        }
    }
}
