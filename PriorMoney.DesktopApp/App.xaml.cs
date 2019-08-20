using PriorMoney.DesktopApp.Startup;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace PriorMoney.DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            UnityInitializer.Configure();
            var container = UnityInitializer.GetConfiguredContainer();

            MainWindow = container.Resolve<MainWindow>();
            MainWindow.Show();
        }
    }
}
