using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DlhSoft.Windows.Controls;

namespace GanttChartDataGridCustomDataBindingSample
{
    public class CustomGanttChartItem : GanttChartItem, INotifyPropertyChanged
    {
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


        public DateTime? MyStartDate
        {
            get { return (DateTime?)GetValue(MyStartDateProperty); }
            set { SetValue(MyStartDateProperty, value); }
        }

        public static readonly DependencyProperty MyStartDateProperty =
            DependencyProperty.Register("MyStartDate", typeof(DateTime?), typeof(CustomGanttChartItem), new PropertyMetadata(null, OnMyStartDatePropertyChanged));

        private static void OnMyStartDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as CustomGanttChartItem;
            if (item != null)
            {
                item.OnPropertyChanged(e.Property.Name);
                item.Start = item.MyStartDate ?? DateTime.Today;
                item.BarVisibility = item.CheckBarVisibility();
            }
        }



        public DateTime? MyFinishDate
        {
            get { return (DateTime?)GetValue(MyFinishDateProperty); }
            set { SetValue(MyFinishDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyFinishDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyFinishDateProperty =
            DependencyProperty.Register("MyFinishDate", typeof(DateTime?), typeof(CustomGanttChartItem), new PropertyMetadata(null, OnMyFinishDatePropertyChanged));

        private static void OnMyFinishDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as CustomGanttChartItem;
            if (item != null)
            {
                item.OnPropertyChanged(e.Property.Name);
                item.Finish = item.MyFinishDate ?? DateTime.Today;
                item.BarVisibility = item.CheckBarVisibility();
            }
        }



        //private DateTime? myStartDate;
        //public DateTime? MyStartDate
        //{
        //    get { return myStartDate; }
        //    set
        //    {
        //        myStartDate = value;
        //        Start = MyStartDate ?? DateTime.Today;

        //        BarVisibility = CheckBarVisibility();

        //        OnPropertyChanged(nameof(MyStartDate));
        //    }
        //}

        //private DateTime? myFinishDate;

        //public DateTime? MyFinishDate
        //{
        //    get { return myFinishDate; }
        //    set
        //    {
        //        myFinishDate = value;
        //        Finish = MyFinishDate ?? DateTime.Today;

        //        BarVisibility = CheckBarVisibility();

        //        OnPropertyChanged(nameof(MyFinishDate));
        //    }
        //}

        private Visibility CheckBarVisibility()
        {
            if (MyStartDate.HasValue && MyFinishDate.HasValue)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }
    }
}
