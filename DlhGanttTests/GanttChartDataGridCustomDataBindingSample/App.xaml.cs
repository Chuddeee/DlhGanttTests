using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using LicenseWindowKiller;

namespace GanttChartDataGridCustomDataBindingSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Task.Run(() => new Killer().Kill());

            var MainWindow = new MainWindow();
            MainWindow.DataContext = new MainWindowViewModel();
            MainWindow.Show();
        }
    }
}
