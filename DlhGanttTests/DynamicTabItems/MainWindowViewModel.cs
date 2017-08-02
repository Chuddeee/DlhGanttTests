using DlhSoft.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicTabItems
{
    public class BaseVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class TabItemVM : BaseVm
    {
        private ObservableCollection<GanttChartItem> tasks;

        public ObservableCollection<GanttChartItem> Tasks
        {
            get { return tasks; }
            set { if (tasks != value) tasks = value; OnPropertyChanged(nameof(tasks)); }
        }

        public TabItemVM(IEnumerable<GanttChartItem> tasks)
        {
            Tasks = new ObservableCollection<GanttChartItem>(tasks);
        }

    }

    public class MainWindowViewModel : BaseVm
    {
        private ObservableCollection<TabItemVM> tabItems;

        public ObservableCollection<TabItemVM> TabItems
        {
            get { return tabItems; }
            set { if (tabItems != value) tabItems = value; OnPropertyChanged(nameof(TabItems)); }
        }
        private TabItemVM selectedTab;

        public TabItemVM SelectedTab
        {
            get { return selectedTab; }
            set { if (selectedTab != value) selectedTab = value; OnPropertyChanged(nameof(SelectedTab)); }
        }

        public MainWindowViewModel()
        {
            TabItems = new ObservableCollection<TabItemVM>();
            Enumerable.Range(0, 3).ToList().ForEach(x => AddTabItem());
        }


        public void AddTabItem()
        {

            TabItems.Add(new TabItemVM(new List<GanttChartItem>
            {
                new GanttChartItem(){ Content="1", Start=DateTime.Now},
                new GanttChartItem(){ Content="2", Start=DateTime.Now},
                new GanttChartItem(){ Content="3", Start=DateTime.Now},
            }));
        }
    }
}
