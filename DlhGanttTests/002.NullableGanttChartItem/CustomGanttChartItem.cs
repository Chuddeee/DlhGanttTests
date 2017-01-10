using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DlhSoft.Windows.Controls;

namespace _002.NullableGanttChartItem
{
    public class CustomGanttChartItem : GanttChartItem
    {


        public DateTime? MyStartDate
        {
            get { return (DateTime?)GetValue(MyStartDateProperty); }
            set
            {
                SetValue(MyStartDateProperty, value);
                Start = MyStartDate ?? DateTime.Today;
            }
        }

        public static readonly DependencyProperty MyStartDateProperty = DependencyProperty.Register("MyStartDate", typeof(DateTime?), typeof(CustomGanttChartItem), new PropertyMetadata(null));



        public DateTime? MyFinishDate
        {
            get { return (DateTime?)GetValue(MyFinishDataProperty); }
            set
            {
                SetValue(MyFinishDataProperty, value);
                Finish = MyFinishDate ?? DateTime.Today;
            }
        }

        public static readonly DependencyProperty MyFinishDataProperty = DependencyProperty.Register("MyFinishDate", typeof(DateTime?), typeof(CustomGanttChartItem), new PropertyMetadata(null));

        private decimal extraCosts;
        public decimal ExtraCosts
        {
            get { return extraCosts; }
            set
            {
                if (value < 0)
                    value = 0;

                extraCosts = value;
                OnPropertyChanged("ExtraCosts");

                if (GanttChartView == null)
                    return;
                CustomGanttChartItem parent = GanttChartView.GetParent(this) as CustomGanttChartItem;
                if (parent == null)
                    return;
                parent.ExtraCosts = GanttChartView.GetChildren(parent).Sum(item => (item as CustomGanttChartItem).ExtraCosts);
            }
        }

    }
}
