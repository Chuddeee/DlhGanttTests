using DlhSoft.Windows.Controls;
using System;
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
    /// Interaction logic for MyGantt.xaml
    /// </summary>
    public partial class MyGantt : UserControl
    {
        public MyGantt()
        {
            InitializeComponent();
        }

        private void GanttChartDataGrid_ItemActivated(object sender, DlhSoft.Windows.Controls.ItemActivatedEventArgs e)
        {

        }

        private void GanttChartDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (this.GanttChart.SelectedItem!=null)
            //{
            //    var item =GanttChart.SelectedItem as NhpGanttCgartItem;

            //    if (item!=null)
            //    {
            //        item.IsReadOnly = false;
            //    }
            //}
        }

        private void GanttChart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb!=null)
            {
                tb.SelectAll();
            }
        }

        private void SelectAllText (object sender, RoutedEventArgs e)
        {
            
        }
    }
}
