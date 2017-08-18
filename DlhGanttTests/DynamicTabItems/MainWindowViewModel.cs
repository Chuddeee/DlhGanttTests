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
            IsReadOnly = true;
        }

        private bool isReadOnly;
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set { if (isReadOnly != value) isReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); }
        }
        private bool _isProjectTabOpen=true;

        public bool IsProjectTabOpen
        {
            get { return _isProjectTabOpen; }
            set { if (_isProjectTabOpen != value) _isProjectTabOpen = value; OnPropertyChanged(nameof(IsProjectTabOpen)); }
        }

        private bool _isEnabledCloseButton = true;

        public bool IsEnabledCloseButton
        {
            get { return _isEnabledCloseButton; }
            set { _isEnabledCloseButton = value; }
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

            TabItems.Add(new TabItemVM(new List<NhpGanttChartItem>
            {
                new NhpGanttChartItem(){ Content="1", Start=DateTime.Now},
                new NhpGanttChartItem(){ Content="2", Start=DateTime.Now},
                new NhpGanttChartItem(){ Content="3", Start=DateTime.Now},
            }));
        }
    }
}
