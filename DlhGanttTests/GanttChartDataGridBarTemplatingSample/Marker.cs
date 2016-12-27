﻿using System;
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
    public class Marker : INotifyPropertyChanged
    {
        public CustomGanttChartItem Item { get; internal set; }
        public IGanttChartView GanttChartView { get { return Item != null ? Item.GanttChartView : null; } }

        private DateTime dateTime;
        public DateTime DateTime
        {
            get { return dateTime; }
            set
            {
                dateTime = value;
                OnPropertyChanged("DateTime");
            }
        }
        public double ComputedLeft
        {
            get { return GanttChartView.GetPosition(DateTime) - GanttChartView.GetPosition(Item.Start); }
        }

        private ImageSource icon;
        public ImageSource Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                OnPropertyChanged("Icon");
            }
        }

        private string note;
        public string Note
        {
            get { return note; }
            set
            {
                note = value;
                OnPropertyChanged("Note");
            }
        }

        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            switch (propertyName)
            {
                case "DateTime":
                    OnPropertyChanged("ComputedLeft");
                    break;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
