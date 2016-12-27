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
    public class CustomDataConverter : IValueConverter
    {
        // Retrieve a collection of GanttChartItem based on the CustomTaskItem data context.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<CustomTaskItem> dataContext = value as IEnumerable<CustomTaskItem>;
            ObservableCollection<GanttChartItem> items = new ObservableCollection<GanttChartItem>();
            foreach (CustomTaskItem customTaskItem in dataContext)
            {
                GanttChartItem item =
                    new GanttChartItem
                    {
                        Content = customTaskItem.Name,
                        Indentation = customTaskItem.IndentLevel,
                        Start = customTaskItem.StartDate,
                        Finish = customTaskItem.FinishDate,
                        CompletedFinish = customTaskItem.CompletionCurrentDate,
                        AssignmentsContent = customTaskItem.AssignmentsString,
                        // Set the associated CustomTaskItem object as the Tag value of the GanttChartItem.
                        Tag = customTaskItem
                    };
                items.Add(item);
                item.PropertyChanged += Item_PropertyChanged;
            }
            return items;
        }

        // When a managed property changes on a GanttChartItem, propagate the updated value to the appropriate property of the associated CustomTaskItem object.
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            GanttChartItem item = sender as GanttChartItem;
            CustomTaskItem customTaskItem = item.Tag as CustomTaskItem;
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
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
