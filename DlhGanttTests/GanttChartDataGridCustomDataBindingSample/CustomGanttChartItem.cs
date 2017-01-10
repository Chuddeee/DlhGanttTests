using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DlhSoft.Windows.Controls;

namespace GanttChartDataGridCustomDataBindingSample
{
    class CustomGanttChartItem : GanttChartItem, INotifyPropertyChanged
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

        private DateTime? myStartDate;

        public DateTime? MyStartDate
        {
            get { return myStartDate; }
            set
            {
                myStartDate = value;
                Start = MyStartDate ?? DateTime.Today;

                BarVisibility = CheckBarVisibility();

                OnPropertyChanged(nameof(MyStartDate));
            }
        }

        private DateTime? myFinishDate;

        public DateTime? MyFinishDate
        {
            get { return myFinishDate; }
            set
            {
                myFinishDate = value;
                Finish = MyFinishDate ?? DateTime.Today;

                BarVisibility = CheckBarVisibility();

                OnPropertyChanged(nameof(MyFinishDate));
            }
        }

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
