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
using System.ComponentModel;
using DlhSoft.Windows.Controls;

namespace GanttChartDataGridBarTemplatingSample
{
    public class Interruption : INotifyPropertyChanged
    {
        public CustomGanttChartItem Item { get; internal set; }
        public IGanttChartView GanttChartView { get { return Item != null ? Item.GanttChartView : null; } }

        private DateTime start;
        public DateTime Start
        {
            get { return start; }
            set
            {
                start = value;
                OnPropertyChanged("Start");
            }
        }
        public double ComputedLeft
        {
            get { return GanttChartView.GetPosition(Start) - GanttChartView.GetPosition(Item.Start); }
        }

        private DateTime finish;
        public DateTime Finish
        {
            get { return finish; }
            set
            {
                finish = value;
                OnPropertyChanged("Finish");
            }
        }
        public double ComputedWidth
        {
            get { return GanttChartView.GetPosition(Finish) - GanttChartView.GetPosition(Item.Start) - ComputedLeft; }
        }

        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            switch (propertyName)
            {
                case "Start":
                    OnPropertyChanged("ComputedLeft");
                    break;
            }
            switch (propertyName)
            {
                case "Start":
                case "Finish":
                    OnPropertyChanged("ComputedWidth");
                    break;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
