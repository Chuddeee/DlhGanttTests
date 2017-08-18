using DlhSoft.Windows.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DynamicTabItems
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dc = (MainWindowViewModel)DataContext;
            dc.AddTabItem();
        }

        public static Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null) return null;
            if (element.GetType() == type) return element;
            Visual foundElement = null;
            if (element is FrameworkElement)
                (element as FrameworkElement).ApplyTemplate();

            for (int i = 0;
                i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                    break;
            }
            return foundElement;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var grid = NhpTabControl.SelectedContent as MyGantt;
            var gantt = GetDescendantByType(NhpTabControl, typeof(MyGantt)) as MyGantt;

            if (gantt == null)
                throw new Exception("MyGantt wasn't found");

            var ganttGrid = ((Grid)gantt.Content).Children[0] as GanttChartDataGrid;

            if (gantt!=null)
            {
                MessageBox.Show("GanttChartDataGrid found successfully");
            }
        }

        private void CloseTabButton_Click(object sender, RoutedEventArgs e)
        {
            var tabControl = NhpTabControl;
            var dc = (MainWindowViewModel)DataContext;
            dc.TabItems.Remove(dc.SelectedTab);
            
        }
    }
}
