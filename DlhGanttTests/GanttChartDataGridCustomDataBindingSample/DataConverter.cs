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
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DlhSoft.Windows.Controls;
using System.ComponentModel;

namespace GanttChartDataGridCustomDataBindingSample
{
    public class DataConverter : IValueConverter
    {
        static DataConverter()
        {
            Instance = new DataConverter();
        }

        public static DataConverter Instance { get; private set; }

        // Retrieve a collection of GanttChartItem based on the CustomTaskItem data context.
        // Формируем коллекцию CustomChartItem на основе привязанных TaskItem'ов
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<TaskItem> tasks = value as IEnumerable<TaskItem>;
            ObservableCollection<GanttChartItem> items = new ObservableCollection<GanttChartItem>();
            foreach (TaskItem customTaskItem in tasks)
            {
                CustomGanttChartItem item =
                    new CustomGanttChartItem
                    {
                        Content = customTaskItem.Name,
                        Indentation = customTaskItem.IndentLevel,
                        Start = customTaskItem.StartDate,
                        Finish = customTaskItem.FinishDate,
                        CompletedFinish = customTaskItem.CompletionCurrentDate,
                        AssignmentsContent = customTaskItem.AssignmentsString,
                        // Set the associated CustomTaskItem object as the Tag value of the GanttChartItem.
                        Tag = customTaskItem,
                        // CustomGanttChart
                        ExtraCosts = customTaskItem.ExtraCosts,
                        MyStartDate = customTaskItem.MyStartDate,
                        MyFinishDate = customTaskItem.MyFinishDate

                    };
                items.Add(item);
                item.PropertyChanged += Item_PropertyChanged;
            }
            return items;
        }

        // When a managed property changes on a GanttChartItem, propagate the updated value to the appropriate property of the associated CustomTaskItem object.
        // Когда  изменяется свойство в GanttChartItem измененное свойство передается в ассоциированное с ним свойство в CustomTaskItem
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CustomGanttChartItem item = sender as CustomGanttChartItem;
            TaskItem customTaskItem = item.Tag as TaskItem;
            switch (e.PropertyName)
            {
                case "Content":
                    customTaskItem.Name = item.Content as string;
                    break;
                case "Start":
                    customTaskItem.StartDate = item.Start;
                    break;
                case "Finish":
                    customTaskItem.FinishDate = item.Finish;
                    break;
                case "CompletedFinish":
                    customTaskItem.CompletionCurrentDate = item.CompletedFinish;
                    break;
                case "AssignmentsContent":
                    customTaskItem.AssignmentsString = item.AssignmentsContent as string;
                    break;
                case "ExtraCost":
                    customTaskItem.ExtraCosts = item.ExtraCosts;
                    break;
                case "MyStartDate":
                    customTaskItem.MyStartDate = item.MyStartDate;
                    break;
                case "MyFinishDate":
                    customTaskItem.MyFinishDate = item.MyFinishDate;
                    break;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
